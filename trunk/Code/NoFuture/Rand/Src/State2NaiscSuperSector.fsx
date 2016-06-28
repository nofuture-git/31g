#I @"../../../packages/"
#r "FSharp.Data.2.3.0/lib/net40/FSharp.Data.dll"
#load "FSharp.Charting.0.90.14/FSharp.Charting.fsx"
#load "MathNet.Numerics.FSharp.3.11.1/MathNet.Numerics.fsx"
#r "MathNet.Numerics.3.11.1/lib/net40/MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.3.11.1/lib/net40/MathNet.Numerics.FSharp.dll"

open System
open System.Text.RegularExpressions
open System.Runtime.InteropServices
open FSharp.Charting
open FSharp.Data
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

open MathNet.Numerics
open MathNet.Numerics.Providers.LinearAlgebra.Mkl
Control.LinearAlgebraProvider <- MklLinearAlgebraProvider()

type Vec = Vector<float>
type Mat = Matrix<float>

let estimate (Y:Vec) (X:Mat) = 
    (X.Transpose() * X).Inverse() * X.Transpose() * Y

//get the Bls SM series Provider with some sample data
type BlsProvider = JsonProvider<""".\Samples\BlsSmSeriesSample.json""">
let sampleData = BlsProvider.GetSample()
type BlsResults = BlsProvider.Datum

//get BLS SM series state codes
type BlsStateProvider = CsvProvider<"""http://download.bls.gov/pub/time.series/sm/sm.state""">
let blsStateData =BlsStateProvider.Load("""http://download.bls.gov/pub/time.series/sm/sm.state""")

type EconSector =
    | MiningAndLogging
    | Construction
    | Manufacturing
    | TradeTransportationUtilities
    | Information
    | ProfessionalServices
    | LeisureHospitality
    | OtherServices
    | Government

let getStateData (stateName:string) (ss:EconSector) = 
    let getStateCodeByName stateName =
        let matchOnName = blsStateData.Rows |> Seq.find (fun x -> x.State_name = stateName)
        matchOnName.State_code

    let convertStateId2Code (stateId:int) =
        sprintf "SMU%02i" stateId

    let convertStateSector (stateId:int) (ss:EconSector) = 
        let stateCode = convertStateId2Code stateId
        match ss with
            | MiningAndLogging -> stateCode + "000001"
            | Construction -> stateCode + "000002"
            | Manufacturing -> stateCode + "000003"
            | TradeTransportationUtilities -> stateCode + "000004"
            | Information -> stateCode + "000005"
            | ProfessionalServices -> stateCode + "000006"
            | LeisureHospitality -> stateCode + "000007"
            | OtherServices -> stateCode + "000008"
            | Government -> stateCode + "000009"

    let convertEconSectorToCode (ss:EconSector) =
        match ss with
            | MiningAndLogging -> "10"
            | Construction -> "20"
            | Manufacturing -> "30"
            | TradeTransportationUtilities -> "40"
            | Information -> "50"
            | ProfessionalServices -> "60"
            | LeisureHospitality -> "70"
            | OtherServices -> "80"
            | Government -> "90"

    let getYearAsDec (period:string) = 
        let periodNum = period.Replace("M0","").Replace("M","")
        float ((System.Convert.ToDouble(periodNum)-1.)  / 12.0)

    let filteredByState (stateCode:string)  (ss:EconSector) =
         let econSectorCode = convertEconSectorToCode ss
         let srcDataFile = sprintf "BLS_SMU.%s.json" econSectorCode
         let srcData = System.IO.Path.Combine(__SOURCE_DIRECTORY__, srcDataFile)
         let latestData = BlsProvider.Load(srcData)
         latestData.Results.Series 
         |> Seq.toList 
         |> List.filter (fun x -> x.SeriesId.StartsWith(stateCode)) 
         |> List.map (fun x -> x.Data)

    let year2ValuePlotter (blsSmData:BlsResults[]) = 
        blsSmData
        |> Seq.toList 
        |> List.filter (fun x -> x.Period <> "M13") 
        |> List.map(fun x -> (float x.Year + (getYearAsDec x.Period), float x.Value))

    let stateId = getStateCodeByName stateName
    let asCode = convertStateSector stateId ss
    let whatever = filteredByState asCode ss
    whatever |> List.map (fun x -> year2ValuePlotter x) |> List.concat


//to Chart results
let chartStateAndSectorJobData stateName (ss:EconSector) = 
    let stateData = getStateData stateName ss
    let chartTitle = sprintf "%s - %A" stateName ss
    Chart.Point([for x in stateData -> x], Name=chartTitle,XTitle="Years",YTitle="Jobs in thousands")

let chartAllSectorJobDataByState stateName =
    Chart.Combine [
        Chart.Point([for x in (getStateData stateName MiningAndLogging) -> x], Name="MiningAndLogging") 
        Chart.Point([for x in (getStateData stateName Construction) -> x], Name="Construction") 
        Chart.Point([for x in (getStateData stateName Manufacturing) -> x], Name="Manufacturing") 
        Chart.Point([for x in (getStateData stateName TradeTransportationUtilities) -> x], Name="TradeTransportationUtilities") 
        Chart.Point([for x in (getStateData stateName Information) -> x], Name="Information") 
        Chart.Point([for x in (getStateData stateName ProfessionalServices) -> x], Name="ProfessionalServices") 
        Chart.Point([for x in (getStateData stateName LeisureHospitality) -> x], Name="LeisureHospitality") 
        Chart.Point([for x in (getStateData stateName OtherServices) -> x], Name="OtherServices") 
        Chart.Point([for x in (getStateData stateName Government) -> x], Name="Government") 
    ]
    |> Chart.WithLegend(InsideArea=false)

let outFileFullName = """C:\Projects\31g\trunk\Code\NoFuture\Rand\Src\SectorEmplByState.txt"""
let header = sprintf "%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\n" "State" "Mining" "Construction" "Manufacturing" "Trade" "Information" "Professional" "Leisure" "Other" "Government"
System.IO.File.AppendAllText(outFileFullName, header)


let getSectorAvgByState stateName =
    let mining = [for x in (getStateData stateName MiningAndLogging) -> snd x] |> List.average
    let ctortn = [for x in (getStateData stateName Construction) -> snd x] |> List.average
    let mfactg = [for x in (getStateData stateName Manufacturing) -> snd x] |> List.average
    let trade = [for x in (getStateData stateName TradeTransportationUtilities) -> snd x] |> List.average
    let info = [for x in (getStateData stateName Information) -> snd x] |> List.average
    let proSvc = [for x in (getStateData stateName ProfessionalServices) -> snd x] |> List.average
    let hostp = [for x in (getStateData stateName LeisureHospitality) -> snd x] |> List.average
    let oother = [for x in (getStateData stateName OtherServices) -> snd x] |> List.average
    let govt = [for x in (getStateData stateName Government) -> snd x] |> List.average

    let total = mining + ctortn + mfactg + trade + info + proSvc + hostp + oother + govt
    let lineItem = sprintf "%s\t%f\t%f\t%f\t%f\t%f\t%f\t%f\t%f\t%f\n" stateName (mining/total) (ctortn/total) (mfactg/total) (trade/total) (info/total) (proSvc/total) (hostp/total) (oother/total) (govt/total)
    System.IO.File.AppendAllText(outFileFullName, lineItem)
    



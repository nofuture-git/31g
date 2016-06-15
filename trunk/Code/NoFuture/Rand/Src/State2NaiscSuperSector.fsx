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


//an example
let stateData = getStateData "North Dakota" MiningAndLogging

Chart.Point [  for x in stateData -> x ]


//TODO - get the linear fx

//
//let blsStateCodes = blsStateData.Rows |> Seq.toList  |> List.map(fun x -> x.State_code)
//
//let blsSector = [for x in 1 .. 9 -> x * 10]
//
//let rec cartesian xs ys =
//    xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))
//
////get the number of sectors times the number of states
//let blsSmSeriesTable = cartesian blsSector blsStateCodes
//
//let writeAllRsltsToFile (sector:int) (stateCode:int) (jsonDataFile:string)= 
//    
//    let matchStateRec = blsStateData.Rows |> Seq.find (fun x -> x.State_code = stateCode)
//    let stateName = matchStateRec.State_name
//    let blsSmData = BlsProvider.Load(jsonDataFile)
//
//    if blsSmData.Results.Series.Length > 0 then
//        
//        let year2Value = blsSmData.Results.Series.[0].Data 
//                                  |> Seq.toList 
//                                  |> List.filter (fun x -> x.Period <> "M13") 
//                                  |> List.map(fun x -> (float x.Year + (getYearAsDec x.Period), float x.Value))
//
//        let years = year2Value |> List.map (fun x -> fst x )
//        let allValues = year2Value |> List.map (fun x -> snd x)
//
//        if years.Length > 0 then
//
//            let yearsMatrix = matrix [for y in years -> [1.; y]]
//            let valueVector = vector [for v in allValues -> v]
//
//            let testEstimate = estimate valueVector yearsMatrix
//            let lineData = System.String.Format("{0}\t{1}\t{2}\t{3}\n",stateName,sector,testEstimate.[0],testEstimate.[1])
//            System.IO.File.AppendAllText((System.IO.Path.Combine(System.Environment.CurrentDirectory, """BlsEmplSector2State.tsv""")), lineData)


//TODO - need to set BlsApi Reg key or will blow out my limit 
//for (sector, stateCode) in blsSmSeriesTable do
//    writeRsltToFile sector stateCode

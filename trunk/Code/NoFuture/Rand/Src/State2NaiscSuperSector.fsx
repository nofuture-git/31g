#I @"../../../packages/"
#r "FSharp.Data.2.3.0/lib/net40/FSharp.Data.dll"
#load "FSharp.Charting.0.90.14/FSharp.Charting.fsx"
#load "MathNet.Numerics.FSharp.3.11.1/MathNet.Numerics.fsx"
#r "MathNet.Numerics.3.11.1/lib/net40/MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.3.11.1/lib/net40/MathNet.Numerics.FSharp.dll"

open System
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

//get BLS SM series state codes
type BlsStateProvider = CsvProvider<"""http://download.bls.gov/pub/time.series/sm/sm.state""">
let blsStateData =BlsStateProvider.Load("""http://download.bls.gov/pub/time.series/sm/sm.state""")

let blsStateCodes = blsStateData.Rows |> Seq.toList  |> List.map(fun x -> x.State_code)

let blsSector = [for x in 1 .. 9 -> x * 10]

let rec cartesian xs ys =
    xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

//get the number of sectors times the number of states
let blsSmSeriesTable = cartesian blsSector blsStateCodes

let getYearAsDec (period:string) = 
    let periodNum = period.Replace("M0","").Replace("M","")
    float ((System.Convert.ToDouble(periodNum)-1.)  / 12.0)

let writeRsltToFile (sector:int) (stateCode:int) = 
    
    let blsUri = sprintf """http://api.bls.gov/publicAPI/v1/timeseries/data/SMU%02i00000%02i00000001""" stateCode sector
    let matchStateRec = blsStateData.Rows |> Seq.find (fun x -> x.State_code = stateCode)
    let stateName = matchStateRec.State_name

    let blsSmData = BlsProvider.Load(blsUri)
    printfn "%s" blsUri
    if blsSmData.Results.Series.Length > 0 then

        let testDataYear2Value = blsSmData.Results.Series.[0].Data 
                                  |> Seq.toList 
                                  |> List.filter (fun x -> x.Period <> "M13") 
                                  |> List.map(fun x -> (float x.Year + (getYearAsDec x.Period), float x.Value))

        let years = testDataYear2Value |> List.map (fun x -> fst x )
        let allValues = testDataYear2Value |> List.map (fun x -> snd x)

        if years.Length > 0 then

            let yearsMatrix = matrix [for y in years -> [1.; y]]
            let valueVector = vector [for v in allValues -> v]

            let testEstimate = estimate valueVector yearsMatrix
            let lineData = System.String.Format("{0}\t{1}\t{2}\t{3}\n",stateName,sector,testEstimate.[0],testEstimate.[1])
            System.IO.File.AppendAllText("""C:\Projects\31g\trunk\temp\BlsEmplSector2State.tsv""", lineData)


//TODO - need to set BlsApi Reg key or will blow out my limit 
//for (sector, stateCode) in blsSmSeriesTable do
//    writeRsltToFile sector stateCode

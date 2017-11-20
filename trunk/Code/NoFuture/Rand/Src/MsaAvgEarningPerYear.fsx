#I @"../../../packages/"
#r "FSharp.Data.2.4.2/lib/net45/FSharp.Data.dll"
#load "FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.fsx"
#load "MathNet.Numerics.FSharp.3.20.0/MathNet.Numerics.fsx"
#r "MathNet.Numerics.3.20.0/lib/net40/MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.3.20.0/lib/net40/MathNet.Numerics.FSharp.dll"

open System
open System.Runtime.InteropServices
open FSharp.Data
open FSharp.Charting
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics
open MathNet.Numerics.Providers.LinearAlgebra.Mkl

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
Control.LinearAlgebraProvider <- MklLinearAlgebraProvider()

type BeaCountyData = JsonProvider<"Samples/BeaRegionalDataPJEARN_MI.Sample.json">

type Vec = Vector<float>
type Mat = Matrix<float>

let beaCountyDataSet = BeaCountyData.Load("BeaRegionalDataPJEARN_MI.Msa.json")

type BeaDataItem = BeaCountyData.Datum

let getDistinctYears (arrayOfBeaItems:BeaDataItem[]) = 
    arrayOfBeaItems 
    |> Seq.toList 
    |> List.map(fun x -> float x.TimePeriod) 
    |> Set.ofList 
    |> Set.toList

let getDistinctFips (arrayOfBeaItems:BeaDataItem[]) = 
    arrayOfBeaItems
    |> Seq.toList 
    |> List.map(fun x -> (string x.GeoFips, x.GeoName)) 
    |> Set.ofList 
    |> Set.toList

let getDataByGeoCode (mCode:string) (arrayOfBeaItems:BeaDataItem[])  =
    arrayOfBeaItems 
    |> Seq.toList 
    |> List.filter (fun x -> System.String.Equals(string x.GeoFips,mCode)) 
    |> List.sortBy (fun x -> x.TimePeriod) 
    |> List.map (fun x -> x.DataValue)

let estimate (Y:Vec) (X:Mat) = 
    (X.Transpose() * X).Inverse() * X.Transpose() * Y

let generateEstTsvData disFips (arrayOfBeaItems:BeaDataItem[]) outFileFullName = 
    let header = sprintf "%s\t%s\t%s\t%s" "GeoCode" "Name" "Intercept" "Slope"
    System.IO.File.AppendAllText(outFileFullName, header)
    
    let myX = matrix [for y in (getDistinctYears arrayOfBeaItems) -> [1.; y]]

    for (msaCode, cityName) in disFips do
        let cityData = getDataByGeoCode msaCode arrayOfBeaItems
        let cityY = vector [for w in cityData -> float w]
        let estReg = estimate cityY myX
        let lineItem = sprintf "%s\t%s\t%f\t%f" msaCode cityName estReg.[0] estReg.[1]
        System.IO.File.AppendAllText(outFileFullName, lineItem)
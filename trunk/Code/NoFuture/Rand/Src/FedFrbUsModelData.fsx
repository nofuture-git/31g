#I @"../../../packages/"
#r "FSharp.Data.2.3.0/lib/net40/FSharp.Data.dll"
#load "FSharp.Charting.0.90.14/FSharp.Charting.fsx"
#load "MathNet.Numerics.FSharp.3.11.1/MathNet.Numerics.fsx"
#r "MathNet.Numerics.3.11.1/lib/net40/MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.3.11.1/lib/net40/MathNet.Numerics.FSharp.dll"
#load "KMeans.fs"
#load "AkaikeInfoCriterion.fs"
#load "PCA.fs"
open NoFuture.Rand.Src.KMeans
open NoFuture.Rand.Src.AkaikeInfoCriterion
open NoFuture.Rand.Src.PCA

open System
open System.IO
open System.Text.RegularExpressions
open System.Runtime.InteropServices
open FSharp.Charting
open FSharp.Data
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics
open MathNet.Numerics.Providers.LinearAlgebra.Mkl
open MathNet.Numerics.Statistics
Control.LinearAlgebraProvider <- MklLinearAlgebraProvider()

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

type Vec = Vector<float>
type Mat = Matrix<float>

let estimate (Y:Vec) (X:Mat) = 
    (X.Transpose() * X).Inverse() * X.Transpose() * Y

let headers,observations = 
    let raw =
        Path.Combine(__SOURCE_DIRECTORY__, """.\Samples\data_only_package\HISTDATA.TXT""")
        |> File.ReadAllLines

    let headers = (raw.[0].Split ',').[1..]

    let observations = 
        raw.[1..]
        |> Array.map (fun line -> (line.Split ',').[1..])
        |> Array.map (Array.map float)

    headers, observations

//basic dataset statistics
printfn "%32s %8s %8s %8s " "Header Name" "Avg" "Min" "Max"
headers
|> Array.iteri (fun i name -> 
    let col = observations |> Array.map (fun obs -> obs.[i])
    let avg = col |> Array.average
    let min = col |> Array.min
    let max = col |> Array.max
    printfn "%32s %8.4f %8.4f %8.4f" name avg min max)

//this gets the covariance of each feature to each 
let correlations = 
    observations
    |> Matrix.Build.DenseOfColumnArrays
    |> Matrix.toRowArrays
    |> Correlation.PearsonMatrix

//printable form of the largest 20
let feats = headers.Length
let correlated = 
    [
        for col in 0 .. (feats - 1) do
            for row in (col + 1) .. (feats - 1) -> 
                correlations.[col,row], headers.[col], headers.[row]
    ]
    |> Seq.sortBy (fun (corr, f1, f2) -> - abs corr)
    |> Seq.rev
    |> Seq.filter (fun (corr, f1, f2) -> not (System.Double.IsNaN corr))
    |> Seq.take 100
    |> Seq.iter (fun (corr, f1, f2) -> 
        printfn "%s %s : %.5f" f1 f2 corr)

let normalized = normalize (headers.Length) observations
let (eValues,eVectors), projector = pca normalized

let total = eValues |> Seq.sumBy (fun x -> x.Magnitude)
eValues
|> Vector.toList
|> List.rev
|> List.scan (fun (percent,cumul) value -> 
    let percent = 100. * value.Magnitude / total
    let cumul = cumul + percent
    (percent, cumul)) (0.,0.)
|> List.tail
|> List.iteri (fun i (p,c) -> printfn "Feat %2i: %.4f%% (%.4f%%)" i p c)

let principalComponent comp1 comp2 =
    let title = sprintf "Component %i vs %i" comp1 comp2
    let features = headers.Length
    let coords = Seq.zip (eVectors.Column(features-comp1)) (eVectors.Column(features-comp2))
    Chart.Point (coords, Title = title, Labels = headers, MarkerSize = 7)
    |> Chart.WithXAxis(Min = -1.0, Max = 1.0,
        MajorGrid = ChartTypes.Grid(Interval = 0.25),
        LabelStyle = ChartTypes.LabelStyle(Interval = 0.25),
        MajorTickMark = ChartTypes.TickMark(Enabled = false))
    |> Chart.WithYAxis(Min = -1.0, Max = 1.0,
        MajorGrid = ChartTypes.Grid(Interval = 0.25),
        LabelStyle = ChartTypes.LabelStyle(Interval = 0.25),
        MajorTickMark = ChartTypes.TickMark(Enabled = false))

principalComponent 3 4

let projections comp1 comp2 =
    let title = sprintf "Component %i vs %i" comp1 comp2
    let features = headers.Length
    let coords =
        normalized
        |> Seq.map projector
        |> Seq.map (fun obs -> obs.[features-comp1], obs.[features-comp2])
    Chart.Point (coords, Title = title)
    |> Chart.WithXAxis(Min = -100.0, Max = 200.0,
        MajorGrid = ChartTypes.Grid(Interval = 50.),
        LabelStyle = ChartTypes.LabelStyle(Interval = 50.),
        MajorTickMark = ChartTypes.TickMark(Enabled = false))
    |> Chart.WithYAxis(Min = -100.0, Max = 200.0,
        MajorGrid = ChartTypes.Grid(Interval = 50.), 
        LabelStyle = ChartTypes.LabelStyle(Interval = 50.),
        MajorTickMark = ChartTypes.TickMark(Enabled = false))
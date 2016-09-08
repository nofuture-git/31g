#I @"../../../packages/"
#r "FSharp.Data.2.3.0/lib/net40/FSharp.Data.dll"
#load "FSharp.Charting.0.90.14/FSharp.Charting.fsx"
#load "MathNet.Numerics.FSharp.3.11.1/MathNet.Numerics.fsx"
#r "MathNet.Numerics.3.11.1/lib/net40/MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.3.11.1/lib/net40/MathNet.Numerics.FSharp.dll"

open System
open System.IO
open System.Text.RegularExpressions
open System.Runtime.InteropServices
open FSharp.Charting
open FSharp.Data
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
//boilerplate F# script code
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

open MathNet.Numerics
open MathNet.Numerics.Providers.LinearAlgebra.Mkl
Control.LinearAlgebraProvider <- MklLinearAlgebraProvider()

type Vec = Vector<float>
type Mat = Matrix<float>

let estimate (Y:Vec) (X:Mat) = 
    (X.Transpose() * X).Inverse() * X.Transpose() * Y

//load up the data from the FED
type FrbUsData = CsvProvider<""".\Samples\data_only_package\HISTDATA.TXT""">
let dataset = FrbUsData.Load(""".\Samples\data_only_package\HISTDATA.TXT""")
let data = dataset.Rows

type Obs = FrbUsData.Row
type Model = Obs -> float
type Featurizer = Obs -> float list

//going to consider HGGDP has the "answer" 
// HGGDP    = Growth rate of GDP, cw 2009$ (annual rate) 
let seed = 3147159
let rng = System.Random(seed)

let shuffle (arr:'a []) =
    let arr = Array.copy arr
    let l = arr.Length
    for i in (l-1) .. -1 .. 1 do
        let temp = arr.[i]
        let j = rng.Next(0,i+1)
        arr.[i] <- arr.[j]
        arr.[j] <- temp
    arr

let training, validation = 
    let shuffled = 
        data
        |> Seq.toArray
        |> shuffle
    let size = 
        0.7 * float (Array.length shuffled) |> int
    shuffled.[..size],shuffled.[size+1..]

//convert observed value over to a predicted value
let predictor (f:Featurizer) (theta:Vec) = 
    f >> vector >> (*) theta

//determine how much our prediction is off on average
let evaluate (model:Model) (data:Obs seq) =
    data
    |> Seq.averageBy (fun obs ->
        abs (model obs - float obs.XGDP))//this is the first appearance of one of the CSV's fields

//takes a feature and training data
let model (f:Featurizer) (data:Obs seq) = 
    let Yt, Xt =
        data
        |> Seq.toList
        |> List.map (fun obs -> float obs.XGDP, f obs)
        |> List.unzip
    let theta = estimate (vector Yt) (matrix Xt)
    let predict = predictor f theta
    theta, predict

let labor (obs:Obs) = 
    [1.0
     obs.LF |> float
     obs.LF * obs.LF |> float ]

let (lfTheata,laborModel) = model labor data

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
printfn "%16s %8s %8s %8s " "Header Name" "Avg" "Min" "Max"
headers
|> Array.iteri (fun i name -> 
    let col = observations |> Array.map (fun obs -> obs.[i])
    let avg = col |> Array.average
    let min = col |> Array.min
    let max = col |> Array.max
    printfn "%16s %8.1f %8.1f %8.1f" name avg min max)

#load "KMeans.fs"
open Unsupervised.KMeans

type Observation = float []

let features = headers.Length

let distance (obs1:Observation) (obs2:Observation) =
    (obs1, obs2)
    ||> Seq.map2 (fun u1 u2 -> pown (u1 - u2) 2)
    |> Seq.sum

let centroidOf (cluster:Observation seq) =
    Array.init features (fun f ->
        cluster
        |> Seq.averageBy (fun u -> u.[f]))

let observations1 = 
    observations
    |> Array.map (Array.map float)
    |> Array.filter (fun x -> Array.sum x > 0.)

let (cluster1, classifier1) =
    let clustering = clusterize distance centroidOf
    let k = 5
    clustering observations1 k

cluster1
|> Seq.iter (fun (id,profile) -> 
    printfn "CLUSTER %i" id
    profile 
    |> Array.iteri (fun i value -> printfn "%16s %.1f" headers.[i] value))

Chart.Combine [
    for (id,profile) in cluster1 ->
        profile
        |> Seq.mapi (fun i value -> headers.[i], value)
        |> Chart.Bar
    ]
|> fun chart -> chart.WithXAxis (LabelStyle=labels)
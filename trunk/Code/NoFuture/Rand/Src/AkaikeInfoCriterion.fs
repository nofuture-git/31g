namespace NoFuture.Rand.Src
module AkaikeInfoCriterion = 
    type Observation = float[]
    let squareError (obs1:Observation) (obs2:Observation) =
        (obs1, obs2)
        ||> Seq.zip
        |> Seq.sumBy (fun (x1,x2) -> pown (x1-x2) 2)

    let RSS (dataset:Observation[]) centroids =
        dataset
        |> Seq.sumBy (fun obs -> 
            centroids
            |> Seq.map (squareError obs)
            |> Seq.min)

    let AIC (dataset:Observation[]) centroids = 
        let k = centroids |> Seq.length
        let m = dataset.[0] |> Seq.length
        RSS dataset centroids + float (2 * m * k)


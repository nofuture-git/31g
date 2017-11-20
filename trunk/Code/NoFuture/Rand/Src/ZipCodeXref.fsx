#I @"../../../packages/"
#r "FSharp.Data.2.4.2/lib/net45/FSharp.Data.dll"

open System
open System.Runtime.InteropServices
open FSharp.Data

type Zcta2CbsaProvider = CsvProvider<"""http://www2.census.gov/geo/docs/maps-data/data/rel/zcta_cbsa_rel_10.txt""">

let zip2Cbsa = Zcta2CbsaProvider.Load("""http://www2.census.gov/geo/docs/maps-data/data/rel/zcta_cbsa_rel_10.txt""")

type Zcta2CountyProvider = CsvProvider<"""http://www2.census.gov/geo/docs/maps-data/data/rel/zcta_county_rel_10.txt""">

let zip2County = Zcta2CountyProvider.Load("""http://www2.census.gov/geo/docs/maps-data/data/rel/zcta_county_rel_10.txt""")
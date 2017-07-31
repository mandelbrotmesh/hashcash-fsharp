// Learn more about F# at http://fsharp.org

open System
open System.Security.Cryptography
open System.Threading
open System.Text

let seed = 
    ()

let compute header zeroBytes =
    printfn "SEED %s " header
    let sha = System.Security.Cryptography.SHA1.Create()
    let hash = sha.ComputeHash(Encoding.UTF8.GetBytes(header))
    printfn "HASH %A " hash
    let checkBytes = hash |> Seq.take zeroBytes
    printfn "BYTES CHECK %A " checkBytes
    let zeroArray = Array.fill (Array.zeroCreate zeroBytes) zeroBytes 0 0
    printfn "ZERO ARRAY %A " zeroArray

let verify =
    ()

let randomChars len = 
    //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    let rand = System.Random() //RandomNumberGenerator.Create()
    let chars = String(Array.concat [ [|'A'..'Z'|]; [|'0'..'9'|]; [|'a'..'b'|] ])
    String(Array.init len (fun _ -> chars.[rand.Next(chars.Length)]))

type headerParts =
    {
        version: string
        bits: string
        timestamp: string
        resource: string
        extension: string
        seed: string
        counter: string
    }

[<EntryPoint>]
let main argv =
    printfn "START HASHCASH F#!"
    let dateStamp = DateTime.Now.ToString("yyMMddmmss")
    compute (randomChars 8) 1
    0 // return an integer exit code

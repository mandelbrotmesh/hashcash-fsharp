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
    let checkBytes = hash |> Array.take zeroBytes
    printfn "BYTES CHECK %A " checkBytes
    let shift = hash.[15] >>> 140
    printfn "%A" shift
    let bytesToBits (bytes:byte[])  =          
        let bitMasks = Seq.unfold (fun bitIndex -> Some((byte(pown 2 bitIndex), bitIndex), bitIndex + 1)) 0     
                            |> Seq.take 8
                            |> Seq.toList
                            |> List.rev

        let byteToBitArray b = List.map (fun (bitMask, bitPosition) -> (b &&& bitMask) >>> bitPosition) bitMasks
        bytes
            |> Array.toList
            |> List.map byteToBitArray
            |> List.collect id
            |> List.toArray
    
    let bits = bytesToBits hash
    printfn "BITS %A" bits
    printfn "LENGTH %i" bits.Length
    let bitArray = System.Collections.BitArray(hash)
    printfn "bit array %A " bitArray
    // printfn "left shift %A" (checkBytes.[0] >>>> 2)
    //create a bit mask
    //and && the arrays together
    //and math magic
    let zeroArray = Array.fill (Array.zeroCreate zeroBytes) 0 zeroBytes 0
    printfn "ZERO ARRAY %A " zeroArray
    let firstByte = (hash.[0].Equals(0))
    printfn "first %A " firstByte
    let secondByte = (hash.[1].Equals(0))
    printfn "second %A " secondByte
    let thirdByte = (hash.[2] <= byte 15)
    printfn "third %A " thirdByte
    firstByte && secondByte && thirdByte
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
    compute (randomChars 100) 1
    0 // return an integer exit code

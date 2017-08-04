// Learn more about F# at http://fsharp.org

open System
open System.Security.Cryptography
open System.Threading
open System.Text

let seed = 
    ()

let compute header counter  =
    // printfn "SEED %s " header
    let sha = System.Security.Cryptography.SHA1.Create()
    let headerPlusCounter = sprintf "%s:%i" header counter
    let hash = sha.ComputeHash(Encoding.UTF8.GetBytes(headerPlusCounter))
    // printfn "HASH %A " hash
    // let checkBytes = hash |> Array.take zeroBytes
    // printfn "BYTES CHECK %A " checkBytes
    // let shift = hash.[15] >>> 140
    // printfn "%A" shift
    // let bytesToBits (bytes:byte[])  =          
    //     let bitMasks = Seq.unfold (fun bitIndex -> Some((byte(pown 2 bitIndex), bitIndex), bitIndex + 1)) 0     
    //                         |> Seq.take 8
    //                         |> Seq.toList
    //                         |> List.rev

    //     let byteToBitArray b = List.map (fun (bitMask, bitPosition) -> (b &&& bitMask) >>> bitPosition) bitMasks
    //     bytes
    //         |> Array.toList
    //         |> List.map byteToBitArray
    //         |> List.collect id
    //         |> List.toArray
    
    // let bits = bytesToBits hash
    // printfn "BITS %A" bits
    // printfn "LENGTH %i" bits.Length
    // let bitArray = System.Collections.BitArray(hash)
    // printfn "bit array %A " bitArray
    // // printfn "left shift %A" (checkBytes.[0] >>>> 2)
    // //create a bit mask
    // //and && the arrays together
    // //and math magic
    // let zeroArray = Array.fill (Array.zeroCreate zeroBytes) 0 zeroBytes 0
    // printfn "ZERO ARRAY %A " zeroArray
    let firstByte = (hash.[0] = byte 0)
    // printfn "first %A " firstByte
    let secondByte = (hash.[1] = byte 0)
    // printfn "second %A " secondByte
    let thirdByte = (hash.[2] <= byte 15)
    // printfn "third %A " thirdByte
    // printfn "byte %A" hash.[0]
    printfn "lots of zeroes? %A" (firstByte && secondByte && thirdByte)
    firstByte && secondByte && thirdByte

    // let test = Array.init 2 (fun i -> byte(i*i))
    // printfn "zero? %A" test
    // printfn "is it zero? %A" (test.[0] = (byte 0))
    // test.[0] = byte 0
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
    let seed = randomChars 10
    let continueTaking result = 
        not <| result
    Seq.initInfinite (fun index -> index + 1)
    // |> Seq.takeWhile (fun x -> x < 10)
    |> Seq.takeWhile (fun cnt -> continueTaking((compute seed cnt)) && cnt < 1000000)
    |> Seq.iter (printfn "%i")
    0 // return an integer exit code

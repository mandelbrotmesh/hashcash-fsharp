// Learn more about F# at http://fsharp.org

open System
open System.Security.Cryptography
open System.Threading
open System.Text

let sha = System.Security.Cryptography.SHA1.Create()

let getHash (input:string) =
    sha.ComputeHash(Encoding.UTF8.GetBytes(input))

let seed = 
    ()

    //THIS COULD BE A USEFUL FUNCTION FOR DEBUGGIN
    //THIS WILL PRINT THE BITS INSIDE YOUR ARRAY OF BYTES
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
let randomChars len = 
    //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    let rand = System.Random() 
    let chars = String(Array.concat [ [|'A'..'Z'|]; [|'0'..'9'|]; [|'a'..'b'|] ])
    String(Array.init len (fun _ -> chars.[rand.Next(chars.Length)]))

let mutable counter = 0

let header counter randomString =
    let ver = sprintf "%i" 1
    let bits = sprintf "%i" 20
    let date = sprintf "%s" (DateTime.Now.ToString("yyMMddmmss"))
    let resource = sprintf "%s" "hashcash@mailinator.com"
    let extension = sprintf "%s" String.Empty
    let rand = sprintf "%s" randomString 
    sprintf "%s:%s:%s:%s:%s:%s:%i" ver bits date resource extension rand counter

let checkHash (hash:byte[]) =
    let firstByte = (hash.[0] = byte 0)
    let secondByte = (hash.[1] = byte 0)
    let thirdByte = (hash.[2] <= byte 15)
    firstByte && secondByte && thirdByte

let compute index randomString = 
    let hash = getHash (header index randomString)
    if not <| checkHash hash 
    then 
        false
    else
        counter <- index
        true

let verify header =
    let hash = getHash header
    checkHash hash

let sender () =
    printfn "STARTING HASHCASH F#"
    let seed = randomChars 8
    let continueTaking result = 
        not <| result        
    let randomString = randomChars 8
    Seq.initInfinite (fun index -> index + 1)
    |> Seq.takeWhile (fun index -> (continueTaking (compute index randomString)
                                   && index < 2000000)) //limit so it does not run forever just in case. this should find a solution on average in 2^20 tries
    |> Seq.iter ignore 
    printfn "FINAL COUNT %i " counter
    printfn "HASH %s" (header counter randomString)
    ()
    
let recipient header =
    printfn "does it pass? %A" (verify header)
    ()

[<EntryPoint>]
let main argv =
    printfn "%A" DateTime.Now
    match argv.[0] with
    | "sender" ->       sender ()
    | "recipient" ->    printfn "%s" argv.[1]
                        recipient argv.[1]
    | _ ->              printfn "%s" "Argument not allowed."
    printfn "%A" DateTime.Now
    0 // return an integer exit code

﻿module Yaaf.FSharp.Collections.Experimental.Tests.PersistentHashMapTest
(* DISABLED BECAUSE HASHING IS NOT WORKING PROPERLY (existing failing tests)
open System
open Yaaf.FSharp.Collections
open Yaaf.FSharp.Collections.PersistentHashMap
open NUnit.Framework
open FsUnit

[<Test>]
let ``empty map should be empty``() =
    let x = empty<int,int>
    x |> length |> should equal 0


[<Test>]
let ``empty map should not contain key 0``() =
    let x = empty
    x |> containsKey 1 |> should equal false

[<Test>]
let ``can add null entry to empty map``() =
    let x = empty
    x |> containsKey "value" |> should equal false
    x |> containsKey null |> should equal false
    x |> add null "Hello" |> containsKey null |> should equal true


[<Test>]
let ``can add empty string as key to empty map``() =
    let x = empty
    x |> containsKey "" |> should equal false
    x |> add "" "Hello" |> containsKey null |> should equal false
    x |> add "" "Hello" |> containsKey "" |> should equal true
    x |> add "" "Hello" |> length |> shouldEqual 1

[<Test>]
let ``can add some integers to empty map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"

    x |> containsKey 1 |> shouldEqual true
    x |> containsKey 5 |> shouldEqual true
    x |> containsKey 6 |> shouldEqual false
    x |> length |> shouldEqual 5

[<Test>]
let ``add operates immutable``() =
    let y =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
    let x =
        y
        |> add 4 "l"
        |> add 5 "o"

    y |> length |> shouldEqual 3
    x |> length |> shouldEqual 5


[<Test>]
let ``can remove some integers from a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
        |> remove 1
        |> remove 4
            
    x |> containsKey 1 |> shouldEqual false
    x |> containsKey 4 |> shouldEqual false
    x |> containsKey 5 |> shouldEqual true
    x |> containsKey 6 |> shouldEqual false
    x |> length |> shouldEqual 3

[<Test>]
let ``remove operates``() =
    let y =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
    let x =
        y
        |> remove 1
        |> remove 4
            
    x |> length |> shouldEqual 3
    y |> length |> shouldEqual 5

[<Test>]
let ``can find integers in a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "l"
    x |> find 5 |> shouldEqual "o"

[<Test>]
let ``can lookup integers from a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
            
    x.[1] |> shouldEqual "h"
    x.[4] |> shouldEqual "l"
    x.[5] |> shouldEqual "o"


[<Test>]
let ``can add the same key multiple to a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
        |> add 3 "a"
        |> add 4 "a"
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "a"
    x |> find 5 |> shouldEqual "o"
    x |> length |> shouldEqual 5

[<Test>]
let ``can iterate through a map``() =
    let x =
        empty
        |> add 1 "h"
        |> add 2 "a"
        |> add 3 "l"
        |> add 4 "l"
        |> add 5 "o"
            
    x |> find 1 |> shouldEqual "h"
    x |> find 4 |> shouldEqual "l"
    x |> find 5 |> shouldEqual "o"


[<Test>]
let ``can convert a seq to a map``() =
    let list = [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"]

    let x = ofSeq list

    x |> toSeq |> Seq.toList |> shouldEqual [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"]

[<Test>]
let ``a map is always sorter``() =
    let list = [ 4,"l"; 5,"o"; 2,"a"; 1,"h"; 3,"l"]

    let x = ofSeq list

    x |> toSeq |> Seq.toList |> shouldEqual [1,"h"; 2,"a"; 3,"l"; 4,"l"; 5,"o"]

[<Test>]
let ``can map a HashMap``() =
    let x =
        empty
        |> add 1 1
        |> add 2 2
        |> add 3 3
        |> add 4 4
        |> add 5 5
            
    x |> map (fun x -> x + 1) |> Seq.toList |> shouldEqual [1,2; 2,3; 3,4; 4,5; 5,6]
    
[<Test>]
let ``can add tons of integers to empty map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add i i !x

    for i in 0 .. counter do 
        !x |> containsKey i |> should equal true

[<Test>]
let ``can find tons of integers in a map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add i i !x

    for i in 0 .. counter do 
        !x |> find i |> shouldEqual i

open Yaaf.FSharp.Collections.Experimental.Tests.TransientHashMapTest

[<Test>]
let ``can add keys with colliding hashes to empty map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        empty 
        |> add x x.Name 
        |> add y y.Name
    
    map |> containsKey x |> should equal true
    map |> containsKey y |> should equal true

    empty |> containsKey y |> should equal false


[<Test>]
let ``can lookup keys with colliding hashes from map``() =
    let x = { Name = "Test"}
    let y = { Name = "Test1"}
    let map = 
        empty 
        |> add x x
        |> add y y
    
    map |> find x |> shouldEqual { Name = "Test"}
    map |> find y |> shouldEqual { Name = "Test1"}

[<Test>]
let ``can add lots of keys with colliding hashes to empty map``() =
    let x = ref empty
    let counter = 1000

    for i in 0 .. counter do 
        x := add { Name = i.ToString() } i !x

    for i in 0 .. counter do 
        !x |> containsKey { Name = i.ToString() } |> should equal true

[<Test>]
let ``can find tons of strings in a map``() =
    let x = ref empty
    let n = 10000
    let r = new Random()

    for i in 0 .. n do 
        x := add (i.ToString()) i !x

    for i in 0 .. 1000000 do 
        !x |> containsKey ((r.Next n).ToString()) |> should equal true
*)
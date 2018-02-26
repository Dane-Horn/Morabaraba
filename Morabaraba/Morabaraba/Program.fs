// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let boardLayout = "\
                       a1----------a4----------a7/n/
                       | '.         |        .' |/n/
                       |   b2------b4------b6   |/n/
                       |   | '.     |    .' |   |/n/
                       |   |   c3--c4--c5   |   |/n/
                       |   |   |        |   |   |/n/
                       d1--d2--d3       d5--d6--d7/n/
                       |   |   |        |   |   |/n/
                       |   |   e3--e4--e5   |   |/n/
                       |   |        |       |   |/n/
                       |   |-------f4------f6   |/n/
                       | .'         |        '. |/n/
                       g1----------g4----------g7/n"
    0 // return an integer exit code

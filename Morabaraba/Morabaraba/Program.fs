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
                       |   | .'     |    '. |   |/n/
                       |   f2-------f4------f6  |/n/
                       | .'         |        '. |/n/
                       g1----------g4----------g7/n"
    let player1Cows = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12]
    let player2Cows = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12]
    let possibleMills = [(*horizontal mills*)
                        "a1, a4, a7";
                        "b2, b4, b6";
                        "c3, c4, c5";
                        "d1, d2, d3";
                        "d5, d6, d7";
                        "e3, e4, e5";
                        "f2, f4, f6";
                        "g1, g4, g7";
                        (*vertical mills*)
                        "a1, d1, g1";
                        "b2, d2, f2";
                        "c3, d3, e3";
                        "a4, b4, c4";
                        "e4, f4, g4";
                        "c5, d5, e5";
                        "b6, d6, f6";
                        "a7, d7, g7";
                        (*diagonal mills*)
                        "a1, b1, c1";
                        "a7, b6, c5";
                        "g1, f2, e3";
                        "e5, f6, g7";]
    0 // return an integer exit code

open System

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


type Piece =
|X
|O

type Cell = 
    {   
        pos : char*int
    }
[<EntryPoint>]
let main argv = 
    let displayBoard (boardString : string) = //takes in a string of cows and places them at the proper positions
        printf"\t    1   2   3    4   5   6   7 
        A   %c------------%c-----------%c         Player1 cows 
        |   | '.         |        .' |         o o o o o o o o o o o o 
        B   |   %c--------%c-------%c   | 
        |   |   | '.     |    .' |   |         Player2 cows 
        C   |   |   %c----%c---%c   |   |         x x x x x x x x x x x x 
        |   |   |   |        |   |   | 
        D   %c---%c---%c        %c---%c---%c 
        |   |   |   |        |   |   | 
        E   |   |   %c----%c---%c   |   | 
        |   |   | .'     |    '. |   |
        F   |   %c--------%c-------%c   |
        |   | .'         |        '. |
        G   %c------------%c-----------%c "
            boardString.[0] boardString.[1] boardString.[2]//row 1
            boardString.[3] boardString.[4] boardString.[5]//row 2
            boardString.[6] boardString.[7] boardString.[8]//row 3
            boardString.[9] boardString.[10] boardString.[11] boardString.[12] boardString.[13] boardString.[14]//row 4
            boardString.[15] boardString.[16] boardString.[17]//row 5
            boardString.[18] boardString.[19] boardString.[20]//row 6
            boardString.[21] boardString.[22] boardString.[23]//row 7
    
    let possibleMills = [(*horizontal mills*)
                        ('A', 1),('A', 4), ('A', 7);
                        ('B',2), ('B',4), ('B',6);
                        ('C',3), ('C',4), ('C',5);
                        ('D',1), ('D',2), ('D',3);
                        ('D',5), ('D',6), ('D',7);
                        ('E',3), ('E',4), ('E',5);
                        ('F',2), ('F',4), ('F',6);
                        ('G',1), ('G',4), ('G',7);
                        (*vertical mills*)
                        ('A',1), ('D',1), ('G',1);
                        ('B',2), ('D',2), ('F',2);
                        ('C',3), ('D',3), ('E',3);
                        ('A',4), ('B',4), ('C',4);
                        ('E',4), ('F',4), ('G',4);
                        ('C',5), ('D',5), ('E',5);
                        ('B',6), ('D',6), ('F',6);
                        ('A',7), ('D',7), ('G',7);
                        (*diagonal mills*)
                        ('a',1), ('b',1), ('c',1);
                        ('a',7), ('b',6), ('c',5);
                        ('g',1), ('f',2), ('e',3);
                        ('e',5), ('f',6), ('g',7);]

    

    displayBoard "OOOOOOOOOOOOOOOOOOOOOOOO"
    Console.ReadLine () |> ignore
    0 // return an integer exit code

open System

let cprintfn color (msg : string) =
    let old = Console.ForegroundColor
    Console.ForegroundColor <- color
    Console.WriteLine msg
    Console.ForegroundColor <- old


let displayCredits () =
    Console.Clear ()
    printfn "Morabaraba\n\n\
        Developed by:\n\
            Dane Horn\n\
            Stephen Hunt\n\
            Stephen Ritchie\n\n\
            \
        This project was created as a part of the\n\
        CSc 302 2018 course at Rhodes University\n\
        Lectured and delivered by Yusuf Motara"


    printf "Press any key to continue"
    Console.ReadKey () |> ignore
    Console.Clear ()

let displayRules () =
    Console.Clear ()
    printfn "
        Morabaraba is a two player game.
        The typical morabaraba board has 24 squares and each player starts with 12 stones (called cows).
        The first phase of the game involves each player placing one of their cows onto an empty square on the board.
        Placing three of your cows in a straight connected line will result in a mill 
        and allow you to remove or \"shoot\" one of your oppponent's cows
        Cows that are in a mill cannot be shot or removed.
        If you create a mill and all your opponent's cows are in a mill then a mill is created, but no cow is shot.
        Once both players have placed their twelve stones, the moving phase begins.
        
        In the moving phase players can move their cows to an adjacent empty square.
        Mills can still be created this way and the rules for mills and shooting still apply.
        You cannot move out of and create the same mill with the same piece in consecutive turns,
        if a player does this, the cow returns to the mill, but they cannot shoot an opponent's cow.
        
        When a player reaches three cows on the board, all their cows become flying cows.
        A flying cow can move to any empty square on the board rather than just adjacent squares.
        
        End conditions:
        A player loses when they reach 2 stones in the moving phase.
        A player also loses when they cannot make a move in the moving phase.
        If 10 moves are resolved while both players are on 3 cows then the game ends in a draw.
        If both players place all their cows on the board without forming a mill then 
        the game ends in a draw as neither player can move."
    printf "Press any key to continue"
    Console.ReadKey () |> ignore
    Console.Clear()

let displayMenu (menuItems : string list) =
    let rec menu state =
        Console.Clear ()
        let mapSelection i item =
            match i = state with
            | false -> item
            | true ->
                ">" + item
        List.iteri (fun index item -> 
                        match index = state with
                        | false -> printfn "%s" item
                        | true -> cprintfn ConsoleColor.DarkGray item) (List.mapi mapSelection menuItems)

        let inputKey = (Console.ReadKey ()).Key
        match inputKey with
        | ConsoleKey.UpArrow -> 
            match state = 0 with
            | false -> menu (state-1)
            | true -> menu (menuItems.Length - 1)
        | ConsoleKey.DownArrow -> menu ((state+1) % menuItems.Length)
        | ConsoleKey.Escape -> -1
        | ConsoleKey.Enter -> state
        | _ -> menu state
    menu 0

let rec mainMenu () =
    Console.Clear()
    match displayMenu ["Start game"; "Rules of Morabaraba"; "Credits"; "Exit"] with
    | 0 ->
        MorabarabaGame.runGame () |> ignore
        mainMenu ()
    | 1 ->
        displayRules ()
        mainMenu ()
    | 2 ->
        displayCredits ()
        mainMenu ()
    | 3 | -1 ->
        printfn "Good bye"
    | _ -> failwith "Error"

[<EntryPoint>]
let main argv =
    mainMenu ()
    Console.ReadLine() |> ignore
    0 // return an integer exit code

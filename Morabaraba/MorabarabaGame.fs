module public MorabarabaGame

open System
open System.Runtime.ExceptionServices
//TYPE DEFINITIONS------------------------------------------------------------------------------------------------------------------
type Player =
|X
|O

type Cow =
|Normal of Player
|Flying of Player
|Empty

type GameState =
|PlacingPhase
|MovingPhase
|Drawn
|Won of Player


type Cell = 
    {
        pos : char*int
        state : Cow
    }

type Board = {
                boardState : Cell list
                stones : int*int
             }

//CONSTANTS-------------------------------------------------------------------------------------------------------------------------
let initialBoard = {    boardState = [
                                      {pos='A',1; state = Empty}; {pos='A',4; state = Empty}; {pos='A',7; state = Empty};
                                      {pos='B',2; state = Empty}; {pos='B',4; state = Empty}; {pos='B',6; state = Empty};
                                      {pos='C',3; state = Empty}; {pos='C',4; state = Empty}; {pos='C',5; state = Empty};
                                      {pos='D',1; state = Empty}; {pos='D',2; state = Empty}; {pos='D',3; state = Empty}; {pos='D',5; state = Empty}; {pos='D',6; state = Empty}; {pos='D',7; state = Empty};
                                      {pos='E',3; state = Empty}; {pos='E',4; state = Empty}; {pos='E',5; state = Empty};
                                      {pos='F',2; state = Empty}; {pos='F',4; state = Empty}; {pos='F',6; state = Empty};
                                      {pos='G',1; state = Empty}; {pos='G',4; state = Empty}; {pos='G',7; state = Empty};
                                     ];
                        stones = (12, 12)
                    }

let mills = [
             (('A', 1), ('A', 4), ('A', 7));
             (('B', 2), ('B', 4), ('B', 6));
             (('C', 3), ('C', 4), ('C', 5));
             (('D', 1), ('D', 2), ('D', 3));
             (('D', 5), ('D', 6), ('D', 7));
             (('E', 3), ('E', 4), ('E', 5));
             (('F', 2), ('F', 4), ('F', 6));
             (('G', 1), ('G', 4), ('G', 7));
             (('A', 1), ('D', 1), ('G', 1));
             (('B', 2), ('D', 2), ('F', 2));
             (('C', 3), ('D', 3), ('E', 3));
             (('A', 4), ('B', 4), ('C', 4));
             (('E', 4), ('F', 4), ('G', 4));
             (('C', 5), ('D', 5), ('E', 5));
             (('B', 6), ('D', 6), ('F', 6));
             (('A', 7), ('D', 7), ('G', 7));                                                 
             (('A', 1), ('B', 2), ('C', 3));
             (('A', 7), ('B', 6), ('C', 5));
             (('G', 1), ('F', 2), ('E', 3));
             (('G', 7), ('F', 6), ('E', 5));
            ]


//DISPLAY FUNCTIONS-----------------------------------------------------------------------------------------------------------------
/// gets string representation of player
let playerToString = function
    | X -> "X"
    | O -> "O"

//-----------------------------------------------------------------
/// converts the board into a string for consistent iteraction between
/// backend and UI
let boardToString (board : Cell list) =
    let mapCellStateToString item =
        match item.state with
        | Normal O | Flying O -> "O"
        | Normal X | Flying X -> "X"
        | Empty -> " "

    String.Concat (List.map mapCellStateToString board)

//-----------------------------------------------------------------
/// takes string representation of the board and displays it
let displayBoard (boardString : string) =
    printfn 
        "    1   2  3   4   5  6   7      \n\
         A   %c----------%c----------%c   \n\
         |   | '.       |        .'|      \n\
         B   |   %c------%c------%c   |   \n\
         |   |   |'.    |    .'|   |      \n\
         C   |   |  %c---%c---%c  |   |   \n\
         |   |   |  |       |  |   |      \n\
         D   %c---%c--%c       %c--%c---%c\n\
         |   |   |  |       |  |   |      \n\
         E   |   |  %c---%c---%c  |   |   \n\
         |   |   |.'    |    '.|   |      \n\
         F   |   %c------%c------%c   |   \n\
         |   |.'        |       '. |      \n\
         G   %c----------%c----------%c   \n
         "
         boardString.[0] boardString.[1] boardString.[2]
         boardString.[3] boardString.[4] boardString.[5]
         boardString.[6] boardString.[7] boardString.[8]
         boardString.[9] boardString.[10] boardString.[11] boardString.[12] boardString.[13] boardString.[14]
         boardString.[15] boardString.[16] boardString.[17]
         boardString.[18] boardString.[19] boardString.[20]
         boardString.[21] boardString.[22] boardString.[23]

//-----------------------------------------------------------------
///displays the stones for each player
let displayScore (x, o) =
    //generate display string from number of stones
    printfn 
        "Player X: %s\n\
         Player O: %s 
        " 
        (String.concat "" (Seq.init x (fun _ -> "x")))
        (String.concat "" (Seq.init o (fun _ -> "o")))

//-----------------------------------------------------------------    
/// clears the console and displays the board and player stones
let refreshBoard board () =
    Console.Clear()
    displayScore board.stones
    (boardToString >> displayBoard) board.boardState

//-----------------------------------------------------------------
/// displays an error message with an optional prompt before continuing with
/// execution
let genericErrorMsg msg prompt =
    printfn "%s" msg
    match prompt with
    | true -> 
        Console.Read () |> ignore
        ()
    | false -> ()
     
//DISPLAY FUNCTIONS-----------------------------------------------------------------------------------------------------------------

//HELPER FUNCTIONS------------------------------------------------------------------------------------------------------------------

/// gets the player that is not player
let getOpponent = function
    | X -> O
    | O -> X

//-----------------------------------------------------------------
/// returns the Cells that belong to player on board
let getPlayerSquares board player =
    List.filter (fun item -> item.state = Normal player || item.state = Flying player ) board

//-----------------------------------------------------------------
/// returns the number of squares player has on board
let getNumPlayerSquares board player =
    (getPlayerSquares board player).Length

/// returns all empty squares present in board
let getEmptySquares board =
    let filterSquares item =
        item.state = Empty
    List.filter filterSquares board

//-----------------------------------------------------------------
/// Takes a position and returns whether or not it matches the valid positions
/// for the standard Morabaraba board
let isValidPos = function
    | ('A', 1) | ('A', 4) | ('A', 7)
    | ('B', 2) | ('B', 4) | ('B', 6)
    | ('C', 3) | ('C', 4) | ('C', 5)
    | ('D', 1) | ('D', 2) | ('D', 3) | ('D', 5) | ('D', 6) | ('D', 7)
    | ('E', 3) | ('E', 4) | ('E', 5)
    | ('F', 2) | ('F', 4) | ('F', 6)
    | ('G', 1) | ('G', 4) | ('G', 7) -> true
    | _ -> false

//-----------------------------------------------------------------    
/// <summary>
/// Gets a position of type char*int from the user.
/// The function is recursively called until valid input is given
/// </summary>
/// <param name="msg"> Input prompt to display </param>
let rec getPosFromUser msg refresh =
    printf "%s" msg 
    let input = Console.ReadLine ()
    match input.Length =2 with
    | false ->
        refresh ()
        printfn "Invalid input, please enter a valid position"
        getPosFromUser msg refresh
    |true ->
        let pos = Char.ToUpper input.[0], (int input.[1]) - 48
        match isValidPos pos with
        | true -> pos
        | false ->
            refresh ()
            printfn "Invalid input, please enter a valid position"
            getPosFromUser msg refresh

//-----------------------------------------------------------------
/// Takes position pos and returns the Cell with pos=pos
/// in board
let getCellFromPos (board : Cell list) pos =
    List.find (fun item -> item.pos = pos) board

/// Takes position pos and returns the state of Cell with pos=pos
/// in board
let getStateFromPos (board : Cell list) pos =
    (List.find (fun item -> item.pos = pos) board).state

//-----------------------------------------------------------------
/// <summary>
/// Updates Cell with pos=pos to have state = newState
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="pos"> The position to update </param>
/// <param name="newState"> New state to update to </param>
let updateSquare (board : Board) pos newState =
    let mapUpdateElement item =
        match item.pos = pos with
        |false -> item
        |true -> {item with state = newState}
    let newState = List.map mapUpdateElement board.boardState
    {board with boardState = newState}

//-----------------------------------------------------------------
/// <summary>
/// Checks if the Cell with pos=playPos is in a mill
/// </summary>
/// <param name="board"> The current board state </param>
/// <param name="playPos"> The chosen position to check </param>
let isInMill (board : Cell list) (playPos : char*int) =
    //get mills that only include position given
    let getRelevantMillStates (a, b, c) =
        match a = playPos || b = playPos || c = playPos with
        | true -> Some (getStateFromPos board a, getStateFromPos board b, getStateFromPos board c)
        | false -> None 
    
    let relevantMillStates = List.choose getRelevantMillStates mills
    //once converted, get only those mills that are full
    let filterFullMills item =
        match item with
        | Normal X, Normal X, Normal X
        | Normal O, Normal O, Normal O
        | Flying X, Flying X, Flying X
        | Flying O, Flying O, Flying O -> true
        | _ -> false
    List.exists filterFullMills relevantMillStates

//-----------------------------------------------------------------
/// <summary>
/// After player has completed a mill, this checks that an opponent's cow can be shot
/// and if so replaces the chosen Cow with Empty
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="player"> The curren player </param>
let rec shootCow (board : Board) player = 
    let opponentSquares = getPlayerSquares board.boardState (getOpponent player)
    let notInMill item =
        not (isInMill board.boardState item.pos)
    let opponentSquaresNotInMills = List.filter notInMill opponentSquares
    
    match opponentSquaresNotInMills.Length = 0 with
    | true -> //All opponent's cows are in mills so no cow is shot and the game continues
        refreshBoard board ()
        board
    |false ->
        let inputPos = getPosFromUser "shoot which cow? " (refreshBoard board)
        match List.exists (fun item -> item.pos = inputPos) opponentSquaresNotInMills with
        | true -> updateSquare board inputPos Empty
        | false -> 
            refreshBoard board ()
            genericErrorMsg "You have to shoot an opponent's square that is not in a mill, select another one" false
            shootCow board player
//HELPER FUNCTIONS------------------------------------------------------------------------------------------------------------------

//PLACING PHASE FUNCTIONS-----------------------------------------------------------------------------------------------------------

/// <summary>
/// Places one of the player's stones onto the board as a Cow of Normal Player
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="player"> The current player </param>
let rec placePiece board player =
    let playPos = getPosFromUser (playerToString player + "'s turn: ") (refreshBoard board)
    let pieceState = getStateFromPos board.boardState playPos 
    match pieceState with //checks that chosen square is empty
    | Normal _ | Flying _ -> 
        refreshBoard board ()
        printfn "That square is already occupied, choose another square"
        placePiece board player
    | Empty ->
        let board = updateSquare board playPos (Normal player)
        let (XStones, OStones) = board.stones
        match player with
        | X ->
            ({board with stones = (XStones - 1, OStones)}, playPos)
        | O ->
            ({board with stones = (XStones, OStones - 1)}, playPos)

//-----------------------------------------------------------------
/// <summary>
/// Executes the entire placing phase for Morabaraba
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="player"> The current player </param>
let rec runPlacingPhase (board : Board) player  =
    refreshBoard board ()
    let board, pos = placePiece board player
    let board = 
        match isInMill board.boardState pos with
        | true ->
            refreshBoard board ()
            shootCow board player
        | _ ->
            board
    match board.stones with
    | (0, 0) -> //Stop placing when both players run out of stones
        refreshBoard board ()
        match getNumPlayerSquares board.boardState X = 12 && getNumPlayerSquares board.boardState O = 12 with //if both players still have all 12 stones on board then no cows have been shot
        | true -> Drawn, board                                                                                //and the board is full, meaning that no one can move and the game is drawn
        | false -> MovingPhase, board
    | _ -> 
        runPlacingPhase board (getOpponent player)
//PLACING PHASE FUNCTIONS-----------------------------------------------------------------------------------------------------------


//MOVING PHASE FUNCTIONS------------------------------------------------------------------------------------------------------------
///Takes a position and returns the positions of the adjacent squares
let getAdjacentSquares = function
    | ('A', 1) -> [('D', 1); ('A', 4); ('B', 2)]
    | ('A', 4) -> [('A', 1); ('B', 4); ('A', 7)]
    | ('A', 7) -> [('A', 4); ('B', 6); ('D', 7)]

    | ('B', 2) -> [('A', 1); ('D', 2); ('C', 3); ('B', 4)]
    | ('B', 4) -> [('B', 2); ('A', 4); ('C', 4); ('B', 6)]
    | ('B', 6) -> [('B', 4); ('C', 5); ('D', 6); ('A', 7)]
    
    | ('C', 3) -> [('B', 2); ('C', 4); ('D', 3)]
    | ('C', 4) -> [('C', 3); ('B', 4); ('C', 5)]
    | ('C', 5) -> [('C', 4); ('D', 5); ('B', 6)]

    | ('D', 1) -> [('A', 1); ('G', 1); ('D', 2)]
    | ('D', 2) -> [('D', 1); ('F', 2); ('D', 3); ('B', 2)]
    | ('D', 3) -> [('D', 2); ('E', 3); ('C', 3)]
    
    | ('D', 5) -> [('E', 5); ('D', 6); ('C', 5)]
    | ('D', 6) -> [('D', 5); ('F', 6); ('D', 7); ('B', 6)]
    | ('D', 7) -> [('D', 6); ('G', 7); ('A', 7)]
    
    | ('E', 3) -> [('F', 2); ('E', 4); ('D', 3)]
    | ('E', 4) -> [('E', 3); ('F', 4); ('E', 5)]
    | ('E', 5) -> [('E', 4); ('F', 6); ('D', 5)]
    
    | ('F', 2) -> [('G', 1); ('F', 4); ('E', 3); ('D', 2)]
    | ('F', 4) -> [('F', 2); ('G', 4); ('F', 6); ('E', 4)]
    | ('F', 6) -> [('F', 4); ('G', 7); ('D', 6); ('E', 5)]
    
    | ('G', 1) -> [('D', 1); ('G', 4); ('F', 2)]
    | ('G', 4) -> [('G', 1); ('F', 4); ('G', 7)]
    | ('G', 7) -> [('G', 4); ('F', 6); ('D', 7)]

    |_ -> failwith "No such position"
/// <summary>
/// Gets all cell's neighbour states
/// </summary>
/// <param name="board"> The current board state </param>
/// <param name="pos"> The position of the cell being checked </param>
let getNeighbourCells board pos = 
    List.map (fun item -> getCellFromPos board item) (getAdjacentSquares pos)
/// <summary>
/// Moves the chosen Flying cow to an Empty square
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="player"> The current player </param>
/// <param name="takePos"> The position from which the piece was taken </param>
let rec doFlyingMove board player ((l, n) as takePos) =    
    let playPos = getPosFromUser (playerToString player + (sprintf " place %c%d at: " l n)) (refreshBoard board)

    match List.exists (fun item -> item.pos = playPos) (getEmptySquares board.boardState) with //Check that chosen square is Empty
    | true ->
        refreshBoard board ()
        printfn "You must place your piece on an empty square"
        doFlyingMove board player takePos
    | false ->
        let board = updateSquare board playPos (Flying player)
        let board = updateSquare board takePos Empty
        board, playPos, takePos

//-----------------------------------------------------------------
/// <summary>
/// Moves the chosen Normal cow to an adjacent Empty square (neighbour)
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="neighbours"> A Cell list of adjacent squares </param>
/// <param name="player"> The current player </param>
/// <param name="takePos"> The position from which the piece was taken </param>
let rec doNormalMove board neighbours player ((l, n) as takePos) =
    let playPos = getPosFromUser (playerToString player + (sprintf " place %c%d at: " l n)) (refreshBoard board)
    match List.exists (fun item -> item.pos = playPos) neighbours with //check that chosen square is a neighbour
    | false ->
        refreshBoard board ()
        printfn "You must move to one of the piece's neighbours"
        doNormalMove board neighbours player takePos
    | true ->
        let board = updateSquare board playPos (Normal player)
        let board = updateSquare board takePos Empty
        board, playPos, takePos
        
//----------------------------------------------------------------- 
/// <summary>
/// Selects a piece to move, gets recursively called until valid input is given
/// then calls either doNormalMove or doFlyingMove based on the piece chosen
/// </summary>
/// <param name="board"> The current board </param>
/// <param name="player"> The current player </param>
let rec movePiece board player =
    let playPos = getPosFromUser (playerToString player + " select piece to move: ") (refreshBoard board)

    let pieceState = getStateFromPos board.boardState playPos
    
    match pieceState = Normal player || pieceState = Flying player with //check that player chose one of their pieces
    | false ->
        refreshBoard board ()
        printfn "You must select one of your pieces"
        movePiece board player
    | true ->
        match pieceState = Flying player with //if Flying piece can move anywhere
        | true ->
            doFlyingMove board player playPos
        | false -> //if not Flying piece is Normal and so a check must be made that piece has at least one Empty neighbour

            let neighbours = getNeighbourCells board.boardState playPos

            let emptyNeighbours = List.filter (fun item -> item.state = Empty) neighbours
            
            match emptyNeighbours with
            | [] -> 
                refreshBoard board ()
                printfn "This piece has no empty neighbours, choose another one"
                movePiece board player
            | _ ->
                doNormalMove board emptyNeighbours player playPos             

//-----------------------------------------------------------------
/// <summary>
/// Checks whether the provided player can make a move
/// </summary>
/// <param name="board">The current board state</param>
/// <param name="player">The player to check</param>
let canPlay board player =
    match getNumPlayerSquares board player <= 3 with //if player has 3 or fewer cows then it is guaranteed that they can play
    | true -> true
    | false ->

        let playerSquares = getPlayerSquares board player
        
        let cellHasEmptyNeighbour (item : Cell) =
           let neighbours = getNeighbourCells board item.pos 
           List.exists (fun state -> state.state = Empty) neighbours

        List.exists cellHasEmptyNeighbour playerSquares

//-----------------------------------------------------------------
/// <summary>
/// Changes all the pieces of the provided player form Normal of Player to Flying of Player, 
/// this happens when the player reaches 3 Cows on board
/// </summary>
/// <param name="board"> The current board</param>
/// <param name="player"> The player whose cows must be changed</param>
let changeAlltoFlying board player =
    let playerSquares = getPlayerSquares board.boardState player

    let rec updateBoard rest outBoard = 
        match rest with
        | [] -> outBoard
        | first::rest ->
            let newBoard = updateSquare outBoard first.pos (Flying player)
            updateBoard rest newBoard
    updateBoard playerSquares board

//-----------------------------------------------------------------
/// <summary>Executes the entire moving phase for Morabaraba </summary>
/// <param name="board"> Board record holding the boardState and player stones </param>
/// <param name="player"> The current player </param>
/// <param name="lastXPlay"> Last full move for player X (piece selected and where it was moved to) </param>
/// <param name="lastOPlay"> Last full move for player O (piece selected and where it was moved to) </param>
/// <param name="endCounter"> Draw counter that is incremented when both players have 3 stones, game is drawn when counter reaches 10 </param>
/// <returns> GameState once all play has been concluded </returns>
let rec runMovingPhase (board : Board) player lastXPlay lastOPlay endCounter =

    let board, playPos, lastPos = movePiece board player //get new board state  
    //lastPos is old position of the piece that was just moved
    //playPOs is the position the piece has just been moved to
    
    //check that a new valid mill has been created 
    //and then shoot and return new board state if one has
    let board = 
        match isInMill board.boardState playPos with
        | true ->
            match player with
            | X ->
                match (lastPos, playPos) = lastXPlay with //checks that player is not recreating a mill in the same way they broke it the previous turn
                | true -> board
                | false ->
                    refreshBoard board ()
                    shootCow board player    
            | O ->
                match (lastPos, playPos) = lastOPlay with //checks that player is not recreating a mill in the same way they broke it the previous turn
                | true -> board
                | false ->
                    refreshBoard board ()
                    shootCow board player
        | _ ->
            board

    let numX, numO = getNumPlayerSquares board.boardState X, getNumPlayerSquares board.boardState O

    //change pieces to flying once a player reaches 3 cows
    //otherwise keep the same
    let board = 
        match numX = 3 with
        | true ->
            changeAlltoFlying board X
        | false ->
            board
    let board = 
        match numO = 3 with
        | true ->
            changeAlltoFlying board O
        | false ->
            board
    
    //end conditions
    refreshBoard board ()
    match endCounter = 10 with //10 moves have been played with both players on 3 Flying Cows
    | true -> Drawn
    | false -> 
        match numX < 3 ||  not (canPlay board.boardState X) with // X has fewer than 3 cows or cannot play
        | true ->
            Won O
        | false ->
            match numO < 3 || not (canPlay board.boardState O) with // O has fewer than 3 cows or cannot play
            | true ->
                Won X
            | false ->
                let endCounter = 
                    match numO = 3 && numX = 3 with //increment counter if both players are on 3 cows
                    | true -> endCounter + 1
                    | false -> -1
                match player with
                | X ->
                    runMovingPhase board O (playPos, lastPos) lastOPlay endCounter
                | O ->
                    runMovingPhase board X lastXPlay (playPos, lastPos) endCounter

//MOVING PHASE FUNCTIONS------------------------------------------------------------------------------------------------------------

//MAIN GAME LOOP--------------------------------------------------------------------------------------------------------------------
/// Runs main game loop and displays a message based on final game state
let rec runGame () =
    let board = initialBoard

    match runPlacingPhase board X with //Game always starts with X
    |Drawn, _ ->
        printfn "Game drawn with no mills created"
        Console.ReadLine () |> ignore
    |MovingPhase, board ->
        match canPlay board.boardState X with
        | false -> 
            printfn "O has won!"
            Console.ReadLine () |> ignore
        | true ->
            match canPlay board.boardState O with
                | false -> 
                    printfn "X has won!"
                    Console.ReadLine () |> ignore
                | true ->
                    refreshBoard board ()
                    match runMovingPhase board X (('Z', 8), ('Z', 8)) (('Z', 8), ('Z', 8)) -1 with //phase starts with X and impossible states are given for the last play of each player
                    | Drawn ->                                                                     //endCounter starts at -1 because it needs to be 0 as both players reach 3 pieces each
                        printfn "Game has been drawn"
                        Console.ReadLine () |> ignore
                    | Won X ->
                        printfn "X has won!"
                        Console.ReadLine () |> ignore
                    | Won O ->
                        printfn "O has won!"
                        Console.ReadLine () |> ignore
                    | _ -> failwith "Lol, this was not supposed to happen"
    | _ -> failwith "Something went terribly wrong"
    //final message and possible replay
    Console.Clear ()
    printf "Would you like to play again [y/n]? "
    match Console.ReadLine () with
    | "y" ->
        runGame ()
    | _ ->
        printfn "Thank you for playing!"
//MAIN GAME LOOP--------------------------------------------------------------------------------------------------------------------
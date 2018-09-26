# How-to

## Create a game

```cs
var game = new ReversiGame(boardWidth, boardHeight);
```

`boardWidth` and `boardHeight` must be between `2` and `20` and even.

__Notes__:

* Do **not** hardcode these constraints in the VM or V. Instead, rely on `ReversiBoard.IsValidWidth` and `ReversiBoard.IsValidHeight` to determine `whether given dimensions are valid.
* Do **not** rely on exceptions thrown by model classes to determine whether the parameters have valid values.

## Determine the current player

```cs
if ( game.CurrentPlayer == Player.BLACK )
{

}

if ( game.CurrentPlayer == Player.WHITE )
{

}
```

## Checking if a move is valid

```cs
var position = new Vector2D(2, 3);

if ( game.IsValidMove(position) )
{

}
```

## Putting a stone

```cs
var position = new Vector2D(4, 2);
var updatedGame = game.PutStone(position);
```

## Finding out who owns a specific cell

```cs
var position = new Vector2D(6, 2);
var owner = game.Board[position];
```

## Finding out which stones will be captured by a move

```cs
var position = new Vector2D(5, 3);
var captured = game.CapturedBy(position);
```

## Determining the number of stones of each player

```cs
var blackStones = game.Board.CountStones(Player.BLACK);
var whiteStones = game.Board.CountStones(Player.WHITE);
```

## Validating width and height

```cs
var width = 4;
var height = 10;

if ( ReversiBoard.IsValidWidth(width) && ReversiBoard.IsValidHeight(height) )
{
    ...
}
```
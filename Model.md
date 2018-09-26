# Classes

## `Player`

`Player` object represent players. There are only two `Player` objects that can exist, and they have been created for you in advance:

* `Player.BLACK`
* `Player.WHITE`

In order to determine which player you received, you can simply use `p == Player.BLACK` or `p == Player.WHITE`. In other words, this is a situation where you can safely use `==` on objects.

The method `OtherPlayer` can be used to switch players: `Player.BLACK.OtherPlayer` returns `Player.WHITE` and vice versa.

__Notes__:

* Do **not** rely on `Player.ToString()`: `ToString()` is a debugging tool and shouldn't be used for more than that.

## `ReversiBoard`

A `ReversiBoard` object represents a game board, i.e., it is a grid with the following characteristics:

* A `ReversiBoard` has a `Width` and `Height`.
* Each cell in the grid contains one of three possible values:
  * `Player.BLACK`
  * `Player.WHITE`
  * `null`
* To determine what a given cell contains, use `board[position]` where `position` is a `Vector2D`.
* Use `ReversiBoard.IsValidWidth` and `ReversiBoard.IsValidHeight` to determine whether given dimensions are valid.
* `board.CountStones(Player.BLACK)` counts the number of positions with black stones on them.

## `ReversiGame`

This class contains most of the functionality you'll need. An important detail is that `ReversiGame` objects
are *immutable*: once created, they never change. Methods that you would expect to modify the object return
a *new* object representing the new state.

A `ReversiGame` object has the following properties:

* `Board` of type `ReversiBoard`: describes the state of the board.
* `CurrentPlayer` of type `Player`: whose turn it is.
* `IsGameOver` of type `bool`.

The following methods are available:

* `game.IsValidMove(position)` returns `true` if the current player is allowed to place a stone at the given `position`.
* `game.PutStone(position)` adds a stone of the current player at the given position. `PutStone` does **not** change `game`, but instead returns a *new* `ReversiGame` object that represents the new state.
* `game.CapturedBy(position)` returns the positions of all stones that would be captured if the current player were to place a stone at the given `position`.

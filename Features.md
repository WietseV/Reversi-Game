# Minimal Features

## Functionality

* Functional Reversi game (no crashes, playable, etc.)
* White and black stone counts visible
* Visual markers for valid moves
* Visual indication of whose turn it is
* Correct handling of end-of-game
* Choosing board size

## Technical Requirements

* MVVM
* Multiple screens (!= multiple windows). See samples for how to do this.
* At least one *reusable* User Control (with at least one self defined dependency properties). It should not have anything Reversi-specific hardcoded but instead be useable in other projects without changes.
* At least three extra features from list below



# Possible Extra Features

* Skins
* Music/Sounds
* Player names
* Let players pick their color (i.e. color picker window)
* User controls
  * Timer
  * Animated stone counters
  * Score bar (visual representation of scores)
  * Separate control for displaying player stones
  * Validating text box (becomes red, shows error tooltip, &hellip;)
* Functionality
  * Show stones that will be captured by certain move
  * Move history
  * Undo moves
  * Save/load games
  * AI (classes are included in domain)
  * Multiplayer over network

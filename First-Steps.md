# First Steps

This text offers a starting point for students who do not know how to begin. You are free, however, to completely ignore this guide.

## Drawing the Board

The first step is the hardest: visualizing the reversi board. There are multiple approaches; we'll discuss the one shown on in the `ItemsControl2D` sample at [WPF samples](https://github.com/fvogels/samples.git). It shows you how to represent a grid using nested `ItemsControl`s. If words like `ItemsSource`, `ItemsPanel` and `ItemTemplate` do not ring a bell, we suggest you revisit the tutorial section discussing `ItemsControl` and experiment a bit with it.

An `ItemsControl` can be used to visualize lists. You also know that a grid can be represented as a list of lists. So, it would make sense to visualize a grid as an `ItemsControl` of `ItemsControl`s:

* The "outer" `ItemsControl` receives the entire grid as its `DataContext` and binds its `ItemsSource` to the `Rows` of that grid.
* The "inner" `ItemsControl` receives a row as `DataContext` and uses its elements as its `ItemsSource`.

The grid to be drawn is represented as a `ReversiBoard`. This model class is not very view-friendly and requires us to write some adapter classes (i.e., view model classes) so that we can bind our controls easily in the XAML.
We suggest you make three "adapter classes":

* One `BoardViewModel` that represents the entire board.
* One `BoardRowViewModel` that represents a specific row.
* One `BoardSquareViewModel` that represents a single square of the board.

> ## Task
>
> Let's ignore the `ReversiBoard` at first.
>
> 1. Create a `BoardSquareViewModel` class. You can leave it empty for now.
> 2. Create a `BoardRowViewModel` class. Let it have a property `Squares` that returns 8 `BoardSquareViewModel` objects in a list.
> 3. Create a `BoardViewModel` with a `Rows` property that returns 8 `BoardRowViewModel`s.

Now that we have view model classes, we can use them to build our GUI.
Let's start with the outer `ItemsControl`:

```diff
    <ItemsControl ItemsSource="{Binding Rows}">
        <ItemsControl.ItemTemplate>
            <DataTemplate>
                <!-- TODO: Add control that draws one row -->
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
```

Each row will be represented by an inner `ItemsControl`.
Each of these would receive a `BoardRowViewModel` as `DataContext`.

```diff
    <ItemsControl ItemsSource="{Binding Rows}">
        <ItemsControl.ItemTemplate>
            <DataTemplate>
+               <ItemsControl ItemsSource="{Binding Squares}">
+                   <ItemsControl.ItemTemplate>
+                       <DataTemplate>
                            <!-- TODO: Add control that draws one square -->
+                       </DataTemplate>
+                   </ItemsControl.ItemTemplate>
+               </ItemsControl>
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
```

For now, have each square be represented by a simple `Border`. Don't forget to set its `Width`, `Height`, `BorderBrush` and `BorderThickness` to make it visible.

> ## Task
>
> 1. Incorporate the above XAML code into your project.
> 2. Define the inner `ItemsControl`'s `ItemTemplate` so that each board square is shown as a literal square.
> 3. Give the outer `ItemsControl` a `BoardViewModel` as `DataContext`.

If you made it as described above, you probably get a series of 64 vertically aligned squares when you run it. This is because each `ItemsControl` arranges its children vertically by default.

> ## Task
>
> Set the `ItemsPanel` of the appropriate `ItemsControl` so that it arranges its children horizontally instead of vertically.

## Dehardcoding the Board's Dimensions

Currently, we do not use a `ReversiBoard` anywhere; the `BoardViewModel` is hardcoded as a 8&times;8 grid. Let's instead have the `BoardViewModel` use the size of a given `ReversiBoard` object.

> ## Task
>
> 1. Have `BoardViewModel`'s constructor take a `ReversiBoard` and store it in a field.
> 2. Have `BoardViewModel` rely on the `ReversiBoard`'s height to determine how many `BoardRowViewModel`s it has to make.
> 3. Find some way to have `BoardRowViewModel` create the right amount (= `ReversiBoard.Width`) of `BoardSquareViewModel`.
> 4. In `MainWindow`'s constructor, create a `ReversiGame` and use its `Board` property to initialize the `BoardViewModel`.
>
> Check your work by creating `ReversiGame`s with different board sizes and verifying that the View shows boards of corresponding size.

## Drawing Stones

We now want to draw the players' stones in the board squares. Let's start with something simple.

> ## Task
>
> Add an `Ellipse` inside the `Border`.

Next, we assign it the correct color.

> ## Task
>
> 1. Let `BoardSquareViewModel` expose information about who's the square's owner (white, black or empty).
> 2. Define an `IValueConverter` that converts an owner to a color. You can associate `Transparent` with "no owner."
> 3. Let the `Ellipse` have the correct color.
>
> To verify if your code works correctly, run your application and check that the board does indeed show the initial board state:
>
> ```
> ........
> ........
> ........
> ...WB...
> ...BW...
> ........
> ........
> ........
> ```

## Putting Stones

Let's make the board interactive by having it react to clicks. As always, let's keep it simple at first and let a click on a square cause it to contain a black stone.

Currently, our square is a `Border`, but it's a better idea to turn it into a `Button` as this control has built-in support for "being clicked".

> ## Task
>
> Change the `Border` into a `Button`.

The clean way to deal with button clicks is to rely on the `Button`'s `Command` property.

> ## Task
>
> We need to add a `PutStone` member of type `ICommand` to `BoardSquareViewModel` which will process user clicks.
>
> * Create a class `PutStoneCommand` that implements `ICommand`.
> * A `PutStoneCommand` object must know with which square it is associated. This means a `PutStoneCommand` object needs a link back to its corresponding `BoardSquareViewModel`.
> * Let's pretend for now that it is always executable (later we'll change this to only allow valid moves).
> * The `Command`'s `Execute` method should set the corresponding `BoardSquareViewModel`'s owner to black.

Check if it works. If clicking on a square appears to do nothing, ask yourself if WPF knows about the change. In the tutorial, we showed multiple ways to make properties "observable" by WPF.

Once this works, i.e., you can turn the whole board black by clicking on every square, we can have it behave according to the actual reversi rules.

> ## Task
>
> Have the GUI follow the reversi rules.
>
> * Use `ReversiGame`'s `PutStone` method to add a stone. You'll have to adapt your view model classes a bit as you want `PutStoneCommand` to be able to somehow relay a message to `ReversiGame`.
> * Warning: `PutStone` returns a *new* `ReversiGame` object! While this may seem counterintuitive, it actually simplifies things a bit.
> * Find a way to "refresh" every `BoardSquareViewModel` after a call to `PutStone`. This is relatively easy to implement, as long as you structure your code correctly.
> * Update the `PutStoneCommand`'s `CanExecute` method so that it only returns `true` when the move it represents is valid. The model provides a method implementing this functionality.

If you did everything correctly, you know have a playable reversi game.

The training wheels come off now. You're on your own to implement the other features. Good luck.
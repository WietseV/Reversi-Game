# General Guidelines

* Don't forget to declare classes `public` if you need to refer to them from another assembly. By default (i.e., if you omit an access modifier), a class is `internal` and won't be visible outside the project it resides in.


# Frequent Mistakes

* M/V/VM separation: put all classes in the correct layer. All functionality-related code should reside in the M-layer. All GUI-specific features (visible strings, colors, sound, ...) should reside in the V-layer. The VM-layer should be restricted to organizing the data from the M-layer so that it can be readily consumed by the V-layer.* Do not rely on `Player.ToString()`. `ToString()` methods are generally meant for debugging only and are a quick and dirty way to get readable information about objects.
* Don't show strings from the model or the view model. These should only expose "conceptual" information, which must be translated to a string by the V-layer. For example, expose the current player as a `Player`, not as a `string`.
* Don't forget that helper classes can dramatically simplify your code. Most students tend to write only classes they have a direct need for, instead of distilling common code into separate classes.
* Don't let converters return `string`s it that's not conceptually what they do. For example, if your converter is supposed to convert a value to a brush, return a `Brushes.Black`, not `"black"`.





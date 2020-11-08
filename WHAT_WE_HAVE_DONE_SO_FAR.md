At the moment, what we have done is lots of things from the evaluator,
we took some ready to use Lua parser and we have created a AST interpreter
which interprets the code. It's current features are:

- Variable Declaration
- Arrays. For example : {1,2,3}
- Array slices (for example you can get the 3 elements of an array like
array[0..3], even though lua doesn't support that, we thought that 
this was a cool feature to have.
- Strings and string/array concat.
- Automatic String to Number when it detects you are operating between the ttwo
- If statements
- All supported unary operators of Lua
- All of the number operations
- Standard library objects and injection of interfaces for the user.
- Array accessing like Python's
- For loops
- Recursivity
- Function declaration with multiple return values and destructuring
- Parameters passing.
- And more! We invite you to check the code inside eval, which contains
 lots of things, in parser_testing we got the component where 
 you can write some code and see examples of what you can do.


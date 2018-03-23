# unity-debugging-visualization-example
This is an example for how you can write visual debugging tools for Unity instead of using Debug.Log or breakpoints for your scripts.

## Example

In this example, there is a hypothetical AI agent with a very simple [FSM](https://en.wikipedia.org/wiki/Finite-state_machine) in which it chooses which task to do next. Every task has private members with interesting data, however, these tasks are not MonoBehaviours. You could just make the classes serializable, but viewing these in Unity's inspector can be quite confusing, and you need to enable Debug mode or make your fields publicly accessable somehow.

Instead, you can implement the SerializeSubobjects interface and give the classes you want to serialize. In the example, the editor marks the "current" state in green.

![alt text](https://github.com/mathiassiig/unity-debugging-visualization-example/blob/master/example.png "")

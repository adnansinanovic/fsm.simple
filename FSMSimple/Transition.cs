using System;

namespace FSMSimple
{
    public class Transition
    {
        public int Event { get; internal set; }
        public int StateFrom { get; internal set; }
        public Func<bool> Condition { get; internal set; }
        public Action<StateMachineArgs> PreAction { get; internal set; }
        public int StateTo { get; internal set; }
        public Action<StateMachineArgs> PostAction { get; internal set; }
    }
}

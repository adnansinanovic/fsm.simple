using System;
using System.Collections.Generic;
using System.Linq;

namespace FSMSimple
{
    public class StateMachine
    {
        #region Constructor
        public StateMachine()
        {
            CurrentState = default(int);
        }
        #endregion

        #region Fields        
        private List<Transition> _transitions = new List<Transition>();
        #endregion

        #region Properties
        public int CurrentState { get; private set; }
        #endregion

        #region Events
        public event Action<StateMachineArgs> BeforeStateChanged;
        public event Action<StateMachineArgs> AfterStateChanged;
        #endregion

        #region Functions
        public void AddTransition(int startState, int eventCode, Func<bool> condition, Action<StateMachineArgs> preAction, int endState, Action<StateMachineArgs> postAction)
        {
            _transitions.Add(new Transition() { StateFrom = startState, Event = eventCode, Condition = condition, PreAction = preAction, StateTo = endState, PostAction = postAction });
        }

        public void RemoveTransition(Transition transition)
        {
            _transitions.Remove(transition);
        }

        public virtual void ProcessEvent(int eventCode)
        {
            var transition = _transitions.FirstOrDefault((x) => CurrentState == x.StateFrom && x.Event == eventCode && (x.Condition == null || x.Condition?.Invoke() == true));

            if (transition != null)
            {
                var args = new StateMachineArgs() { StartState = CurrentState, EndState = transition.StateTo, Event = eventCode };
                bool isStateDifferent = CurrentState != transition.StateTo;

                if (isStateDifferent)
                    BeforeStateChanged?.Invoke(args);

                transition.PreAction?.Invoke(args);

                CurrentState = transition.StateTo;

                if (isStateDifferent)
                    AfterStateChanged?.Invoke(args);

                transition.PostAction?.Invoke(args);
            }

        }
        #endregion
    }
}

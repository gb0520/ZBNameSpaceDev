using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.HierarchialFSM
{
    public class State
    {
        //상태표기
        public string Comment { get; protected set; }
        public UnityEvent enterEvent = new UnityEvent();
        public UnityEvent updateEvent = new UnityEvent();
        public UnityEvent exitEvent = new UnityEvent();

        private Machine machine;
        private List<Transition> transitions;

        public State(string Comment)
        {
            this.Comment = Comment;
        }

        public void AddTransition(Transition transition)
        {
            if (transitions == null) transitions = new List<Transition>();
            transitions.Add(transition);
        }
        public void MachineConnect(Machine machine) => this.machine = machine;

        public void Enter()
        {
            OnEnter();
            enterEvent.Invoke();
        }
        public void Update()
        {
            OnUpdate();
            updateEvent.Invoke();
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Count; i++) 
                {
                    if (transitions[i].condition.Invoke())
                    {
                        machine.SwapState(transitions[i].nextStateID);
                        return;
                    }
                }
            }
        }
        public void Exit()
        {
            OnExit();
            exitEvent.Invoke();
        }

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }
    }
}
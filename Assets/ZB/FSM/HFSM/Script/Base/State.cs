using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class State<ID> : IState<ID>
    {
        private ID name;
        private List<Transition<ID>> transitions;

        private UnityEvent enterEvent;
        private UnityEvent exitEvent;
        private UnityEvent updateEvent;

        protected Machine<ID> machine;

        public virtual ID GetName()
        {
            return name;
        }
        public void OnEnter()
        {
            Debug.Log($"Enter {GetName()}");
            EnterLogic();
            if (enterEvent != null) 
                enterEvent.Invoke();
        }
        public void OnExit()
        {
            ExitLogic();
            if (exitEvent != null)
                exitEvent.Invoke();
        }
        public void OnUpdate()
        {
            UpdateLogic();
            if (updateEvent != null)
                updateEvent.Invoke();
        }

        protected virtual void EnterLogic()
        {

        }
        protected virtual void ExitLogic()
        {

        }
        protected virtual void UpdateLogic()
        {

        }

        public void AddTransition(params Transition<ID>[] transition)
        {
            transitions = transitions ?? new List<Transition<ID>>();
            for (int i = 0; i < transition.Length; i++)
            {
                this.transitions.Add(transition[i]);
            }
        }
        public void OnConditionCheck()
        {
            if (transitions == null) return;

            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.Invoke())
                {
                    Debug.Log($"{GetName()} / {transitions[i].from} / {transitions[i].to}");
                    machine.SwapState(transitions[i].to);
                    return;
                }
            }
        }

        public void SetHighMachine(Machine<ID> machine) => this.machine = machine;

        public State(ID name, UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null)
        {
            this.name = name;
            if (enterAction != null)
            {
                enterEvent = new UnityEvent();
                enterEvent.AddListener(enterAction);
            }
            if (exitAction != null)
            {
                exitEvent = new UnityEvent();
                exitEvent.AddListener(exitAction);
            }
            if (stayAction != null)
            {
                updateEvent = new UnityEvent();
                updateEvent.AddListener(stayAction);
            }
        }
    }
}
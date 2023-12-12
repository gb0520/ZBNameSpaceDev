using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.StateMachine
{
    public class State
    {
        protected Machine machine;
        protected Factory factory;

        public State(Machine machine, Factory factory)
        {
            this.machine = machine;
            this.factory = factory;
        }

        public virtual void OnEnter()
        {

        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnExit()
        {

        }
    }
}
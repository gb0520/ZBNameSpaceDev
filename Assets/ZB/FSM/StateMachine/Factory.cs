using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.StateMachine
{
    public class Factory : MonoBehaviour
    {
        protected Machine machine;

        public virtual void OnInit()
        {

        }
        public virtual State BirthState()
        {
            return new State(machine, this);
        }
        public virtual State DeathState()
        {
            return new State(machine, this);
        }

        public void Init(Machine machine)
        {
            this.machine = machine;
            OnInit();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.FSM.ObjectHFSM
{
    public class Transition<ID>
    {
        public ID from { get; private set; }
        public ID to { get; private set; }
        public BoolDelegate condition { get; private set; }

        public bool Condition()
        {
            return condition.Invoke();
        }

        public Transition(ID from, ID to, BoolDelegate condition)
        {
            this.from = from;
            this.to = to;
            this.condition = condition;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class Transition
    {
        public BoolDelegate condition;
        public string nextStateID;

        public Transition(BoolDelegate condition, string nextStateID)
        {
            this.condition = condition;
            this.nextStateID = nextStateID;
        }
    }
}
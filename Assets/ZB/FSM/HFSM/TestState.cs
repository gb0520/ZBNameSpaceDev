using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class TestState<ID> : State<ID>
    {
        protected override void UpdateLogic()
        {
            Debug.Log($"{GetName()}");
        }

        public TestState
            (ID name, UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction)
        {

        }
    }

    public class Move<ID> : State<ID>
    {
        Vector2 moveDir;
        float movePower = 3;

        protected override void UpdateLogic()
        {

        }

        public Move
            (ID name, UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction)
        {
            this.movePower = movePower;
        }
    }
}
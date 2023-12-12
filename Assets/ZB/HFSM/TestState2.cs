using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class TestState2<ID> : State<ID>
    {
        protected override void EnterLogic()
        {
            Debug.Log("TestState2 Enter");
        }

        public TestState2
            (ID name, UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction) { }
    }
}
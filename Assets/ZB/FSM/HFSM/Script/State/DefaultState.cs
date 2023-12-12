using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class DefaultState<ID> : State<ID>
    {
        public DefaultState
            (ID name,
            UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction)
        {

        }
    }
}
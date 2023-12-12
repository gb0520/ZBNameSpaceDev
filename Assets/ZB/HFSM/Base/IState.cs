using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public interface IState<ID>
    {
        ID GetName();
        void OnEnter();
        void OnUpdate();
        void OnExit();
        void AddTransition(params Transition<ID>[] transition);
        void OnConditionCheck();
        void SetHighMachine(Machine<ID> machine);
    }
}
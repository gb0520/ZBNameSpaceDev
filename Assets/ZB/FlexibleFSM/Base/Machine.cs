using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public delegate bool BoolDelegate();
    [System.Serializable]
    public class Machine
    {
        //���� ���� �ڸ�Ʈ
        public string stateComment { get => currentState.Comment; }
        //���� ����
        private State currentState;
        [SerializeField]
        private string currentStateID;
        //���� ����
        private State firstState;
        [SerializeField]
        private string firstStateID;

        //��ϵ� ���µ�
        private Dictionary<string, State> states;

        //���� �߰�
        public void AddState(string id, State state, bool isFirstState = false)
        {
            if (states == null) states = new Dictionary<string, State>();
            if (!states.ContainsKey(id))
            {
                states.Add(id, state);
                state.MachineConnect(this);
            }
            if (isFirstState)
            {
                firstState = state;
                firstStateID = id;
            }
        }
        //���� ��ȯ ���� �߰�
        public void AddTransition(string from, string to, BoolDelegate condition)
        {
            if (states.ContainsKey(from) &&
                states.ContainsKey(to))
            {
                states[from].AddTransition(new Transition(condition, to));
            }
        }
        //���� ��ȯ
        public void SwapState(string to)
        {
            if (states.ContainsKey(to))
            {
                currentState.Exit();
                currentState = states[to];
                currentStateID = to;
                currentState.Enter();
            }
        }
        public void SwapState(string from, string to)
        {
            if (states.ContainsKey(from) &&
                states.ContainsKey(to) &&
                currentStateID == from)
            {
                currentState.Exit();
                currentState = states[to];
                currentStateID = to;
                currentState.Enter();
            }
        }
        public void Update()
        {
            if (currentState == null)
            {
                currentState = firstState;
                currentStateID = firstStateID;
                currentState.Enter();
            }
            currentState.Update();
        }
    }
}
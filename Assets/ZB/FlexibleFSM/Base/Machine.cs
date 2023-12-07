using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public delegate bool BoolDelegate();
    [System.Serializable]
    public class Machine
    {
        //현재 상태 코멘트
        public string stateComment { get => currentState.Comment; }
        //현재 상태
        private State currentState;
        [SerializeField]
        private string currentStateID;
        //최초 상태
        private State firstState;
        [SerializeField]
        private string firstStateID;

        //등록된 상태들
        private Dictionary<string, State> states;

        //상태 추가
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
        //상태 전환 조건 추가
        public void AddTransition(string from, string to, BoolDelegate condition)
        {
            if (states.ContainsKey(from) &&
                states.ContainsKey(to))
            {
                states[from].AddTransition(new Transition(condition, to));
            }
        }
        //상태 전환
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.FSM.ObjectHFSM
{
    public delegate bool BoolDelegate();
    [System.Serializable]
    public class Machine<ID> : IState<ID>
    {
        public ID nowStateName { get; private set; }
        public ID machineName { get; private set; }

        //상위머신
        private Machine<ID> highMachine;
        //상태 리스트
        private List<IState<ID>> states;
        //서브머신 리스트
        private List<Machine<ID>> subMachines;
        //상태변환조건 리스트
        private List<Transition<ID>> transitions;

        //최초상태
        private IState<ID> firstState;
        //현재상태
        private IState<ID> nowState;

        //작동중 여부
        [SerializeField]
        private bool activing;
        
        /// <summary>
        /// ID 일치하는지 판단
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool IDEqual(ID t1, ID t2)
        {
            return t1.Equals(t2);
        }

        public Machine(ID name)
        {
            this.machineName = name;
        }


        //* 상위 머신으로써의 역할
        /// <summary>
        /// 첫번째 상태 지정
        /// </summary>
        /// <param name="state"></param>
        public void AddFirstState(IState<ID> state)
        {
            firstState = state;
        }
        /// <summary>
        /// 상태 추가
        /// </summary>
        /// <param name="state"></param>
        public void AddState(params IState<ID>[] state)
        {
            states = states ?? new List<IState<ID>>();
            for (int i = 0; i < state.Length; i++)
            {
                states.Add(state[i]);
                state[i].SetHighMachine(this);
            }
        }
        /// <summary>
        /// 서브머신 추가
        /// </summary>
        /// <param name="subMachine"></param>
        public void AddSubMachine(params Machine<ID>[] subMachine)
        {
            subMachines = subMachines ?? new List<Machine<ID>>();
            for (int i = 0; i < subMachine.Length; i++)
            {
                subMachines.Add(subMachine[i]);
            }

            AddState(subMachine);
        }
        /// <summary>
        /// 서브머신 가져오기
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Machine<ID> GetSubMachine(ID name)
        {
            if (subMachines == null)
                return null;

            for (int i = 0; i < subMachines.Count; i++)
            {
                if (IDEqual(subMachines[i].machineName, name))
                {
                    return subMachines[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 활성화, 비활성화
        /// </summary>
        /// <param name="active"></param>
        public void Active(bool active)
        {
            activing = active;
            if (!active) nowState = null;
        }
        /// <summary>
        /// 상태전환 시도
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool TrySwapState(ID from, ID to)
        {
            //현재 상태와, from이 일치할 경우
            if (IDEqual(nowState.GetName(), from)) 
            {
                //리스트에서 다음상태 가져온다.
                IState<ID> nextState = GetState(to);
                if (nextState != null)
                {
                    nowState.OnExit();
                    nowState = nextState;
                    nowStateName = nextState.GetName();
                    nowState.OnEnter();
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 상태전환
        /// </summary>
        /// <param name="to"></param>
        public void SwapState(ID to)
        {
            //리스트에서 다음상태 가져온다.
            IState<ID> nextState = GetState(to);
            if (nextState != null)
            {
                nowState.OnExit();
                nowState = nextState;
                nowStateName = nextState.GetName();
                nowState.OnEnter();
            }
        }
        

        //* IState에 의해 구현됨, 상태로써의 역할
        public void OnEnter()
        {
            if (nowState == null)
            {
                nowState = firstState;
                SetNowStateFirst();
            }
            nowState.OnEnter();
        }
        public void OnExit()
        {
            nowState.OnExit();
        }
        public void OnUpdate()
        {
            if (activing)
            {
                if (nowState == null)
                {
                    SetNowStateFirst();
                    nowState.OnEnter();
                }
                nowState.OnUpdate();
                nowState.OnConditionCheck();
                Debug.Log(nowState.GetName());
            }
        }

        public ID GetName()
        {
            return machineName;
        }
        public void AddTransition(params Transition<ID>[] transition)
        {
            IState<ID> state = null;
            Machine<ID> machine = null;

            for (int i = 0; i < transition.Length; i++)
            {
                state = GetState(transition[i].from);
                machine = GetSubMachine(transition[i].from);

                if (state != null)
                {
                    if (machine != null)
                    {
                        machine.AddOwnTransition(transition);
                        return;
                    }

                    state.AddTransition(transition);
                }
            }
        }
        public void OnConditionCheck()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.Invoke())
                {
                    highMachine.SwapState(transitions[i].to);
                    return;
                }
            }
        }
        public void SetHighMachine(Machine<ID> machine)
        {
            this.highMachine = machine;
        }
        /// <summary>
        /// 해당머신의 Transition 추가
        /// </summary>
        /// <param name="transition"></param>
        public void AddOwnTransition(params Transition<ID>[] transition)
        {
            transitions = transitions ?? new List<Transition<ID>>();
            for (int i = 0; i < transition.Length; i++)
            {
                transitions.Add(transition[i]);
            }
        }


        //상태 가져오기
        private IState<ID> GetState(ID name)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (IDEqual(states[i].GetName(), name))
                {
                    return states[i];
                }
            }
            return null;
        }
        private void SetNowStateFirst()
        {
            nowState = firstState;
            nowStateName = firstState.GetName();
        }
    }
}
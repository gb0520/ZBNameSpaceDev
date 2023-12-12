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

        //�����ӽ�
        private Machine<ID> highMachine;
        //���� ����Ʈ
        private List<IState<ID>> states;
        //����ӽ� ����Ʈ
        private List<Machine<ID>> subMachines;
        //���º�ȯ���� ����Ʈ
        private List<Transition<ID>> transitions;

        //���ʻ���
        private IState<ID> firstState;
        //�������
        private IState<ID> nowState;

        //�۵��� ����
        [SerializeField]
        private bool activing;
        
        /// <summary>
        /// ID ��ġ�ϴ��� �Ǵ�
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


        //* ���� �ӽ����ν��� ����
        /// <summary>
        /// ù��° ���� ����
        /// </summary>
        /// <param name="state"></param>
        public void AddFirstState(IState<ID> state)
        {
            firstState = state;
        }
        /// <summary>
        /// ���� �߰�
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
        /// ����ӽ� �߰�
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
        /// ����ӽ� ��������
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
        /// Ȱ��ȭ, ��Ȱ��ȭ
        /// </summary>
        /// <param name="active"></param>
        public void Active(bool active)
        {
            activing = active;
            if (!active) nowState = null;
        }
        /// <summary>
        /// ������ȯ �õ�
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool TrySwapState(ID from, ID to)
        {
            //���� ���¿�, from�� ��ġ�� ���
            if (IDEqual(nowState.GetName(), from)) 
            {
                //����Ʈ���� �������� �����´�.
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
        /// ������ȯ
        /// </summary>
        /// <param name="to"></param>
        public void SwapState(ID to)
        {
            //����Ʈ���� �������� �����´�.
            IState<ID> nextState = GetState(to);
            if (nextState != null)
            {
                nowState.OnExit();
                nowState = nextState;
                nowStateName = nextState.GetName();
                nowState.OnEnter();
            }
        }
        

        //* IState�� ���� ������, ���·ν��� ����
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
        /// �ش�ӽ��� Transition �߰�
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


        //���� ��������
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
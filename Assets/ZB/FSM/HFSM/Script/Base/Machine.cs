using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        private UnityEvent enterEvent;
        private UnityEvent exitEvent;
        private UnityEvent updateEvent;

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

        public Machine(ID name, UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null)
        {
            this.machineName = name;
            if (enterAction != null)
            {
                enterEvent = new UnityEvent();
                enterEvent.AddListener(enterAction);
            }
            if (exitAction != null)
            {
                exitEvent = new UnityEvent();
                exitEvent.AddListener(exitAction);
            }
            if (stayAction != null)
            {
                updateEvent = new UnityEvent();
                updateEvent.AddListener(stayAction);
            }
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
        public void AddState(bool firstIndexIsFirstState, params IState<ID>[] state)
        {
            states = states ?? new List<IState<ID>>();
            if (state.Length > 0)
            {
                if (firstIndexIsFirstState)
                {
                    firstState = state[0];
                }

                for (int i = 0; i < state.Length; i++)
                {
                    states.Add(state[i]);
                    state[i].SetHighMachine(this);
                }
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

            AddState(false, subMachine);
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
        public void Active(bool active = true)
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
                return SwapState(to);
            }
            return false;
        }
        /// <summary>
        /// ������ȯ
        /// </summary>
        /// <param name="to"></param>
        public bool SwapState(ID to)
        {
            //����Ʈ���� �������� �����´�.
            IState<ID> nextState = GetState(to);
            Machine<ID> machine = GetSubMachine(to);

            if (nextState != null)
            {
                //���� ���� Exit
                nowState.OnExit();
                //�������� ����
                nowState = nextState;
                nowStateName = nextState.GetName();
                //�������°� ����ӽ��� ���, �ʱ���·� ����
                if (machine != null) machine.SetNowStateFirst();
                //��������
                nowState.OnEnter();
                return true;
            }
            return false;
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
            if (enterEvent != null)
                enterEvent.Invoke();
        }
        public void OnExit()
        {
            nowState.OnExit();
            if (exitEvent != null)
                exitEvent.Invoke();
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
                if (updateEvent != null)
                    updateEvent.Invoke();
                nowState.OnConditionCheck();
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
                        machine.AddOwnTransition(transition[i]);
                        return;
                    }

                    state.AddTransition(transition[i]);
                }
            }
        }
        public void OnConditionCheck()
        {
            if (transitions == null) return;
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].condition.Invoke())
                {
                    Debug.Log($"{transitions[i].from} / {transitions[i].to}");
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
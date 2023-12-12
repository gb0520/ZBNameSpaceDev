using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.FSM.ObjectHFSM
{
    public class TestPlayer2 : MonoBehaviour
    {
        public enum STATENAME {
            none,

            main,

            sub1,
            sub1_1,
            sub1_2,

            sub2,
            sub2_1,
            sub2_2,
            
            state
        }

        private Machine<STATENAME> mainMachine = new Machine<STATENAME>(STATENAME.main);
        [SerializeField] private float mainValue;
        [SerializeField] private float subValue1;
        [SerializeField] private float subValue2;
        [SerializeField] private STATENAME mainMachineStateName;
        [SerializeField] private STATENAME subMachine1StateName;
        [SerializeField] private STATENAME subMachine2StateName;

        private void Awake()
        {
            //* 서브머신1 정의────────────────────────────────────────────────────────────────────────────────
            Machine<STATENAME> subMachine1 = new Machine<STATENAME>(STATENAME.sub1);

            //상태 정의
            TestState<STATENAME> stateForSub1_1 = new TestState<STATENAME>(STATENAME.sub1_1);
            TestState<STATENAME> stateForSub1_2 = new TestState<STATENAME>(STATENAME.sub1_2);

            //전환조건 정의
            Transition<STATENAME> transitionForSub1_1To2 =
                new Transition<STATENAME>(STATENAME.sub1_1, STATENAME.sub1_2, () => subValue1 < 0);
            Transition<STATENAME> transitionForSub1_2To1 =
                new Transition<STATENAME>(STATENAME.sub1_2, STATENAME.sub1_1, () => subValue1 > 0);

            //상태, 전환조건 서브머신1에 연결
            subMachine1.AddState(stateForSub1_1, stateForSub1_2);
            subMachine1.AddFirstState(stateForSub1_1);
            subMachine1.AddTransition(transitionForSub1_1To2, transitionForSub1_2To1);
            //서브머신1 활성화
            subMachine1.Active(true);

            //* 서브머신2 정의────────────────────────────────────────────────────────────────────────────────
            Machine<STATENAME> subMachine2 = new Machine<STATENAME>(STATENAME.sub2);

            //상태 정의
            TestState<STATENAME> stateForSub2_1 = new TestState<STATENAME>(STATENAME.sub2_1);
            TestState<STATENAME> stateForSub2_2 = new TestState<STATENAME>(STATENAME.sub2_2);

            //전환조건 정의
            Transition<STATENAME> transitionForSub2_1To2 =
                new Transition<STATENAME>(STATENAME.sub2_1, STATENAME.sub2_2, () => subValue2 < 0);
            Transition<STATENAME> transitionForSub2_2To1 =
                new Transition<STATENAME>(STATENAME.sub2_2, STATENAME.sub2_1, () => subValue2 > 0);

            //상태, 전환조건 서브머신1에 연결
            subMachine2.AddState(stateForSub2_1, stateForSub2_2);
            subMachine2.AddFirstState(stateForSub2_1);
            subMachine2.AddTransition(transitionForSub2_1To2, transitionForSub2_2To1);
            //서브머신2 활성화
            subMachine2.Active(true);

            //* 메인머신────────────────────────────────────────────────────────────────────────────────
            //상태 정의
            TestState<STATENAME> stateForMain = new TestState<STATENAME>(STATENAME.state);

            //전환조건 정의
            Transition<STATENAME> transitionForMain_1To2 =
                new Transition<STATENAME>(STATENAME.sub1, STATENAME.sub2, () => mainValue < 0);
            Transition<STATENAME> transitionForMain_2To1 =
                new Transition<STATENAME>(STATENAME.sub2, STATENAME.sub1, () => mainValue > 0);
            Transition<STATENAME> transitionForMain_1ToState =
                new Transition<STATENAME>(STATENAME.sub1, STATENAME.state, () => mainValue > 100);
            Transition<STATENAME> transitionForMain_StateTo1 =
                new Transition<STATENAME>(STATENAME.state, STATENAME.sub1, () => mainValue < 100);

            //서브머신, 전환조건 메인머신에 연결
            mainMachine.AddSubMachine(subMachine1, subMachine2);
            mainMachine.AddState(stateForMain);
            mainMachine.AddFirstState(subMachine1);
            mainMachine.AddTransition(transitionForMain_1To2);
            mainMachine.AddTransition(transitionForMain_2To1);
            mainMachine.AddTransition(transitionForMain_1ToState);
            mainMachine.AddTransition(transitionForMain_StateTo1);
            //메인머신 활성화
            mainMachine.Active(true);
        }

        private void Update()
        {
            mainMachine.OnUpdate();
        }
    }
}
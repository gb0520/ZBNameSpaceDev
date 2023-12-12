using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.FSM.ObjectHFSM
{
    public class TestPlayer : MonoBehaviour
    {
        public enum TestStateID { none, main, test1, test2 }

        private Machine<TestStateID> mainMachine = new Machine<TestStateID>(TestStateID.main);
        [SerializeField] private float tempValue;
        [SerializeField] private TestStateID nowStateName;

        private void Awake()
        {
            //상태 정의
            TestState<TestStateID> testState1 = new TestState<TestStateID>(TestStateID.test1);
            TestState2<TestStateID> testState2 = new TestState2<TestStateID>(TestStateID.test2);

            //전환조건 정의
            Transition<TestStateID> state1To2 =
                new Transition<TestStateID>(TestStateID.test1, TestStateID.test2, () => tempValue > 0);
            Transition<TestStateID> state2To1 =
                new Transition<TestStateID>(TestStateID.test2, TestStateID.test1, () => tempValue < 0);

            //상태, 전환조건 머신에 연결
            mainMachine.AddFirstState(testState1);
            mainMachine.AddState(testState1, testState2);

            //전환조건 머신에 연결
            mainMachine.AddTransition(state1To2, state2To1);

            //머신 활성화
            mainMachine.Active(true);
        }

        private void Update()
        {
            mainMachine.OnUpdate();
            nowStateName = mainMachine.nowStateName;
        }
    }
}
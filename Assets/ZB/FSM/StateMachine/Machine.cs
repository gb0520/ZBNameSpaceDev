using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.StateMachine
{
    public class Machine : MonoBehaviour
    {
        [SerializeField] protected Factory factory;

        protected State nowState;

        public virtual void SwapState(State state)
        {
            nowState.OnExit();
            nowState = state;
            nowState.OnEnter();
        }

        protected void Awake()
        {
            factory.Init(this);
        }
        protected void OnEnable()
        {
            nowState = factory.BirthState();
            nowState.OnEnter();
        }
        protected void Update()
        {
            if (nowState != null) 
                nowState.OnUpdate();
        }
    }
}
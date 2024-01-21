using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.Screen.ScreenTo3DBoreInteract
{
    public class Object : MonoBehaviour
    {
        public UnityEvent InputEnterEvent;
        public UnityEvent InputExitEvent;
        public bool CurrentInputIsEnter { get; private set; }

        public void InputEnter(InputValue inputValue)
        {
            InputEnterEvent.Invoke();
            CurrentInputIsEnter = true;
            OnInputEnter(inputValue);
        }
        public void InputExit(InputValue inputValue)
        {
            InputExitEvent.Invoke();
            CurrentInputIsEnter = false;
            OnInputExit(inputValue);
        }
        public virtual void OnInputEnter(InputValue inputValue)
        {

        }
        public virtual void OnInputExit(InputValue inputValue)
        {

        }
    }
}
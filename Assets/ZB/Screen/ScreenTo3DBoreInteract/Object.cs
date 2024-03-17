using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.Screen.ScreenTo3DBoreInteract
{
    public class Object : MonoBehaviour
    {
        [Header("ScreenTo3DBoreInteract")]
        public UnityEvent LeftInputEnterEvent;
        public UnityEvent LeftInputExitEvent;
        public UnityEvent RightInputEnterEvent;
        public UnityEvent RightInputExitEvent;
        public bool CurrentInputIsEnter { get; private set; }

        public void InputEnter(InputValue inputValue)
        {
            if (inputValue == InputValue.LeftMouse) 
                LeftInputEnterEvent.Invoke();
            if (inputValue == InputValue.RightMouse)
                RightInputEnterEvent.Invoke();
            CurrentInputIsEnter = true;
            OnInputEnter(inputValue);
        }
        public void InputExit(InputValue inputValue)
        {
            if (inputValue == InputValue.LeftMouse)
                LeftInputExitEvent.Invoke();
            if (inputValue == InputValue.RightMouse)
                RightInputExitEvent.Invoke();
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
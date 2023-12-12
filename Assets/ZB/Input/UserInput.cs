using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.ZBInput
{
    public delegate void Vector2Delegate(Vector2 vector2);
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private KeyInput[] keyInputs;
        [SerializeField] private Direction2DInput[] direction2DInputs;

        public void OnUpdate()
        {
            for (int i = 0; i < keyInputs.Length; i++)
            {
                keyInputs[i].OnUpdate();
            }
            for (int i = 0; i < direction2DInputs.Length; i++)
            {
                direction2DInputs[i].OnUpdate();
            }
        }
        public KeyInput GetKeyInput(string name)
        {
            for (int i = 0; i < keyInputs.Length; i++)
            {
                if (keyInputs[i].Comment == name)
                    return keyInputs[i];
            }
            return null;
        }
        public Direction2DInput GetDirection2DInput(string name)
        {
            for (int i = 0; i < direction2DInputs.Length; i++)
            {
                if (direction2DInputs[i].Comment == name)
                    return direction2DInputs[i];
            }
            return null;
        }

        [System.Serializable]
        public class KeyInput
        {
            public string Comment;
            public KeyCode KeyCode;
            private UnityEvent DownEvent;
            private UnityEvent UpEvent;
            private UnityEvent StayEvent;
            public void AddDownEvent(UnityAction action)
            {
                DownEvent = DownEvent ?? new UnityEvent();
                DownEvent.AddListener(action);
            }
            public void AddUpEvent(UnityAction action)
            {
                UpEvent = UpEvent ?? new UnityEvent();
                UpEvent.AddListener(action);
            }
            public void AddStayEvent(UnityAction action)
            {
                StayEvent = StayEvent ?? new UnityEvent();
                StayEvent.AddListener(action);
            }
            public void RemoveDownEvent(UnityAction action)
            {
                if (DownEvent == null) return;
                DownEvent.RemoveListener(action);
            }
            public void RemoveUpEvent(UnityAction action)
            {
                if (UpEvent == null) return;
                UpEvent.RemoveListener(action);
            }
            public void RemoveStayEvent(UnityAction action)
            {
                if (StayEvent == null) return;
                StayEvent.RemoveListener(action);
            }
            public void RemoveAllDownEvent()
            {
                if (DownEvent == null) return;
                DownEvent.RemoveAllListeners();
            }
            public void RemoveAllUpEvent()
            {
                if (UpEvent == null) return;
                UpEvent.RemoveAllListeners();
            }
            public void RemoveAllStayEvent()
            {
                if (StayEvent == null) return;
                StayEvent.RemoveAllListeners();
            }
            public void OnUpdate()
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode) &&
                    DownEvent != null)
                {
                    DownEvent.Invoke();
                }
                if (UnityEngine.Input.GetKeyUp(KeyCode) &&
                    UpEvent != null)
                {
                    UpEvent.Invoke();
                }
                if (UnityEngine.Input.GetKey(KeyCode) &&
                    StayEvent != null)
                {
                    StayEvent.Invoke();
                }
            }
        }

        [System.Serializable]
        public class Direction2DInput
        {
            public string Comment;
            public KeyCode UpKey;
            public KeyCode DownKey;
            public KeyCode LeftKey;
            public KeyCode RightKey;
            public Vector2 Direction;
            [SerializeField] private bool left = false;
            [SerializeField] private bool right = false;
            [SerializeField] private bool up = false;
            [SerializeField] private bool down = false;
            private Vector2Delegate StayEvent;

            public void OnUpdate()
            {
                if (UnityEngine.Input.GetKey(UpKey))
                    up = true;
                else
                    up = false;
                if (UnityEngine.Input.GetKey(DownKey))
                    down = true;
                else
                    down = false;
                if (UnityEngine.Input.GetKey(LeftKey))
                    left = true;
                else
                    left = false;
                if (UnityEngine.Input.GetKey(RightKey))
                    right = true;
                else
                    right = false;

                int x = 0;
                int y = 0;
                if (up && !down) y = 1;
                else if (!up && down) y = -1; 
                else y = 0;
                if (right && !left) x = 1;
                else if(!right && left) x = -1;
                else x = 0;

                Direction = new Vector2(x, y);

                StayEvent.Invoke(Direction);
            }
            public void AddVector2Delegate(Vector2Delegate callback)
            {
                StayEvent += callback;
            }

            public void RemoveVector2Delegate(Vector2Delegate callback)
            {
                StayEvent -= callback;
            }

            public void RemoveAllVector2Delegate()
            {
                StayEvent = null;
            }
        }
    }
}
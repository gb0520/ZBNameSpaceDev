using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZB.Screen.ScreenToUiBoreInteract
{
    public enum InputValue { None, LeftMouse, RightMouse }
    public class Manager : MonoBehaviour
    {
        [Header("ScreenToUiBoreInteract")]
        [SerializeField]
        private bool isActive;
        [SerializeField]
        private InputValue currentInputValue;
        [SerializeField]
        private List<Object> currentScreenBoreObjects;
        [SerializeField]
        private Canvas targetCanvas;
        [SerializeField]
        private LayerMask targetLayer;

        public void Active(bool active)
        {
            isActive = active;
            if (inputWaitCycle != null) StopCoroutine(inputWaitCycle);

            if (active)
            {
                inputWaitCycle = InputWaitCycle();
                StartCoroutine(inputWaitCycle);
            }
        }

        public void OnInputEnter(InputValue inputValue)
        {
            currentInputValue = inputValue;
            currentScreenBoreObjects = GetMouseOverlapObjects(Input.mousePosition);

            if (currentScreenBoreObjects == null) return;

            for (int i = 0; i < currentScreenBoreObjects.Count; i++)
            {
                currentScreenBoreObjects[i].InputEnter(inputValue);
            }
        }
        public void OnInputExit(InputValue inputValue)
        {
            currentInputValue = inputValue;
            currentScreenBoreObjects = GetMouseOverlapObjects(Input.mousePosition);

            if (currentScreenBoreObjects == null) return;

            for (int i = 0; i < currentScreenBoreObjects.Count; i++)
            {
                currentScreenBoreObjects[i].InputExit(inputValue);
            }
        }

        private List<Object> GetMouseOverlapObjects(Vector3 position)
        {
            if (targetCanvas == null) return null;

            List<Object> screenBoreObjects = new List<Object>();

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count <= 0) return null;

            for (int i = 0; i < results.Count; i++)
            {
                int layer = 1 << results[i].gameObject.layer;
                Object screenBoreObject;
                if ((targetLayer & layer) != 0 &&
                    results[i].gameObject.TryGetComponent(out screenBoreObject))
                {
                    screenBoreObjects.Add(screenBoreObject);
                }
            }
            return screenBoreObjects;
        }
        private IEnumerator inputWaitCycle;
        private IEnumerator InputWaitCycle()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnInputEnter(InputValue.LeftMouse);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    OnInputExit(InputValue.LeftMouse);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    OnInputEnter(InputValue.RightMouse);
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    OnInputExit(InputValue.RightMouse);
                }

                yield return null;
            }
        }
        private void Awake()
        {
            Active(isActive);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZB.Screen.ScreenTo3DBoreInteract
{
    public enum InputValue { None, LeftMouse, RightMouse }
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        private bool isActive;
        [SerializeField]
        private InputValue currentInputValue;
        [SerializeField]
        private List<Object> current3DBoreObject;
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
            current3DBoreObject = GetMouseOverlapObjects(Input.mousePosition);

            if (current3DBoreObject == null) return;

            for (int i = 0; i < current3DBoreObject.Count; i++)
            {
                current3DBoreObject[i].InputEnter(inputValue);
            }
        }
        public void OnInputExit(InputValue inputValue)
        {
            currentInputValue = inputValue;
            current3DBoreObject = GetMouseOverlapObjects(Input.mousePosition);

            if (current3DBoreObject == null) return;

            for (int i = 0; i < current3DBoreObject.Count; i++)
            {
                current3DBoreObject[i].InputExit(inputValue);
            }
        }

        private List<Object> GetMouseOverlapObjects(Vector3 position)
        {
            List<Object> overlappedObjects = new List<Object>();

            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            if (hits.Length <= 0) return null;

            for (int i = 0; i < hits.Length; i++)
            {
                Object hitObject = null;
                if (hits[i].transform.TryGetComponent(out hitObject)) 
                    overlappedObjects.Add(hitObject);
            }

            return overlappedObjects;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ZB.Screen
{
    public class ScreenDragImplement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] Image m_img_screen;
        [SerializeField] RectTransform m_rtf_inputStart;
        [SerializeField] RectTransform m_rtf_inputEnd;
        [SerializeField] Image m_img_inputStart;
        [SerializeField] Image m_img_inputEnd;
        [SerializeField] ScreenDrag m_screenDrag;

        public bool m_Dragging { get => m_dragging; }
        public bool m_DragStop { get => m_dragstop; }
        public Vector2 m_DragVector { get => m_rtf_inputEnd.position - m_rtf_inputStart.position; }
        public Vector2 m_DragVector_OneFrame { get => m_dragVector_OneFrame; }
        
        private Vector2 m_endFollow;
        [SerializeField] private Vector2 m_dragVector_OneFrame;
        [SerializeField] private bool m_dragging;
        [SerializeField] private bool m_dragstop;
        private float m_dragStopCoyoteT;

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_rtf_inputStart.position = eventData.position;
            m_dragging = true;
            m_screenDrag.UEvent_OnEnter.Invoke();

            m_endFollow = m_rtf_inputEnd.position;

            dragStopWatch_C = dragStopWatch();
            StartCoroutine(dragStopWatch_C);
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_rtf_inputEnd.position = eventData.position;
            m_dragVector_OneFrame = new Vector2(m_rtf_inputEnd.position.x - m_endFollow.x, m_rtf_inputEnd.position.y - m_endFollow.y);
            m_endFollow = m_rtf_inputEnd.position;

            m_dragStopCoyoteT = 0.1f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_dragging = false;
            m_screenDrag.UEvent_OnExit.Invoke();

            StopCoroutine(dragStopWatch_C);
            m_dragstop = false;
        }

        private void Awake()
        {
            if (m_screenDrag.ShowDebug)
            {
                m_img_screen.color = new Color(1, 1, 1, 0.2f);
                m_img_inputStart.enabled = true;
                m_img_inputEnd.enabled = true;
            }
            else
            {
                m_img_screen.color = Color.clear;
                m_img_inputStart.enabled = false;
                m_img_inputEnd.enabled = false;
            }
        }
        private void OnDisable()
        {
            m_dragging = false;
            m_dragstop = false;
            StopAllCoroutines();
            m_dragstop = false;
        }

        IEnumerator dragStopWatch_C;
        IEnumerator dragStopWatch()
        {
            while(true)
            {
                if (m_dragStopCoyoteT > 0)
                {
                    m_dragStopCoyoteT -= Time.deltaTime;
                    m_dragstop = false;
                    yield return true;
                }
                else
                {
                    m_dragstop = true;
                    yield return true;
                }
            }
        }
    }
}
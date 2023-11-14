using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZB.Screen
{
    public class ScreenDrag : MonoBehaviour
    {
        public bool ShowDebug { get => showDebug; }
        [SerializeField] bool showDebug;

        /// <summary>
        /// �巡�� ������ �� �̺�Ʈ
        /// </summary>
        public UnityEvent UEvent_OnEnter;

        /// <summary>
        /// �巡�� ���� �� �̺�Ʈ
        /// </summary>
        public UnityEvent UEvent_OnExit;

        /// <summary>
        /// �ֱ� �巡���� ����
        /// </summary>
        public Vector2 DragVector { get => m_screen.m_DragVector; }

        /// <summary>
        /// 1������ �˻� ���� �̵��� ����
        /// </summary>
        public Vector2 DragVector_OneFrame { get => m_screen.m_DragVector_OneFrame; }

        /// <summary>
        /// �ֱ� �巡���� ���� ũ��
        /// </summary>
        public float Magnitude { get => m_screen.m_DragVector.magnitude; }

        /// <summary>
        /// 1������ �˻� ���� �̵��� ���� ũ��
        /// </summary>
        public float Magnitude_OneFrame { get => m_screen.m_DragVector_OneFrame.magnitude; }

        /// <summary>
        /// �巡�� ���� ��
        /// </summary>
        public bool Dragging { get => m_screen.m_Dragging; }

        /// <summary>
        /// �巡�� ���� �߿�, ����
        /// </summary>
        public bool DragStop { get => m_screen.m_DragStop; }

        [SerializeField] ScreenDragImplement m_screen;

        /// <summary>
        /// �巡�� ��ũ�� Ȱ��ȭ
        /// </summary>
        /// <param name="Ȱ��ȭ ����"></param>
        public void Active(bool active)
        {
            m_screen.gameObject.SetActive(active);
        }
    }
}
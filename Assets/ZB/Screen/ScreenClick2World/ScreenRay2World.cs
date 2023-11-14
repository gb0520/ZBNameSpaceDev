using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Screen
{
    public class ScreenRay2World : MonoBehaviour
    {
        /// <summary>
        /// 접근한 월드좌표계의 오브젝트
        /// </summary>
        public Transform m_TF_CurrentTarget { get => m_tf_currentTarget; }
        /// <summary>
        /// 접근한 월드좌표계의 오브젝트
        /// </summary>
        public Transform[] m_TF_CurrentTargets { get => m_tf_currentTargets; }

        [Header("먼저 설정이 필요한 옵션들")]
        [Tooltip("충돌 대상 레이어")]
        [SerializeField] private LayerMask m_targetLayer;

        [Header("확인용 옵션들")]
        [SerializeField] private RectTransform m_rtf_point;
        [SerializeField] private bool m_activeOnStart;

        [SerializeField] private Transform m_tf_currentTarget;
        [SerializeField] private Transform[] m_tf_currentTargets;
        private Ray ray;
        private RaycastHit hit;
        private RaycastHit[] hits;

        public void RayShot(bool checkMultiple = false)
        {
            Logic(checkMultiple);
        }
        public bool TryRayShot(out Transform outResult)
        {
            Logic(false);

            outResult = null;
            if (m_tf_currentTarget != null && 
                m_tf_currentTarget != null)
            {
                outResult = m_tf_currentTarget;
                return true;
            }
            return false;
        }
        public bool TryRayShot(out Transform[] outResult)
        {
            Logic(true);

            outResult = null;
            if (m_tf_currentTarget != null &&
                m_tf_currentTarget.TryGetComponent(out outResult))
            {
                outResult = m_tf_currentTargets;
                return true;
            }
            return false;
        }
        public bool TryRayShot<T>(out T outResult) where T : Component
        {
            RayShot();
            outResult = null;
            if (m_tf_currentTarget != null &&
                m_tf_currentTarget.TryGetComponent(out outResult))
            {
                return true;
            }
            return false;
        }
        public bool TryRayShot<T>(out T[] outResult) where T : Component
        {
            RayShot();
            outResult = null;

            if (m_tf_currentTarget != null)
            {
                List<T> list = new List<T>();
                T temp;
                for (int i = 0; i < m_tf_currentTargets.Length; i++)
                {
                    if (m_tf_currentTargets[i].TryGetComponent(out temp))
                    {
                        list.Add(temp);
                    }
                }

                if (list.Count == 0) return false;

                outResult = new T[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    outResult[i] = list[i];
                }
                return true;
            }

            return false;
        }

        public void RayShotByMouse(bool checkMultiple = false)
        {
            m_rtf_point.position = Input.mousePosition;
            RayShot(checkMultiple);
        }
        public bool TryRayShotByMouse(out Transform outResult)
        {
            m_rtf_point.position = Input.mousePosition;
            return TryRayShot(out outResult);
        }
        public bool TryRayShotByMouse(out Transform[] outResult)
        {
            m_rtf_point.position = Input.mousePosition;
            return TryRayShot(out outResult);
        }
        public bool TryRayShotByMouse<T>(out T outResult) where T : Component
        {
            m_rtf_point.position = Input.mousePosition;
            return TryRayShot(out outResult);
        }
        public bool TryRayShotByMouse<T>(out T[] outResult) where T : Component
        {
            m_rtf_point.position = Input.mousePosition;
            return TryRayShot(out outResult);
        }

        /// <summary>
        /// 포인터 이동
        /// </summary>
        public void MovePoint(Vector2 rectPos)
        {
            m_rtf_point.position = rectPos;
        }

        private void Logic(bool checkMultiple)
        {
            ray = Camera.main.ScreenPointToRay(m_rtf_point.position);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_targetLayer))
            {
                RayShotReaction rayShotReaction;
                m_tf_currentTarget = hit.collider.transform;

                if (m_tf_currentTarget.TryGetComponent(out rayShotReaction))
                {
                    rayShotReaction.Event_OnShot.Invoke();
                }

                if (checkMultiple)
                {
                    hits = Physics.RaycastAll(ray);
                    m_tf_currentTargets = new Transform[hits.Length];
                    for (int i = 0; i < hits.Length; i++)
                    {
                        m_TF_CurrentTargets[i] = hits[i].collider.transform;
                        if (m_TF_CurrentTargets[i].TryGetComponent(out rayShotReaction))
                        {
                            rayShotReaction.Event_OnShot.Invoke();
                        }
                    }
                }
            }
        }
    }
}
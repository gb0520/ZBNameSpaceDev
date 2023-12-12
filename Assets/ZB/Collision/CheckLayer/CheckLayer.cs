using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.CollisionCheck
{
    public class CheckLayer : MonoBehaviour
    {
        public UnityEvent uEvent_Enter;
        public UnityEvent uEvent_Exit;

        public Transform tf_LastTouched { get => tf_lastTouched; }
        public bool Touching { get => touching; }

        [SerializeField] LayerMask targetLayer;
        [SerializeField] Transform tf_lastTouched;
        [SerializeField] bool touching;

        private void OnCollisionEnter(Collision collision)
        {
            if(CheckCondition(collision.gameObject))
            {
                tf_lastTouched = collision.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (CheckCondition(collision.gameObject))
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (CheckCondition(other.gameObject))
            {
                tf_lastTouched = other.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (CheckCondition(other.gameObject))
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (CheckCondition(collision.gameObject))
            {
                tf_lastTouched = collision.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (CheckCondition(collision.gameObject))
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CheckCondition(collision.gameObject))
            {
                tf_lastTouched = collision.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (CheckCondition(collision.gameObject))
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }

        private bool CheckCondition(GameObject gameObject)
        {
            return ((1 << gameObject.layer) & targetLayer) != 0;
        }
    }
}
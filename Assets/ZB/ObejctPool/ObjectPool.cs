using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Object
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] bool InitOnAwake;
        [SerializeField] SinglePool[] pools;

        public Transform Spawn(string name, Vector3 position)
        {
            SinglePool pool;
            if (TryGetPool(name, out pool))
            {
                return pool.Spawn(position);
            }
            return null;
        }

        public T Spawn<T>(string name, Vector3 position)
        {
            return Spawn(name, position).GetComponent<T>();
        }

        public bool Spawn_TryGetComponent<T>(string name, Vector3 position, out T target)
        {
            return Spawn(name, position).TryGetComponent(out target);
        }

        public void Init()
        {
            for (int i = 0; i < pools.Length; i++)
            {
                pools[i].Init(transform);
            }
        }

        private bool TryGetPool(string name, out SinglePool singlePool)
        {
            singlePool = null;
            for (int i = 0; i < pools.Length; i++)
            {
                if (pools[i].Name == name)
                {
                    singlePool = pools[i];
                    return true;
                }
            }
            return false;
        }

        private void Awake()
        {
            if (InitOnAwake)
                Init();
        }

        [System.Serializable]
        public class SinglePool
        {
            public string Name { get => name; }

            [SerializeField] private string name;
            [SerializeField] private Transform original;
            [SerializeField] private Transform[] pool;
            [SerializeField] private int index;
            [SerializeField] private int initMax;
            private Transform parent;

            public Transform Spawn(Vector3 position)
            {
                Transform result = GetMargin();
                result.position = position;
                result.SetParent(parent);
                return result;
            }

            public T Spawn_GetCopmonent<T>(Vector3 position)
            {
                return Spawn(position).GetComponent<T>();
            }

            public bool Spawn_TryGetComponent<T>(Vector3 position, out T target)
            {
                Transform result = Spawn(position);
                return result.TryGetComponent(out target);
            }

            public Transform GetMargin()
            {
                index = (index + 1) % pool.Length;

                //여유 원소가 없다.
                if (pool[index].gameObject.activeSelf)
                {
                    Transform[] tempArray = pool;
                    pool = new Transform[tempArray.Length + 1];
                    for (int i = 0; i < tempArray.Length; i++)
                    {
                        pool[i] = tempArray[i];
                    }
                    Transform temp = Instantiate(original);
                    temp.SetParent(parent);
                    pool[pool.Length - 1] = temp;
                    temp.gameObject.SetActive(false);
                    index = pool.Length - 1;
                }

                return pool[index];
            }

            public void Init(Transform parent)
            {
                this.parent = parent;
                Transform temp;
                pool = new Transform[initMax];
                for (int i = 0; i < initMax; i++)
                {
                    temp = Instantiate(original);
                    temp.transform.SetParent(parent);
                    temp.gameObject.SetActive(false);
                    pool[i] = temp;
                }
            }
        }
    }
}
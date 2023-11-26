using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

namespace ZB.Msg
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private List<string> currentMessages;

        private static Message instance;
        private Reaction[] reactions;

        public static void Send(ID id, string message = "")
        {
            InstanceCheck();

            Reaction reaction = GetElement(id);
            if (reaction != null)
            {
                reaction.Invoke(message);
            }
        }
        public static UnityEvent GetSendedEvent(ID id, string message)
        {
            InstanceCheck();
            UnityEvent unityEvent = new UnityEvent();
            Reaction reaction = GetElement(id);

            unityEvent.AddListener(() => reaction.Invoke(message));
            return unityEvent;
        }
        public static UnityEvent GetSendedEvent_OrMakeElement(ID id, string message)
        {
            InstanceCheck();

            UnityEvent unityEvent = new UnityEvent();
            Reaction reaction = GetElement(id, false);

            if (reaction != null)
            {
                unityEvent.AddListener(() => reaction.Invoke(message));
                return unityEvent;
            }

            unityEvent.AddListener(() => reaction.Invoke(message));
            return unityEvent;
        }
        public static void AddEvent(ID id, UnityAction<string> action)
        {
            InstanceCheck();

            Reaction reaction = GetElement(id, false);

            instance.reactions[(int)id].AddListener(action);
        }
        public static void RemoveEvent(ID id, UnityAction<string> action)
        {
            InstanceCheck();

            Reaction reaction = GetElement(id);

            instance.reactions[(int)id].RemoveListener(action);
        }

        private static Reaction GetElement(ID id, bool logShow = true)
        {
            if ((int)id > instance.reactions.Length) 
            {
                if (logShow) 
                    Debug.LogError($"ZB.Message -> GetElement / id 입력오류 : {id}");
                return null;
            }
            return instance.reactions[(int)id];
        }
        private static void InstanceCheck()
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "ZB.Message";
                instance = obj.AddComponent<Message>();
                instance.Init();
            }
        }
        private void AddCurrentMessage(string id, string message)
        {
            if (currentMessages.Count >= 10)
                currentMessages.RemoveAt(0);
            currentMessages.Add($"id : {id} / message : {message}");
        }
        private void Init()
        {
            currentMessages = new List<string>();
            reactions = new Reaction[(int)ID.COUNT];
            Debug.Log((int)ID.COUNT);
            for (int i = 0; i < reactions.Length; i++)
            {
                string id2string = ((ID)i).ToString();
                reactions[i] = new Reaction();
                reactions[i].AddListener((string message) => AddCurrentMessage(id2string, message));
            }
            transform.SetSiblingIndex(0);
        }
    }

    public class Reaction
    {
        public StringUnityEvent UnityEvent { get => unityEvent; }
        public int Count { get => count; }

        private StringUnityEvent unityEvent;
        private int count;

        public void Invoke(string message)
        {
            unityEvent.Invoke(message);
        }

        public void AddListener(UnityAction<string> action)
        {
            unityEvent.AddListener(action);
            count++;
        }

        //유니티 이벤트에 더이상 액션이 없으면 false 반환
        public bool RemoveListener(UnityAction<string> action)
        {
            unityEvent.RemoveListener(action);
            count--;
            return count > 1;
        }

        public Reaction()
        {
            unityEvent = new StringUnityEvent();
            count = 0;
        }
    }
}
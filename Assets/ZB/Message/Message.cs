using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

namespace ZB.MessageSystem
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private List<InsertedReaction> insertedReactions;
        [SerializeField] private List<string> currentMessages;

        private static Message instance;
        Dictionary<string, Reaction> reactions;

        public static void Send(string id, string message = "")
        {
            InstanceCheck();

            Reaction reaction = GetElement(id);
            if (reaction != null)
            {
                reaction.Invoke(message);
            }
        }
        public static UnityEvent GetSendedEvent(string id, string message)
        {
            InstanceCheck();
            UnityEvent unityEvent = new UnityEvent();
            Reaction reaction = GetElement(id);

            unityEvent.AddListener(() => reaction.Invoke(message));
            return unityEvent;
        }
        public static UnityEvent GetSendedEvent_OrMakeElement(string id, string message)
        {
            InstanceCheck();

            UnityEvent unityEvent = new UnityEvent();
            Reaction reaction = GetElement(id, false);

            if (reaction != null)
            {
                unityEvent.AddListener(() => reaction.Invoke(message));
                return unityEvent;
            }

            if (!instance.reactions.ContainsKey(id))
            {
                instance.reactions.Add(id, reaction);
            }

            unityEvent.AddListener(() => reaction.Invoke(message));
            return unityEvent;
        }
        public static void AddEvent(string id, UnityAction<string> action)
        {
            InstanceCheck();

            Reaction reaction = GetElement(id, false);

            if (reaction == null)
            { 
                reaction = new Reaction();
                reaction.AddListener((string message) => instance.AddCurrentMessage(id, message));
                instance.reactions.Add(id, reaction);
                instance.insertedReactions.Add(new InsertedReaction(id));
            }

            instance.reactions[id].AddListener(action);
            instance.FindInsertedReaction(id).count++;
        }
        public static void RemoveEvent(string id, UnityAction<string> action)
        {
            InstanceCheck();

            Reaction reaction = GetElement(id);

            if (reaction != null)
            {
                instance.FindInsertedReaction(id).count--;
                if (!reaction.RemoveListener(action))
                {
                    //이벤트에 더이상 액션 없을경우, 메시지 딕셔너리에서 제거
                    instance.reactions.Remove(id);
                    instance.insertedReactions.Remove(instance.FindInsertedReaction(id));
                }
            }
        }

        private static Reaction GetElement(string id, bool logShow = true)
        {
            if (!instance.reactions.ContainsKey(id))
            {
                if (logShow) 
                    Debug.LogError($"ZB.Message -> GetElement / id 입력오류 : {id}");
                return null;
            }
            return instance.reactions[id];
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
        private InsertedReaction FindInsertedReaction(string name)
        {
            for (int i = 0; i < insertedReactions.Count; i++)
            {
                if (insertedReactions[i].name == name)
                {
                    return insertedReactions[i];
                }
            }
            return null;
        }
        private void Init()
        {
            insertedReactions = new List<InsertedReaction>();
            currentMessages = new List<string>();
            reactions = new Dictionary<string, Reaction>();
            transform.SetSiblingIndex(0);
        }

        [System.Serializable]
        public class InsertedReaction
        {
            public string name;
            public int count;

            public InsertedReaction(string name)
            {
                this.name = name;
            }
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
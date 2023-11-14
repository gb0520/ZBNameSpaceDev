using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    List<Observer> observers;

    public void Notify(object message)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(message);
        }
    }

    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

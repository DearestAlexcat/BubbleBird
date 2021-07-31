using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomEvent
{
    [SerializeField]
    string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    [SerializeField]
    UnityEvent unityEvents;
    public UnityEvent UnityEvents
    {
        get
        {
            return unityEvents;
        }
        set
        {
            UnityEvents = value;
        }
    }
}

public class AnimationEvent : MonoBehaviour
{
    [SerializeField]
    List<CustomEvent> customEvents;
    public List<CustomEvent> CustomEventsField
    {
        get
        {
            return customEvents;
        }
        set
        {
            customEvents = value;
        }
    }

    public void InvokeEventWithName(string name)
    {
        for (int i = 0; i < CustomEventsField.Count; i++)
        {
            if(CustomEventsField[i].Name == name)
            {
                CustomEventsField[i].UnityEvents.Invoke();
                return;
            }
        }
    }
}

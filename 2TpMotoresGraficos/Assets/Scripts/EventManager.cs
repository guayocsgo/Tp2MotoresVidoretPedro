using UnityEngine;
using UnityEngine.Events;
using System;


[Serializable]
public class Int2Event : UnityEvent<int, int> { }
public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        if (current == null) { current = this; } else if (current != this) { Destroy(this); }
    }

    public Int2Event updateBulletsEvent = new Int2Event();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyCrisisSubTypeEvent : UnityEvent<CrisisSubType>
{
}

public class BoxButtonTest : MonoBehaviour
{
    public MyCrisisSubTypeEvent m_MyEvent;
    public CrisisSubType crisisSubType;
    public KeyboardKey key;

    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new MyCrisisSubTypeEvent();

        m_MyEvent.AddListener(Ping);
    }

    void Update()
    {
        if (Input.GetKeyDown(key.ToString()) && m_MyEvent != null)
        {
            m_MyEvent.Invoke(crisisSubType);
        }
    }

    void Ping(CrisisSubType crisisSubType)
    {
        Debug.Log("Ping " + crisisSubType);
    }
}

public enum KeyboardKey
{
    t,
    y,
    u,
}

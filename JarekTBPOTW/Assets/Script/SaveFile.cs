using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class TimerData
{
    public string time;
}

[System.Serializable]
public class TimeList
{
    public TimerData[] list; 
}

public class SaveFile : MonoBehaviour
{
    public TimeList list;

    public void Save()
    {

    }
}

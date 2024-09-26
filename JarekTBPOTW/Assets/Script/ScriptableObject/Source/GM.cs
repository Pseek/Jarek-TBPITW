using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using Unity.VisualScripting;
using Unity.Collections;

[System.Serializable]
public class TimerList
{
    public List<float> list = new List<float>();

    public void SortList()
    {
        list = list.OrderBy(time => time).ToList();
    }
    public void AddTimeList(float time)
    {
        list.Add(time);
        SortList();
    }
}
[System.Serializable]
public class RunData
{
    public List<float> listTimerBALData = new List<float>(24);
}
[System.Serializable]
public class RunCurrent
{
    public List<float> listTimerBALCurrent = new List<float>(24);
}

[CreateAssetMenu(fileName = "Gamemanager", menuName =("Gm"))]
public class GM : ScriptableObject
{
    public RunData rD;
    public TimerList tM;
    public RunCurrent rC;
    public int nbrBAL_a_D;
    public bool isWin = false;
    public bool isPause = false;
    public bool isStop = false;
    public bool isCheckpoint = false;
    public float elapsedTime;

    string filePath = Application.streamingAssetsPath + "/ListTimerEnd.json";
    string filePath2 = Application.streamingAssetsPath + "/ListTimerBAL.json";

    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonUtility.ToJson(tM);
        string json2 = JsonUtility.ToJson(rD);

        if (!File.Exists(filePath) && !File.Exists(filePath2))
        {
            File.Create(filePath).Close();
            File.Create(filePath2).Close();
        }
        File.WriteAllText(filePath, json);
        File.WriteAllText(filePath2, json2);
    }

    [ContextMenu("Load")]

    public void Load()
    {
        string json = File.ReadAllText(filePath);
        string json2 = File.ReadAllText(filePath2);
        tM = JsonUtility.FromJson<TimerList>(json);
        rD = JsonUtility.FromJson<RunData>(json2);
    }
    
    public void RemoveBAL()
    {
        nbrBAL_a_D = 0;
        isWin = false;
        elapsedTime = 0;
        isStop = false;
    }
    
    public void AddBAL(int paper)
    {
        nbrBAL_a_D += paper;
        int i = nbrBAL_a_D - 1;
        rC.listTimerBALCurrent.RemoveAt(i);
        rC.listTimerBALCurrent.Insert(i, elapsedTime);
        if (rD.listTimerBALData[i] == 0)
        {
            rD.listTimerBALData.RemoveAt(i);
            rD.listTimerBALData.Insert(i, elapsedTime);
            Save();
        }
        if (rD.listTimerBALData[i] > elapsedTime)
        {
            rD.listTimerBALData.RemoveAt(i);
            rD.listTimerBALData.Insert(i, elapsedTime);
            Save();
        }
    }

    public void ChangesScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TimeScaleZero()
    {
        Time.timeScale = 0;
    }

    public void TimeScaleOne()
    {
        Time.timeScale = 1;
    }
}

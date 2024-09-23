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
    public List<float> listTimerBAL = new List<float>();
}

[CreateAssetMenu(fileName = "Gamemanager", menuName =("Gm"))]
public class GM : ScriptableObject
{
    private CompareTimeUI cTUI;
    public RunData rD;
    public TimerList tM;
    public int nbrBAL_a_D;
    public bool isWin = false;
    public bool isPause = false;
    public bool isStop = false;
    public bool isCheckpoint = false;
    public float elapsedTime;

    string filePath = Application.streamingAssetsPath + "/words.json";

    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonUtility.ToJson(tM);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        File.WriteAllText(filePath, json);
    }

    [ContextMenu("Load")]

    public void Load()
    {
        string json = File.ReadAllText(filePath);
        tM = JsonUtility.FromJson<TimerList>(json);
    }
    
    public void RemoveBAL()
    {
        nbrBAL_a_D = 0;
        isWin = false;
        elapsedTime = 0;
        isStop = false;
    }
    
    public void AddBAL (int paper)
    {
        nbrBAL_a_D += paper;
        int i = nbrBAL_a_D - 1;
        if (rD.listTimerBAL[i] == 0)
        {
            rD.listTimerBAL.RemoveAt(i);
            rD.listTimerBAL.Insert(i, elapsedTime);
        }
        else if (rD.listTimerBAL[i] > elapsedTime)
        {
            rD.listTimerBAL.RemoveAt(i);
            rD.listTimerBAL.Insert(i, elapsedTime);
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

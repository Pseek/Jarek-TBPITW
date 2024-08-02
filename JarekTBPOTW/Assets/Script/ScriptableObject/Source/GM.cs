using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

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

[CreateAssetMenu(fileName = "Gamemanager", menuName =("Gm"))]
public class GM : ScriptableObject
{
    public TimerList tM;
    public int nbrBAL_a_D;
    public bool isWin = false;
    public bool isPause = false;
    public bool isStop = false;
    public float elapsedTime;

    string filePath = Application.streamingAssetsPath + "/words.json";

    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonUtility.ToJson(tM);
        Debug.Log(json);

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
    }
    
    public void AddBAL (int paper)
    {
        nbrBAL_a_D += paper;
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

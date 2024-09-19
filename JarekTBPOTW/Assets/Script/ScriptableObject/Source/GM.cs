using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using Unity.VisualScripting;

[System.Serializable]
public class TimerList
{
    public List<float> list = new List<float>();

    /*public List<float> listTimerBal1 = new List<float>();
    public List<float> listTimerBal2 = new List<float>();
    public List<float> listTimerBal3 = new List<float>();
    public List<float> listTimerBal4 = new List<float>();
    public List<float> listTimerBal5 = new List<float>();
    public List<float> listTimerBal6 = new List<float>();
    public List<float> listTimerBal7 = new List<float>();
    public List<float> listTimerBal8 = new List<float>();
    public List<float> listTimerBal9 = new List<float>();
    public List<float> listTimerBal10 = new List<float>();
    public List<float> listTimerBal11 = new List<float>();
    public List<float> listTimerBal12 = new List<float>();
    public List<float> listTimerBal13 = new List<float>();
    public List<float> listTimerBal14 = new List<float>();
    public List<float> listTimerBal15 = new List<float>();
    public List<float> listTimerBal16 = new List<float>();
    public List<float> listTimerBal17 = new List<float>();
    public List<float> listTimerBal18 = new List<float>();
    public List<float> listTimerBal19 = new List<float>();
    public List<float> listTimerBal20 = new List<float>();
    public List<float> listTimerBal21 = new List<float>();
    public List<float> listTimerBal22 = new List<float>();
    public List<float> listTimerBal23 = new List<float>();
    public List<float> listTimerBal24 = new List<float>();*/

    public List<List<float>> listTimerBAL = new List<List<float>>();
    public void SortList()
    {
        list = list.OrderBy(time => time).ToList();
    }
    public void AddTimeList(float time)
    {
        list.Add(time);
        SortList();
    }

    public void AddList(List<float> timerListAddBal)
    {
        listTimerBAL.Add(timerListAddBal);
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
        for (int i = 0; i < nbrBAL_a_D; i++)
        {
            //tM.AddList();
            Debug.Log("j'ajoute une liste");
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

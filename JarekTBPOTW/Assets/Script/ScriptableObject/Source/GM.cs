using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Gamemanager", menuName =("Gm"))]

public class GM : ScriptableObject
{
    public int nbrBAL_a_D;
    public bool isWin = false;
    
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

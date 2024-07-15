using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Gamemanager", menuName =("Gm"))]

public class GM : ScriptableObject
{
    public int nbrBAL_a_D;

    public void AddBAL (int paper)
    {
        nbrBAL_a_D += paper;
    }

    public void CheckWin()
    {
        SceneManager.LoadScene("");
    }

    public void ChangesScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

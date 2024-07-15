using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUIManager : MonoBehaviour
{
    public string sceneUI;
    void Start()
    {
        SceneManager.LoadScene(sceneUI, LoadSceneMode.Additive);
    }
}

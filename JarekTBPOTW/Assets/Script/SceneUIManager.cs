using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneUIManager : MonoBehaviour
{
    public string sceneUI;
    
    void Start()
    {
        SceneManager.LoadScene(sceneUI, LoadSceneMode.Additive);
    }
}

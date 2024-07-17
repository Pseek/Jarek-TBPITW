using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputTime;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent <string,int> sumbitTime;

    public void SubmitTime ()
    {
        sumbitTime.Invoke(inputName.text, int.Parse(inputTime.text));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddBAL_UI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextPause;
    public TextMeshProUGUI scoreTextCompare;
    public GM gM;

    public void Update()
    {
        scoreText.text = gM.nbrBAL_a_D.ToString() + "/" + gM.nbrBALEndUI.ToString();
        scoreTextPause.text = gM.nbrBAL_a_D.ToString() + "/" + gM.nbrBALEndUI.ToString();
        scoreTextCompare.text = gM.nbrBAL_a_D.ToString() + "/" + gM.nbrBALEndUI.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddBAL_UI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GM gM;

    public void Update()
    {
        scoreText.text = gM.nbrBAL_a_D.ToString() + "/20";
    }
}

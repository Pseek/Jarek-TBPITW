using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PenaltyTimerUI : MonoBehaviour
{
    [SerializeField] private GM gM;
    [SerializeField] private TextMeshProUGUI penaltyTimerText;
    [SerializeField] private GameObject penaltyTimer;
    [SerializeField] private Image imageTrap;
    [SerializeField] private Sprite dogSprite;
    [SerializeField] private Sprite dumpsterSprite;
    [SerializeField] private RectTransform imageTrapRect;
    private float chrono;
    [SerializeField] private float timerForClose;

    public void Update()
    {
        chrono += Time.deltaTime;
        if (gM.takePenalty)
        {
            chrono = 0f;
            penaltyTimer.SetActive(true);
            penaltyTimerText.text = $"Tu as perdu {gM.penaltyTimerForUI} seccondes t'aurais pu faire gaffe MERDE !!! ";
            if (gM.penaltyTimerForUI == gM.penaltyTimeDumpster)
            {
                imageTrap.sprite = dumpsterSprite;
                imageTrapRect.sizeDelta = new Vector2(50f,50f);
            }
            else if (gM.penaltyTimerForUI == gM.penaltyTimeDog)
            {
                imageTrap.sprite = dogSprite;
                imageTrapRect.sizeDelta = new Vector2(150f, 100f);
            }
            gM.takePenalty = false;
        }
        if (chrono > timerForClose)
        {
            chrono = 0f;
            penaltyTimer.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEndGameTExt : MonoBehaviour
{
    public GM gM;
    public GameObject EndGameText;
    public void Update()
    {
        if (gM.isWin == true && gM.isStop == false)
        {
            StartCoroutine(ShowReturnPosteText());
        }
    }
    IEnumerator ShowReturnPosteText()
    {
        EndGameText.SetActive(true);
        yield return new WaitForSeconds(8f);
        EndGameText.SetActive(false);
        gM.isStop = true;
        StopCoroutine(ShowReturnPosteText());
    }
}

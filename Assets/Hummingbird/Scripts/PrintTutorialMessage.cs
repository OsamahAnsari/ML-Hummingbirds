using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintTutorialMessage : MonoBehaviour
{
    public string message;
    public TextMeshProUGUI banner;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Got here");
            StartCoroutine(ShowMessage());
        }
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(0.25f);
        banner.text = message;
        yield return new WaitForSeconds(3f);
        banner.text = "";
    }
}

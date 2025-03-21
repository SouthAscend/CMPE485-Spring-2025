using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private String message;
    static Text text;

    private void Start()
    {
        GameObject countObj = GameObject.Find($"Canvas/MessageText");
        text = countObj.GetComponent<Text>();
    }

    public void DeleteMessage()
    {
        text.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = message;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeleteMessage();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Message
{
    static Text text;
    public static void UpdateMessage(string message)
    {
        if (!text)
        {
            GameObject countObj = GameObject.Find($"Canvas/MessageText");
            text = countObj.GetComponent<Text>();
        }

        text.text = message;
    }

    public static void DeleteMessage()
    {
        if (!text)
        {
            GameObject countObj = GameObject.Find($"Canvas/MessageText");
            text = countObj.GetComponent<Text>();
        }

        text.text = "";
    }
}

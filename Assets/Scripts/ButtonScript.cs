using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    public delegate void ButtonClickHandler();
    public ButtonClickHandler buttonClickHandler;

    void OnMouseDown()
    {
        buttonClickHandler.Invoke();
    }
}

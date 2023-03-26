using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RateButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Yandex.ActivateRateButtonEvent += ActivateRateButtonEventHandler;
    }

    private void OnDisable()
    {
        Yandex.ActivateRateButtonEvent -= ActivateRateButtonEventHandler;
    }

    private void ActivateRateButtonEventHandler(bool value)
    {
        button.interactable = value;
    }
}

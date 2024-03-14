using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtension : MonoBehaviour
{
    [SerializeField] private SeType _mouseOver;
    [SerializeField] private SeType _clickButton;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ClickButton);
    }

    public void MouseOverSe()
    {
        AudioManager.Instance.PlaySe(_mouseOver);
        print("MouseOver");
    }
    private void ClickButton()
    {
        AudioManager.Instance.PlaySe(_clickButton);
    }

}

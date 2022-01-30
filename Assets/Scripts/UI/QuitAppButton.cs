using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitAppButton : MonoBehaviour
{
    
    [SerializeField] private Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

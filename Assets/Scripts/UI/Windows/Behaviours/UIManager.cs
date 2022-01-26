using System.Collections;
using System.Collections.Generic;
using Lever.UI.Windows.Interfaces;
using UI;
using UnityEngine;

public class UIManager : UIHelper, IUIManager
{
    [SerializeField] private LoginBehaviour loginBehaviour;


    public void OpenLogin()
    {
        loginBehaviour.Show();
    }

    public void OpenRoomBrowser(string playerName)
    {
        Debug.Log(playerName);
    }

    public void OpenLobby()
    {
        throw new System.NotImplementedException();
    }

    public void OpenLevelLoadScreen()
    {
        throw new System.NotImplementedException();
    }
}

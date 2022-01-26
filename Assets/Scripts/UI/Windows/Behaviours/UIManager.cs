using System.Collections;
using System.Collections.Generic;
using Lever.UI.Windows.Behaviours;
using Lever.UI.Windows.Interfaces;
using UI;
using UnityEngine;
using Zenject;

public class UIManager : UIHelper, IUIManager
{
    [SerializeField] private LoginBehaviour loginBehaviour;
    [SerializeField] private RoomBrowserBehaviour roomBrowserBehaviour;
    [SerializeField] private LobbyBehaviour lobbyBehaviour;

    private ILoadingScreen loadingScreen;

    [Inject]
    private void Construct(LoadingScreenBehaviour loadingScreenBehaviour)
    {
        loadingScreen = loadingScreenBehaviour;
    }
    
    public void OpenLogin()
    {
        loginBehaviour.Show();
    }

    public void OpenRoomBrowser(string playerName)
    {
        Debug.Log(playerName);
        roomBrowserBehaviour.Show(playerName);
    }

    public void OpenLobby(string roomName)
    {
        Debug.Log(roomName);
        OpenLoadingScreen(true);
        lobbyBehaviour.Show(roomName);
    }

    public void OpenLevelLoadScreen()
    {
        throw new System.NotImplementedException();
    }

    public void OpenLoadingScreen(bool shoudOpen)
    {
        if (shoudOpen)
        {
            loadingScreen.Show();
            return;
        }
        loadingScreen.Hide();
    }
}

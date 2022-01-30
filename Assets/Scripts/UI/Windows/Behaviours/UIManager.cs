using System;
using System.Collections;
using System.Collections.Generic;
using Lever.Networking;
using Lever.UI.Animations;
using Lever.UI.Windows.Behaviours;
using Lever.UI.Windows.Interfaces;
using Photon.Realtime;
using UI;
using UnityEngine;
using Zenject;

public class UIManager : UIHelper, IUIManager
{
    [SerializeField] private LoginBehaviour loginBehaviour;
    [SerializeField] private RoomBrowserBehaviour roomBrowserBehaviour;
    [SerializeField] private LobbyBehaviour lobbyBehaviour;
    [SerializeField] private RaisingMoonAnimation raisingMoonAnimation;

    private ILoadingScreen loadingScreen;
    private ILobbyNetworking lobbyNetworking;

    private string thisPlayerName;

    [Inject]
    private void Construct(LoadingScreenBehaviour loadingScreenBehaviour)
    {
        loadingScreen = loadingScreenBehaviour;
    }
    
    [Inject]
    private void Construct(ILobbyNetworking lobbyNetworking)
    {
        this.lobbyNetworking = lobbyNetworking;
    }

    public void OpenLogin()
    {
        loginBehaviour.Show();
    }

    public void OpenRoomBrowser(string playerName)
    {
        thisPlayerName = playerName;
        Debug.Log(playerName);
        lobbyNetworking.NickName = playerName;
        roomBrowserBehaviour.LoadRoomList(lobbyNetworking.Rooms);
        roomBrowserBehaviour.Show(playerName);

        lobbyNetworking.OnRoomListChanged += roomBrowserBehaviour.LoadRoomList;
    }

    public void UnsubscribeOnCloseRoomBrowser()
    {
        lobbyNetworking.OnRoomListChanged -= roomBrowserBehaviour.LoadRoomList;
    }

    public void CreateRoom(string name, int maxPlayers = 4)
    {

        lobbyNetworking.CreateRoom(name, (byte)maxPlayers);
        CreateLobby(name, maxPlayers);
    }

    

    public void QuitLobby()
    {
        if (thisPlayerName == String.Empty) thisPlayerName = "Leaver";
        lobbyBehaviour.Hide();
        OpenRoomBrowser(thisPlayerName);
        lobbyNetworking.LeaveRoom();
    }

    public void StartGame()
    {
        lobbyNetworking.StartGame();
        raisingMoonAnimation.PlayRaisingMoon();
        lobbyBehaviour.Hide();
    }

    public void OpenLobby(RoomInfo roomInfo)
    {
        lobbyBehaviour.Show(roomInfo);
        
        lobbyBehaviour.UpdatePlayersList(lobbyNetworking.PlayersInRoom);
        lobbyNetworking.OnPlayerListChanged += lobbyBehaviour.UpdatePlayersList;
        lobbyNetworking.JoinRoom(roomInfo.Name);
    }
    
    public void CreateLobby(string newRoomName, int maxPlayerCount)
    {
        lobbyBehaviour.Show(newRoomName, maxPlayerCount);
        
        lobbyBehaviour.UpdatePlayersList(lobbyNetworking.PlayersInRoom);
        lobbyNetworking.OnPlayerListChanged += lobbyBehaviour.UpdatePlayersList;
    }

    public void UnsubscribeOnCloseLobby()
    {
        lobbyNetworking.OnPlayerListChanged -= lobbyBehaviour.UpdatePlayersList;
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

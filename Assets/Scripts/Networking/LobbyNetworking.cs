using System;
using System.Collections.Generic;
using Lever.Constants;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Lever.Networking
{
    public class LobbyNetworking : MonoBehaviourPunCallbacks, ILobbyNetworking
    {
        private List<RoomInfo> rooms;

        public event Action<List<RoomInfo>> OnRoomListChanged;
        public event Action<Player[]> OnPlayerListChanged;
        public event Action OnJoinedToRoom;
        public event Action<Player> OnMasterChanged;

        public List<RoomInfo> Rooms
        {
            get => rooms;
            set
            {
                rooms = value;
                OnRoomListChanged?.Invoke(rooms);
            }
        }

        public Player[] PlayersInRoom => PhotonNetwork.PlayerList;
        public int Ping => PhotonNetwork.GetPing();
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        public string NickName
        {
            get => PhotonNetwork.NickName;
            set => PhotonNetwork.NickName = value;
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom(string name, byte maxPlayers = 4)
        {
            PhotonNetwork.CreateRoom(name, new RoomOptions {MaxPlayers = maxPlayers, IsVisible = true, IsOpen = true}, TypedLobby.Default);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(SceneNames.Multiplayer);
        }

        private void JoinLobby()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        private void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        // NOT FOR USING

        #region NotForUsing

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            JoinLobby();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined the room");
            OnJoinedToRoom?.Invoke();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            OnMasterChanged?.Invoke(newMasterClient);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerListChanged?.Invoke(PhotonNetwork.PlayerList);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerListChanged?.Invoke(PhotonNetwork.PlayerList);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"{returnCode}: {message}");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Rooms = roomList;
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Rooms count:" + PhotonNetwork.CountOfRooms);
        }

        #endregion
    }
}
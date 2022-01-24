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
    public class LobbyNetworking : MonoBehaviourPunCallbacks
    {
        private List<RoomInfo> rooms;

        public event Action<List<RoomInfo>> OnRoomListChanged;

        public List<RoomInfo> Rooms
        {
            get => rooms;
            private set
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

            JoinLobby();
        }

        public void CreateRoom(string name, byte maxPlayers = 4)
        {
            PhotonNetwork.CreateRoom(name, new RoomOptions {MaxPlayers = maxPlayers, IsOpen = true, EmptyRoomTtl = 0});
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(SceneNames.Multiplayer);
        }

        private void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
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
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined the room");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"{returnCode}: {message}");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Rooms = roomList;
        }

        #endregion
    }
}
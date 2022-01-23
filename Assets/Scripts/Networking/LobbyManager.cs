using System;
using Lever.Constants;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Lever.Networking
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
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

        private void Start()
        {
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
        }

        public void CreateRoom(string name, byte maxPlayers = 4)
        {
            PhotonNetwork.CreateRoom(name, new RoomOptions { MaxPlayers = maxPlayers , IsOpen = false, EmptyRoomTtl = 0});
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined the room");
            PhotonNetwork.LoadLevel(SceneNames.Multiplayer);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"{returnCode}: {message}");
        }
    }
}
using System;
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
        [SerializeField] private TMP_Text LogText;
        
        private void Start()
        {
            PhotonNetwork.NickName = "Player " + new Random().Next(1000, 5555);
            Log("Player's name is set to " + PhotonNetwork.NickName);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Log("Connected to Master");
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom("test", new RoomOptions { MaxPlayers = 4});
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom("test");
        }

        public override void OnJoinedRoom()
        {
            Log("Joined the room");
            PhotonNetwork.LoadLevel("NetworkingGame");
        }

        private void Log(string message)
        {
            Debug.Log(message);
            LogText.text += "\n" + message;
        }
    }
}
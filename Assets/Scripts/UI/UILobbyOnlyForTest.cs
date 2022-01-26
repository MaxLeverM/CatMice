using System;
using System.Collections;
using Lever.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = System.Random;

namespace Lever.UI
{
    public class UILobbyOnlyForTest : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nickName;
        [SerializeField] private TMP_InputField roomName;
        [SerializeField] private Button createRoom;
        [SerializeField] private Button joinRoom;
        [SerializeField] private Button startGame;
        [SerializeField] private ScrollRect listRooms;
        [SerializeField] private ScrollRect listPlayers;
        [SerializeField] private TMP_Text ping;
        [SerializeField] private TMP_Text isMaster;

        private ILobbyNetworking lobbyNetworking;
        private string selectedRoom = "test";

        [Inject]
        private void Construct(ILobbyNetworking lobbyNetworking)
        {
            this.lobbyNetworking = lobbyNetworking;
        }

        private void Awake()
        {
            nickName.text = $"Player{new Random().Next(0, 9999)}";
            roomName.text = selectedRoom;
            nickName.onEndEdit.AddListener((x) => lobbyNetworking.NickName = x);
            createRoom.onClick.AddListener(() => lobbyNetworking.CreateRoom(roomName.text));
            joinRoom.onClick.AddListener(() => lobbyNetworking.JoinRoom(selectedRoom));
            startGame.onClick.AddListener(() => lobbyNetworking.StartGame());
            StartCoroutine(PingTest());

            lobbyNetworking.OnJoinedToRoom += UpdateIsMaster;
            lobbyNetworking.OnMasterChanged += (x) => UpdateIsMaster();
        }

        private void UpdateIsMaster()
        {
            isMaster.text = lobbyNetworking.IsMasterClient ? "Is Master" : "Is NOT Master";
        }

        private IEnumerator PingTest()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                ping.text = lobbyNetworking.Ping.ToString();
            }
        }
    }
}
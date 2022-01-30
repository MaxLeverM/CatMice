﻿using Lever.UI.Windows.Interfaces;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Lever.UI.Windows.Behaviours
{
    public class LobbyBehaviour : PopUpWindow
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI currentPlayerCountField;
        [SerializeField] private string roomPostName = " room";
        [SerializeField] private PlayerInListBehaviour playerInListPrefab;
        [SerializeField] private PlayerInListBehaviour[] spawnedPlayerList = new PlayerInListBehaviour[0];
        [SerializeField] private Transform contentHolder;
        [SerializeField] private Button startGameButton;

        private string currentRoomName;

        private int maxPlayerInThisLobbyValue;

        private IUIManager uiManager;
        
        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
            startGameButton.onClick.AddListener(StartGame);
        }
        
        /// <summary>
        /// Show room on connect
        /// </summary>
        /// <param name="roomInfo"></param>
        public void Show(RoomInfo roomInfo)
        {
            currentRoomName = roomInfo.Name;
            titleText.text = roomInfo.Name + roomPostName;
            maxPlayerInThisLobbyValue = roomInfo.MaxPlayers;
            
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.MoveToTargetPosition();
        }

        private void StartGame()
        {
            uiManager.StartGame();
        }

        /// <summary>
        /// Show lobby if createRoom
        /// </summary>
        /// <param name="roomName"></param>
        public void Show(string roomName, int maxPlayerCount)
        {
            currentRoomName = roomName;
            titleText.text = roomName + roomPostName;
            maxPlayerInThisLobbyValue = maxPlayerCount;
            
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.MoveToTargetPosition();
        }
        

        public void Hide()
        {
            Animations.MoveToEndPosition(() => ToggleVisibleCanvasGroup(CanvasGroup, false));
            uiManager.UnsubscribeOnCloseLobby(); 
        }

        public void UpdatePlayersList(Player[] playersArray)
        {
            UpdatePlayerCount(playersArray.Length);
            
            foreach (var spawnedCell in spawnedPlayerList)
                Destroy(spawnedCell.gameObject);
            
            foreach (var player in playersArray)
            {
                var newCell = Instantiate(playerInListPrefab, contentHolder);
                newCell.LoadData(player);
            }
        }

        private void UpdatePlayerCount(int currentCount)
        {
            currentPlayerCountField.text = $"{currentCount}/{maxPlayerInThisLobbyValue}";
        }
    }
}
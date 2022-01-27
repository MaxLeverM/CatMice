﻿using Lever.UI.Windows.Interfaces;
using Photon.Realtime;
using TMPro;
using UnityEngine;
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
        
        private string currentRoomName;

        private IUIManager uiManager;
        
        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }
        
        
        public void Show(RoomInfo roomInfo)
        {
            currentRoomName = roomInfo.Name;
            titleText.text = roomInfo.Name + roomPostName;
            
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.MoveToTargetPosition();
        }

        public void Show(string roomName)
        {
            currentRoomName = roomName;
            titleText.text = roomName + roomPostName;
            
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
            foreach (var spawnedCell in spawnedPlayerList)
                Destroy(spawnedCell.gameObject);
            
            foreach (var player in playersArray)
            {
                var newCell = Instantiate(playerInListPrefab, contentHolder);
                newCell.LoadData(player);
            }
        }
    }
}
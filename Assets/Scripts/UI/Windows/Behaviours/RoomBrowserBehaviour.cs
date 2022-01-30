using System.Collections.Generic;
using Lever.UI.Windows.Interfaces;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace Lever.UI.Windows.Behaviours
{
    public class RoomBrowserBehaviour : PopUpWindow
    {
        [SerializeField] private TextMeshProUGUI roomTitle;
        [SerializeField] private string titleEndString = " select room";
        [SerializeField] private int maxPLayerCount = 8;
        private int currentPlayerCount = 4;


        [Header("Rooms list")]
        [SerializeField] private Transform listOfRoomContent;
        [SerializeField] private RoomButtonBehaviour roomButtonPrefab;
        private RoomButtonBehaviour[] loadedRoomsArray = new RoomButtonBehaviour[0];

        [Header("Create room")]
        [SerializeField] private TMP_InputField newServerNameInputField;
        [SerializeField] private TextMeshProUGUI currentPlayerCountText;
        [SerializeField] private Button plusPlayerCount;
        [SerializeField] private Button minusPLayerCount;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private int minRoomNameLeght = 4;
        
        private IUIManager uiManager;
        
        
        public int CurrentPlayerCount
        {
            get => currentPlayerCount;
            set
            {
                currentPlayerCount = value;
                currentPlayerCountText.text = value.ToString();
            }
        }


        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public void Show(string currentPlayerNick)
        {
            roomTitle.text = currentPlayerNick + titleEndString;
            
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.MoveToTargetPosition();
            
            plusPlayerCount.onClick.AddListener(() => AddPlayer(1));
            minusPLayerCount.onClick.AddListener(() => AddPlayer(-1));
            
            newServerNameInputField.onValueChanged.AddListener(
                (value) => CheckNickLeght(value, minRoomNameLeght, createRoomButton));
            createRoomButton.onClick.AddListener(CreateRoomButton);
            
            
        }

        public void Hide()
        {
            plusPlayerCount.onClick.RemoveListener(() => AddPlayer(1));
            minusPLayerCount.onClick.RemoveListener(() => AddPlayer(-1));
            Animations.MoveToEndPosition(() => ToggleVisibleCanvasGroup(CanvasGroup, false));
            
            newServerNameInputField.onValueChanged.RemoveListener(
                (value) => CheckNickLeght(value, minRoomNameLeght, createRoomButton));
            
            createRoomButton.onClick.RemoveListener(CreateRoomButton);
            uiManager.UnsubscribeOnCloseRoomBrowser();
        }


        public void LoadRoomList(List<RoomInfo> loadedRoomsNames) // я хз что получаем
        {
            foreach (Transform room in listOfRoomContent)
                Destroy(room.gameObject);

            if (loadedRoomsNames == null)
                return;
            
            foreach (var roomInfo in loadedRoomsNames)
            {
                if (roomInfo.MaxPlayers == 0) continue;
                var newRoomButton = Instantiate(roomButtonPrefab, listOfRoomContent);
                newRoomButton.LoadData(roomInfo, roomInfo.MaxPlayers, this);
            }
        }

        public void GoToRoom(RoomInfo roomInfo)
        {
            Hide();
            
            uiManager.OpenLobby(roomInfo);
        }

        private void AddPlayer(int additionalValue)
        {
            //Check max
            if (currentPlayerCount + additionalValue >= maxPLayerCount)
            {
                plusPlayerCount.interactable = false;
                
                if (currentPlayerCount + additionalValue == maxPLayerCount)
                    CurrentPlayerCount += additionalValue;
                
                return;
            }
            plusPlayerCount.interactable = true;
            
            //Check min
            if (currentPlayerCount + additionalValue <= 2)
            {
                minusPLayerCount.interactable = false;

                if (currentPlayerCount + additionalValue == 2)
                    CurrentPlayerCount += additionalValue;
                return;
            }
            minusPLayerCount.interactable = true;

            CurrentPlayerCount += additionalValue;
        }

        private void CreateRoomButton()
        {
            uiManager.CreateRoom(newServerNameInputField.text, currentPlayerCount);
            Hide();
        }
    }
}
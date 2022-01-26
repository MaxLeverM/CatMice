using Lever.UI.Windows.Interfaces;
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
        private int currentPlayerCount = 2;

        [Header("Rooms list")]
        [SerializeField] private Transform listOfRoomContent;
        [SerializeField] private RoomButtonBehaviour roomButtonPrefab;
        private RoomButtonBehaviour[] loadedRoomsArray;

        [Header("Create room")]
        [SerializeField] private TMP_InputField newServerNameInputField;
        [SerializeField] private Button plusPlayerCount;
        [SerializeField] private Button minusPLayerCount;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private int minRoomNameLeght = 4;
        
        private IUIManager uiManager;

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
            
            GetRoomList();
        }

        public void Hide()
        {
            plusPlayerCount.onClick.RemoveListener(() => AddPlayer(1));
            minusPLayerCount.onClick.RemoveListener(() => AddPlayer(-1));
            Animations.MoveToEndPosition(() => ToggleVisibleCanvasGroup(CanvasGroup, false));
            
            newServerNameInputField.onValueChanged.RemoveListener(
                (value) => CheckNickLeght(value, minRoomNameLeght, createRoomButton));
        }

        public void GetRoomList()
        {
            //Чтото чтоб отправить запрос и получить его в LoadRoomList();
        }

        public void LoadRoomList(string[] loadedRoomsNames) // я хз что получаем
        {
            foreach (var room in loadedRoomsArray)
                Destroy(room.gameObject);

            foreach (var roomName in loadedRoomsNames)
            {
                var newRoomButton = Instantiate(roomButtonPrefab, listOfRoomContent);
                newRoomButton.LoadData(roomName, 8, this);
            }
        }

        public void GoToRoom(string roomName)
        {
            Hide();
            
            uiManager.OpenLobby(roomName);
        }

        private void AddPlayer(int additionalValue)
        {
            //Check max
            if (currentPlayerCount + additionalValue >= maxPLayerCount)
            {
                plusPlayerCount.interactable = false;
                
                if (currentPlayerCount + additionalValue == maxPLayerCount)
                    currentPlayerCount += additionalValue;
                
                return;
            }
            plusPlayerCount.interactable = true;
            
            //Check min
            if (currentPlayerCount + additionalValue <= 2)
            {
                minusPLayerCount.interactable = false;

                if (currentPlayerCount + additionalValue == 2)
                    currentPlayerCount += additionalValue;
                return;
            }
            minusPLayerCount.interactable = true;
        }
    }
}
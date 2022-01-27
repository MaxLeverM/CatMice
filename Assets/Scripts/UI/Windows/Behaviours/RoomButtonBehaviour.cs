using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Lever.UI.Windows.Behaviours
{
    [RequireComponent(typeof(Button))]
    public class RoomButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomNameField;
        [SerializeField] private TextMeshProUGUI playerCountField;
        [SerializeField] private Button thisButton;

        private RoomBrowserBehaviour roomBrowserController;
        private string roomName = "noName";
        private int currentplayerCount;
        private int maxPlayerCount = 2;

        private RoomInfo thisRoom;

        
        
        private void OnEnable()
        {
            thisButton.onClick.AddListener(OpenThisRoom);
        }

        private void OnDisable()
        {
            thisButton.onClick.RemoveListener(OpenThisRoom);
        }


        public void LoadData(RoomInfo roomInfo, int maxPlayerCount, RoomBrowserBehaviour roomBrowserBehaviour)
        {
            thisRoom = roomInfo;
            this.maxPlayerCount = maxPlayerCount;
            roomBrowserController = roomBrowserBehaviour;

            roomNameField.text = roomInfo.Name;
            playerCountField.text = $"0/{maxPlayerCount}";
        }

        public void UpdateCurrentPLayerCount(int curPLayerCount)
        {
            currentplayerCount = curPLayerCount;
            playerCountField.text = $"{currentplayerCount}/{maxPlayerCount}";

            if (curPLayerCount >= maxPlayerCount)
            {
                thisButton.interactable = false;
                return;
            }

            thisButton.interactable = true;
        }

        public void OpenThisRoom()
        {
            Debug.Log("Open room}");
            roomBrowserController.GoToRoom(thisRoom);
            
        }

    }
}
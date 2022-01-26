using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Lever.UI.Windows.Behaviours
{
    public class RoomBrowserBehaviour : PopUpWindow
    {
        [SerializeField] private TextMeshProUGUI roomTitle;
        [SerializeField] private string titleEndString = " select room";
        [SerializeField] private int maxPLayerCount = 8;
        private int currentPlayerCount = 2;

        [SerializeField] private Transform listOfRoomContent;

        [SerializeField] private Button plusPlayerCount;
        [SerializeField] private Button minusPLayerCount;
        


        public void Show(string currentPlayerNick)
        {
            roomTitle.text = currentPlayerNick + titleEndString;
            
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.MoveToTargetPosition();
            
            plusPlayerCount.onClick.AddListener(() => AddPlayer(1));
            minusPLayerCount.onClick.AddListener(() => AddPlayer(-1));
        }

        public void Hide()
        {
            plusPlayerCount.onClick.RemoveListener(() => AddPlayer(1));
            minusPLayerCount.onClick.RemoveListener(() => AddPlayer(-1));
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
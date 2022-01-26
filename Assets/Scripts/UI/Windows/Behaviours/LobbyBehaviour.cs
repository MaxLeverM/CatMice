using TMPro;
using UnityEngine;

namespace Lever.UI.Windows.Behaviours
{
    public class LobbyBehaviour : PopUpWindow
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private string roomPostName = " room";

        private string currentRoomName;
            
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
        }
    }
}
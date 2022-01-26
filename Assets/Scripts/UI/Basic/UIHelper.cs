using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHelper : MonoBehaviour
    {
        private const int VisibleAlpha = 1;
        private const int InvisibleAlpha = 0;

        public void ToggleVisibleCanvasGroup(CanvasGroup canvasGroup, bool boolValue)
        {
            canvasGroup.alpha = boolValue ? VisibleAlpha : InvisibleAlpha;
            canvasGroup.interactable = boolValue;
            canvasGroup.blocksRaycasts = boolValue;
        }
        
        public void CheckNickLeght(string enteredNick, int minCharField, Button targetButton)
        {
            if (enteredNick.Length >= minCharField)
            {
                targetButton.interactable = true;
                return;
            }
            targetButton.interactable = false;
        }
    }
}

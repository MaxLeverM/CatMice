using UnityEngine;

namespace UI
{
    public class UIHelper : MonoBehaviour
    {
        private const int VisibleAlpha = 1;
        private const int InvisibleAlpha = 0;

        private void ToggleVisibleCanvasGroup(CanvasGroup canvasGroup, bool boolValue)
        {
            canvasGroup.alpha = boolValue ? VisibleAlpha : InvisibleAlpha;
            canvasGroup.interactable = boolValue;
            canvasGroup.blocksRaycasts = boolValue;
        }
    }
}

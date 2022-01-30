using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHelper : MonoBehaviour
    {
        private const int VisibleAlpha = 1;
        private const int InvisibleAlpha = 0;

        private bool smoothTooggleBool;

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

        public void SmoothToggleVisibleCanvasGroup(CanvasGroup canvasGroup, float duration, bool boolValue)
        {
            canvasGroup.interactable = boolValue;
            canvasGroup.blocksRaycasts = boolValue;
            
            smoothTooggleBool = true;
            StartCoroutine(SmoothToggling(canvasGroup, duration, boolValue));
            StartCoroutine(DisableLoop(duration));
        }

        public void SmoothShadeColorAlpha(Image targetImage,Color baseColor, float duration, bool boolValue)
        {
            baseColor.a = boolValue ? 0 : 1;
            targetImage.color = baseColor;
            StartCoroutine(SmoothTogglingAlpha(targetImage, duration, boolValue));
            StartCoroutine(DisableLoop(duration));
        }

        private IEnumerator DisableLoop(float duration)
        {
            yield return new WaitForSeconds(duration);
            smoothTooggleBool = false;
        }

        private IEnumerator SmoothToggling(CanvasGroup canvasGroup, float duration, bool boolValue)
        {
            float startAlphaValue = boolValue ? 0 : 1;
            float endAlphaValue = boolValue ? 1 : 0;

            float lastFrameValue = startAlphaValue;

            var time = Time.time;
            
            while (smoothTooggleBool)
            {
                var t = (Time.time - time) / duration;
                
                canvasGroup.alpha = Mathf.Lerp(lastFrameValue, endAlphaValue, t);
                yield return null;
            }
        }
        
        private IEnumerator SmoothTogglingAlpha(Image image, float duration, bool boolValue)
        {
            float startAlphaValue = boolValue ? 0 : 1;
            float endAlphaValue = boolValue ? 1 : 0;

            float lastFrameValue = startAlphaValue;

            var time = Time.time;
            
            while (smoothTooggleBool)
            {
                var t = (Time.time - time) / duration;
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(lastFrameValue, endAlphaValue, t));
                yield return null;
            }
        }
    }
}

using System.Collections;
using Lever.UI.Windows.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lever.UI.Windows.Behaviours
{
    public class LoadingScreenBehaviour : PopUpWindow, ILoadingScreen
    {
        [SerializeField] private TextMeshProUGUI titleSting;
        [SerializeField] private RawImage image;
        [SerializeField] private string loadingText = "Loading";
        [SerializeField] private string[] loadingDots = new[] { ".", "..", "..." };
        [SerializeField] private float updateTime = 0.5f;
        [SerializeField] private float rotateSpeed = 1f;

        private bool screenActive;
        
        [ContextMenu("Show")]
        public void Show()
        {
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            Animations.ScaleToTargetSize();
            screenActive = true;
            StartCoroutine(RotateIconLoop());
            StartCoroutine(LoadingLoopActions());
        }

        public void Hide()
        {
            Animations.ScaleToEndSize(() => ToggleVisibleCanvasGroup(CanvasGroup, false));
            screenActive = false;
        }

        private IEnumerator RotateIconLoop()
        {
            while (screenActive)
            {
                yield return null;
                image.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            }
        }

        private IEnumerator LoadingLoopActions()
        {
            int counter = 0;
            
            while (screenActive)
            {
                counter = counter >= loadingDots.Length ? 0 : counter;
                
                titleSting.text = loadingText + loadingDots[counter];
                
                counter++;
                yield return new WaitForSeconds(updateTime);
            }
        }
    }
}
using System;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Lever.UI.Animations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RaisingMoonAnimation : UIHelper, IRaisingMoonAnimation
    {
        [SerializeField] private RawImage targetMoonImage;
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private RectTransform thisRectTransform;
        [SerializeField] private CanvasGroup shadowCanvasGroup;
        
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 targetPosition = Vector3.zero;
        [SerializeField] private float doMoveSpeed = 1f;
        [SerializeField] private Vector3 targetScale = new Vector3(5f, 5f, 5f);
        [SerializeField] private float doScaleSpeed = 2f;
        
        


        [ContextMenu("Play")]
        public void PlayRaisingMoon()
        {
            MoveMoonToStartPosition();
            var sq = DOTween.Sequence();
            sq.Append(thisRectTransform.DOAnchorPos(targetPosition, doMoveSpeed).OnComplete(SmoothShadeBackGround));
            sq.Append(thisRectTransform.DOScale(targetScale, doScaleSpeed)).OnComplete(() => HideBackground());
        }

        [ContextMenu("ShowMainBackground")]
        public void ShowMainBackground()
        {
            SmoothToggleVisibleCanvasGroup(backgroundCanvasGroup, 1f, true);
        }

        private void HideBackground()
        {
            MoveMoonToStartPosition();
            ToggleVisibleCanvasGroup(backgroundCanvasGroup, false);
            
            SmoothToggleVisibleCanvasGroup(shadowCanvasGroup, doScaleSpeed, false);
        }

        private void MoveMoonToStartPosition()
        {
            thisRectTransform.anchoredPosition = startPosition;
            thisRectTransform.localScale = Vector3.one;
        }

        private void SmoothShadeBackGround()
        {
            SmoothToggleVisibleCanvasGroup(shadowCanvasGroup, doScaleSpeed - 0.1f, true);
        }

        public void PlayRaisingMoon(Texture newImage)
        {
            targetMoonImage.texture = newImage;
            PlayRaisingMoon();
        }
    }
}
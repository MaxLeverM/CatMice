using System;
using UI;
using UnityEngine;

namespace Lever.UI.Windows.Behaviours
{
    [RequireComponent(typeof(AnimateTransformTo), typeof(CanvasGroup))]
    public class PopUpWindow : UIHelper
    {
        [SerializeField] private AnimateTransformTo animations;
        [SerializeField] private CanvasGroup canvasGroup;

        public AnimateTransformTo Animations => animations;
        public CanvasGroup CanvasGroup => canvasGroup;

        public virtual void Reset()
        {
            if (TryGetComponent(out AnimateTransformTo animator))
                animations = animator;

            if (TryGetComponent(out CanvasGroup canvasGroup))
                this.canvasGroup = canvasGroup;
        }
    }
}
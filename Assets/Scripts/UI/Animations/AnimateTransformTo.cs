using System;
using DG.Tweening;
using UI;
using UnityEngine;

    public class AnimateTransformTo : MonoBehaviour
    {
        
        [Header("MoveTo")]
        [SerializeField] private Vector3 startPosition = Vector3.zero;
        [SerializeField] private Vector3 endPosition = Vector3.zero;
        [SerializeField] private float timeToReachTarget = 1f;
        
        private Vector3 targetPosition;
        
        [Header("ScaleTo")]
        [SerializeField] private Vector3 startScale = Vector3.one;
        [SerializeField] private Vector3 endScale = Vector3.one;
        [SerializeField] private float timeToReachSize = 0.5f;

        private Vector3 targetScale;
        
        
        private void Awake()
        {
            targetScale = transform.localScale;
            endScale = endScale == Vector3.zero ? startScale : endScale;
            
            targetPosition = transform.position;
            endPosition = endPosition == Vector3.zero ? startPosition : endPosition;
        }

        #region Move

        [ContextMenu("MoveToTargetPosition")]
        public void MoveToTargetPosition()
        {
            transform.position = startPosition;
            transform.DOMove(targetPosition, timeToReachTarget);
        }

        [ContextMenu("MoveToEndPosition")]
        public void MoveToEndPosition(TweenCallback onComplete)
        {
            transform.DOMove(endPosition, timeToReachTarget).OnComplete(onComplete);
        }

        public void MoveToPosition(Vector3 targetPosition, float time)
        {
            transform.DOMove(targetPosition, time);
        }

        #endregion

        #region Scale

        [ContextMenu("ScaleToTargetSize")]
        public void ScaleToTargetSize()
        {
            transform.position = startScale;
            transform.DOScale(targetScale, timeToReachSize);
        }

        [ContextMenu("ScaleToEndSize")]
        public void ScaleToEndSize(TweenCallback onComplete)
        {
            transform.DOScale(endScale, timeToReachSize).OnComplete(onComplete);
        }

        
        
        public void ScaleToSize(Vector3 targetScale, float time)
        {
            transform.DOScale(targetScale, time);
        }

        #endregion
        
    }

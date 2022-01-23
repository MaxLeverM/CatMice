using System;
using DG.Tweening;
using UI;
using UnityEngine;

namespace UnityTemplateProjects.UI.Animations
{
    public class MoveTo : UIHelper
    {
        [SerializeField] private Vector3 startPosition = Vector3.zero;
        [SerializeField] private Vector3 endPosition = Vector3.zero;
        [SerializeField] private float timeToReachTarget = 1f;

        private Vector3 targetPosition;

        private void Awake()
        {
            targetPosition = transform.position;
            endPosition = endPosition == Vector3.zero ? startPosition : endPosition;
        }

        [ContextMenu("MoveToTargetPosition")]
        public void MoveToTargetPosition()
        {
            transform.position = startPosition;
            transform.DOMove(targetPosition, timeToReachTarget);
        }

        [ContextMenu("MoveToEndPosition")]
        public void MoveToEndPosition()
        {
            transform.DOMove(endPosition, timeToReachTarget);
        }

        public void MoveToPosition(Vector3 targetPosition, float time)
        {
            transform.DOMove(targetPosition, time);
        }
    }
}
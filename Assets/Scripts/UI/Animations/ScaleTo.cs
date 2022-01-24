using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleTo : MonoBehaviour
{
    [SerializeField] private Vector3 startScale = Vector3.one;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private float timeToReachTarget = 1f;

    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = transform.position;
        endScale = endScale == Vector3.zero ? startScale : endScale;
    }

    [ContextMenu("ScaleToTargetSize")]
    public void MoveToTargetPosition()
    {
        transform.position = startScale;
        transform.DOScale(targetPosition, timeToReachTarget);
    }

    [ContextMenu("ScaleToEndSize")]
    public void MoveToEndPosition()
    {
        transform.DOScale(endScale, timeToReachTarget);
    }

    public void ScaleToSize(Vector3 targetScale, float time)
    {
        transform.DOScale(targetScale, time);
    }
}

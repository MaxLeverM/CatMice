using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class BushBehaviour : MonoBehaviourPun
{
    [SerializeField] private Animator bushAnimator;

    private Volume postProcessVolume;
    private Vignette vignette;
    private float defaultVignetteValue;
    private float valueToSet = 0.5f;
    private float currentVignetteValue;
    private Coroutine currentRoutine;

    [Inject]
    private void Construct(Volume postProcessVolume)
    {
        this.postProcessVolume = postProcessVolume;
    }

    private void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);
        defaultVignetteValue = vignette.intensity.value;
        currentVignetteValue = defaultVignetteValue;
    }

    private IEnumerator SetVignette(float endValue)
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(currentVignetteValue, endValue, i);
            yield return null;
        }
        currentVignetteValue = vignette.intensity.value;
    }

    private void PlayAnimation()
    {
        bushAnimator.SetTrigger("IsInside");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            PlayAnimation();
            if (!photonView.IsMine) return;
            if(currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SetVignette(valueToSet));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            PlayAnimation();
            if (!photonView.IsMine) return;
            if(currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SetVignette(defaultVignetteValue));
        }
    }
}

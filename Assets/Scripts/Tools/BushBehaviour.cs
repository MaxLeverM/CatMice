using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class BushBehaviour : MonoBehaviour
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

    private IEnumerator SetVignetteAndAnimate(float endValue)
    {
        bushAnimator.SetTrigger("IsInside");
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(currentVignetteValue, endValue, i);
            yield return null;
        }
        currentVignetteValue = vignette.intensity.value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            if(currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SetVignetteAndAnimate(valueToSet));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            if(currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SetVignetteAndAnimate(defaultVignetteValue));
        }
    }
}

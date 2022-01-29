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

    [Inject]
    private void Construct(Volume postProcessVolume)
    {
        this.postProcessVolume = postProcessVolume;
    }

    private void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);
        defaultVignetteValue = vignette.intensity.value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            bushAnimator.SetBool("IsInside", true);
            vignette.intensity.value = valueToSet;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControl>())
        {
            bushAnimator.SetBool("IsInside", false);
            vignette.intensity.value = defaultVignetteValue;
        }
    }
}

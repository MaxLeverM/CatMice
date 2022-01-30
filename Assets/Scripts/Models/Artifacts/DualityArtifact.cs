using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class DualityArtifact : MonoBehaviour
{
    [SerializeField] private float lifeTime = 8;
    
    private bool isEffectPositive;

    public bool IsEffectPositive { get => isEffectPositive; set => isEffectPositive = value; }
    
    private IEnumerator Start()
    {
        GenerateEffect();
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    
    protected abstract void ApplyEffect(PlayerControl player);
    
    protected virtual void GenerateEffect()
    {
        isEffectPositive = Random.Range(0, 2) > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerControl>();
        if (player != null && !player.IsHunter)
        {
            ApplyEffect(player);
            Destroy(gameObject);
        }
    }
}

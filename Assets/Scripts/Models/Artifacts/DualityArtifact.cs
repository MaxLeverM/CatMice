using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class DualityArtifact : MonoBehaviour
{
    [SerializeField] private float lifeTime = 8;
    private PhotonView photonView;

    private bool isEffectPositive;

    public bool IsEffectPositive
    {
        get => isEffectPositive;
        set => isEffectPositive = value;
    }

    private void Awake()
    {
        photonView = PhotonView.Get(this);
    }

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
        var player = other.GetComponent<PlayerControlNetworking>();
        if (player != null && !player.IsHunter && player.GetPhotonView.IsMine)
        {
            ApplyEffect(player);
            photonView.RPC("DestroyMyself", RpcTarget.All);
        }
    }

    [PunRPC]
    protected void DestroyMyself()
    {
        Destroy(gameObject);
    }
}
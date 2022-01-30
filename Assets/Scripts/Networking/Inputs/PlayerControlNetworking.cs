﻿using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

class PlayerControlNetworking : PlayerControl
{
    [Header("Networking")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private int timeToRespawn = 5;
    
    [SerializeField] private bool isDead = false;
    
    public event Action OnKillVictim;
    public event Action OnRespawn;
    public event Action<int> OnTimeToRespawnChanged;
    public event Action OnDead;

    protected override void Start()
    {
        if (!photonView.IsMine)
        {
            SetLayerRecursively(playerModel, LayerMask.NameToLayer("OtherPlayer"));
        }

        base.Start();
    }

    protected override void Update()
    {
        if(photonView.IsMine && !isDead)
            base.Update();
    }

    private void SetLayerRecursively(GameObject gameObject, LayerMask layerMask)
    {
        gameObject.layer = layerMask;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            SetLayerRecursively(child.gameObject, layerMask);
        }
    }

    protected override void Attack()
    {
        playerAnimator.SetTrigger("Kick");
        var ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, attackDistance, LayerMask.NameToLayer("OtherPlayer")))
        {
            var otherPlayer = hit.collider.GetComponent<PlayerControlNetworking>();
            if (!otherPlayer.IsHunter)
            {
                otherPlayer.Dead();
                OnKillVictim?.Invoke();
            }
        }
    }
    
    public override void TransformToCat()
    {
        photonView.RPC("TransformToCatNetwork", RpcTarget.All);
    }

    [PunRPC]
    private void TransformToCatNetwork()
    {
        StartCoroutine(TransformToCatCoroutine());
    }

    public override void Dead()
    {
        photonView.RPC("DeadNetwork", RpcTarget.All);
    }

    [PunRPC]
    private void DeadNetwork()
    {
        if (photonView.IsMine)
        {
            isDead = true;
            OnDead?.Invoke();
            StartCoroutine(RespawnTimer());
        }
    }

    private IEnumerator RespawnTimer()
    {
        for (int i = 0; i < timeToRespawn; i++)
        {
            OnTimeToRespawnChanged?.Invoke(timeToRespawn - i);
            yield return new WaitForSeconds(1f);
        }
        isDead = false;
        OnRespawn?.Invoke();
    }
}
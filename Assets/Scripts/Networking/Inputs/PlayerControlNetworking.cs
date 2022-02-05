using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

class PlayerControlNetworking : PlayerControl
{
    [Header("Networking")] [SerializeField]
    private GameObject playerModel;

    [SerializeField] private PhotonView photonView;
    [SerializeField] private int timeToRespawn = 5;

    [SerializeField] private bool isDead = false;

    public PhotonView GetPhotonView => photonView;
    public event Action<string> OnKillVictim;
    public event Action OnRespawn;
    public event Action<int> OnTimeToRespawnChanged;
    public event Action OnDead;
    public event Action OnTransformToCat;

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
        if (photonView.IsMine && !isDead)
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
        pawsAnimator.SetTrigger("Kick");
        var ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.5f, out hit, attackDistance, LayerMask.NameToLayer("OtherPlayer")))
        {
            var otherPlayer = hit.collider.GetComponent<PlayerControlNetworking>();
            if (!otherPlayer.IsHunter && !otherPlayer.isDead)
            {
                otherPlayer.Dead();
                photonView.RPC("KillVictimEvent", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            }
        }
    }

    public void TransformToMouse()
    {
        photonView.RPC("TransformToMouseNetwork", RpcTarget.All);
    }

    [PunRPC]
    public void KillVictimEvent(string nickName)
    {
        OnKillVictim?.Invoke(nickName);
    }

    [PunRPC]
    private void TransformToMouseNetwork()
    {
        StartCoroutine(TransformToMouseCoroutine());
    }

    public void TransformToCat(bool withCallback)
    {
        if (withCallback)
            OnTransformToCat?.Invoke();
        photonView.RPC("TransformToCatNetwork", RpcTarget.All);
    }

    public override void TransformToCat()
    {
        OnTransformToCat?.Invoke();
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
        isDead = true;
        OnDead?.Invoke();
        playerAnimator.SetBool("Death", isDead);
        StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer()
    {
        for (int i = 0; i < timeToRespawn; i++)
        {
            OnTimeToRespawnChanged?.Invoke(timeToRespawn - i);
            yield return new WaitForSeconds(1f);
        }
        isDead = false;
        playerAnimator.SetBool("Death", isDead);
        OnRespawn?.Invoke();
    }
}
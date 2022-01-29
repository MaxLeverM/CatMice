using Photon.Pun;
using UnityEngine;

class PlayerControlNetworking : PlayerControl
{
    [Header("Networking")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PhotonView photonView;

    protected override void Start()
    {
        if (!photonView.IsMine)
        {
            SetLayerRecursively(playerModel, LayerMask.NameToLayer("OtherPlayer"));
        }

        base.Start();
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
}
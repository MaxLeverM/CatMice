using Photon.Pun;
using UnityEngine;

namespace Lever.Networking.Inputs
{
    public class PlayerCameraNetworking : PlayerCamera
    {
        [SerializeField] private PhotonView photonView;
        protected override void Start()
        {
            if(!photonView.IsMine)
                Destroy(gameObject);
        }
    }
}
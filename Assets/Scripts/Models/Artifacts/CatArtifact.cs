using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Lever.Models
{
    public class CatArtifact : DualityArtifact, IPunInstantiateMagicCallback
    {
        private Vector3 teleportPoint;

        public Vector3 TeleportPoint
        {
            get => teleportPoint;
            set => teleportPoint = value;
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;
            teleportPoint = (Vector3) instantiationData[0];
        }

        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.TransformToCat();
                return;
            }
            
            player.TeleportPlayer(teleportPoint);
        }
    }
}
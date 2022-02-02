using UnityEngine;
using Zenject;

namespace Lever.Models
{
    public class CatArtifact : DualityArtifact
    {
        private Vector3 teleportPoint;

        public Vector3 TeleportPoint
        {
            get => teleportPoint;
            set => teleportPoint = value;
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
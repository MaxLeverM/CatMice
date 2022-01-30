using UnityEngine;
using Zenject;

namespace Lever.Models
{
    public class CatArtifact : DualityArtifact
    {
        private Transform[] points;
        private TeleportationPoints _teleportationPoints;
        private Vector3 teleportPoint;

        public Vector3 TeleportPoint
        {
            get => teleportPoint;
            set => teleportPoint = value;
        }

        [Inject]
        private void Construct(TeleportationPoints teleportationPoints)
        {
            _teleportationPoints = teleportationPoints;
        }
        
        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.TransformToCat();
                return;
            }

            // points = _teleportationPoints.TeleportPoints;
            // var randomPointIndex = Random.Range(0, points.Length);
            player.TeleportPlayer(teleportPoint);
        }
    }
}
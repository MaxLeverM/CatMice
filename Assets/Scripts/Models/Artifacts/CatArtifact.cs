using UnityEngine;
using Zenject;

namespace Lever.Models
{
    public class CatArtifact : DualityArtifact
    {
        private Transform[] points;
        private TeleportationPoints _teleportationPoints;

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

            points = _teleportationPoints.TeleportPoints;
            var randomPointIndex = Random.Range(0, points.Length);
            player.gameObject.transform.position = points[randomPointIndex].position;
        }
    }
}
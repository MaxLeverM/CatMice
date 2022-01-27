using UnityEngine;
using Zenject;

namespace Lever.Models
{
    public class CatArtifact : DualityArtifact
    {
        private Transform[] points;
        private ITeleportationPoints teleportationPoints;

        [Inject]
        private void Construct(ITeleportationPoints teleportationPoints)
        {
            this.teleportationPoints = teleportationPoints;
        }
        
        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.TransformToCat();
                return;
            }

            points = teleportationPoints.GetPoints();
            var randomPointIndex = Random.Range(0, points.Length);
            player.gameObject.transform.position = points[randomPointIndex].position;
        }
    }
}
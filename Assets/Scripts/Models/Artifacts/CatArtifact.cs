namespace Lever.Models
{
    public class CatArtifact : DualityArtifact
    {
        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.TransformToCat();
                return;
            }
            
            //TODO: Implement random teleportation
        }
    }
}
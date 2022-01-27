namespace Lever.Models
{
    public class SpeedArtifact : DualityArtifact
    {
        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.ApplyNewSpeed(IsEffectPositive);
                return;
            }

            player.ApplyNewSpeed(IsEffectPositive);
        }
    }
}
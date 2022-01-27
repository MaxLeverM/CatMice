namespace Lever.Models
{
    public class BigJumpArtifact : DualityArtifact
    {
        protected override void ApplyEffect(PlayerControl player)
        {
            if (IsEffectPositive)
            {
                player.ApplyBigJump();
                return;
            }

            player.DisableJump();
        }
    }
}
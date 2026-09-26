using System;

public sealed class PlayerDamageController : IPlayerDamageReceiver
{
    private readonly PowerController powerController;
    private readonly IPlayerProtectionState playerProtection;

    public PlayerDamageController(
        PowerController powerController,
        IPlayerProtectionState playerProtection)
    {
        this.powerController = powerController
            ?? throw new ArgumentNullException(nameof(powerController));
        this.playerProtection = playerProtection
            ?? throw new ArgumentNullException(nameof(playerProtection));
    }

    public bool TryTakeDamage(int amount)
    {
        if (playerProtection.IsActive)
        {
            return false;
        }

        return powerController.ReducePower(amount);
    }
}

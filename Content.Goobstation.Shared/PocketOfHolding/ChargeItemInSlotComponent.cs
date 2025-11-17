using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.PocketOfHolding;

/// <summary>
/// This is used for the pocket of holding recharger item
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ChargeItemInSlotComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool HasRecharge;
}

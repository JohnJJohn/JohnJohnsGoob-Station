using Content.Goobstation.Shared.Power.Components;
using Robust.Shared.Containers;

namespace Content.Goobstation.Shared.PocketOfHolding;

/// <summary>
/// This handles the pocket of holding (recharge) item
/// </summary>
public sealed class ChargeItemInSlotSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<ChargeItemInSlotComponent, EntInsertedIntoContainerMessage>(OnItemAdded);
        SubscribeLocalEvent<ChargeItemInSlotComponent, EntRemovedFromContainerMessage>(OnItemRemoved);
    }

    private void OnItemRemoved(EntityUid uid, ChargeItemInSlotComponent component, EntRemovedFromContainerMessage args)
    {
        if (!component.HasRecharge)
            RemComp<BatterySelfRechargerComponent>(args.Entity);
    }

    private void OnItemAdded(EntityUid uid, ChargeItemInSlotComponent component, EntInsertedIntoContainerMessage args)
    {
        if (HasComp<BatterySelfRechargerComponent>(args.Entity)) // if it already has recharging don't remove it
        {
            component.HasRecharge = true;
            return;
        }

        component.HasRecharge = false;
        var battery = EnsureComp<BatterySelfRechargerComponent>(args.Entity);
        battery.AutoRecharge = true;
        battery.AutoRechargeRate = 50f;
    }
}

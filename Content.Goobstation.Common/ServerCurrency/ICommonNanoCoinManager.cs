using Robust.Shared.Network;

namespace Content.Goobstation.Common.ServerCurrency;

public interface ICommonNanoCoinManager
{
    public void Initialize();

    /// <summary>
    /// Saves player balances to the database before allowing the server to shutdown.
    /// </summary>
    public void Shutdown();

    public float ChangeNanoCoins(NetUserId userId, float amount);
}


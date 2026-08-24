using System.Threading.Tasks;
using Content.Goobstation.Common.ServerCurrency;
using Content.Server.Database;
using Robust.Server.Player;
using Robust.Shared.Asynchronous;
using Robust.Shared.Network;

namespace Content.Goobstation.Server.ServerCurrency;

public sealed class ServerNanoCoinsSystem : ICommonNanoCoinManager
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly ITaskManager _task = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    private ISawmill _sawmill = default!;
    private readonly List<Task> _pendingSaveTasks = new();

    public void Initialize()
    {
        _sawmill = Logger.GetSawmill("nano-coins");
    }

    public void Shutdown()
    {
        _task.BlockWaitOnTask(Task.WhenAll(_pendingSaveTasks));
    }

    private async Task<float> ModifyBalanceAsync(NetUserId userId, float amountDelta)
    {
        var task = Task.Run(() => _db.ModifyNanoCoins(userId, amountDelta));
        TrackPending(task);
        return await task;
    }

    private async void TrackPending(Task task)
    {
        _pendingSaveTasks.Add(task);

        try
        {
            await task;
        }
        finally
        {
            _pendingSaveTasks.Remove(task);
        }
    }

    public float ChangeNanoCoins(NetUserId userId, float amount)
    {
        var result = Task.Run(() => ModifyBalanceAsync(userId, amount)).GetAwaiter().GetResult();
        _sawmill.Info("added nanocoins to: " + userId + "amount: " + result);

        return result;
    }
}

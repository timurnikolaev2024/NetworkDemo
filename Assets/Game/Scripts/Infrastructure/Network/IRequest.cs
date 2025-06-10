using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Infrastructure
{
    public interface IRequest<TReturn>
    {
        UniTask<TReturn> ExecuteAsync(CancellationToken ct);
    }
}
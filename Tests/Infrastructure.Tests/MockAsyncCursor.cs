using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Infrastructure.Tests
{
    public class MockAsyncCursor<T> : IAsyncCursor<T>
    {
        private readonly IEnumerable<T> _items;
        private bool moveNextCalled;

        public MockAsyncCursor(IEnumerable<T> items)
        {
            _items = items ?? Enumerable.Empty<T>();
        }

        public IEnumerable<T> Current => _items;

        public bool MoveNext(CancellationToken cancellationToken = new())
        {
            return !moveNextCalled && (moveNextCalled = true);
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
        }
    }
}
namespace CodeSample;

internal static class EnumerableExtensions
{
    /// <summary>
    /// Adopts the <see cref="IEnumerable{T}"/> sequence to <see cref="IAsyncEnumerable{T}"/> interface.
    /// </summary>
    /// <returns>The asynchronous version of the original sequence.</returns>
    public static IAsyncEnumerable<T> AsAsync<T>(this IEnumerable<T> enumerable) => new AsyncEnumerableAdaptor<T>(enumerable);

    private sealed class AsyncEnumerableAdaptor<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;
        public AsyncEnumerableAdaptor(IEnumerable<T> enumerator) => _enumerable = enumerator;
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator<T>(_enumerable.GetEnumerator());
    }

    private sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        public AsyncEnumerator(IEnumerator<T> enumerator) => _enumerator = enumerator;

        public T Current => _enumerator.Current;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_enumerator.MoveNext());
    }
}




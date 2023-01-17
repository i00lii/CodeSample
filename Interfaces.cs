namespace CodeSample;

/// <summary>
/// Represents the very common interface of <see cref="TSignal"/> receiver.
/// </summary>
public interface IReceiver<TSignal>
{
    /// <summary>
    /// Opens the channel and starts to recieve <see cref="TSignal"/> sequence.
    /// </summary>
    /// <returns>The asynchronous <see cref="TSignal"/> sequence.</returns>
    IAsyncEnumerable<TSignal> Connect(CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the very common interface for <see cref="TSignal"/> consuming.
/// </summary>
public interface IConsumer<TSignal, TResult>
{
    /// <summary>
    /// Consumes the whole <see cref="TSignal"/> sequence from the <paramref name="receiver"/> 
    /// and returns the <see cref="TResult"/> aggregation.
    /// </summary>
    /// <param name="receiver">The asynchronous <see cref="TSignal"/> sequence provider.</param>
    /// <returns>The aggregation of the <see cref="TSignal"/> sequence.</returns>
    Task<TResult> Consume(IReceiver<TSignal> receiver);
}

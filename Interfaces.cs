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

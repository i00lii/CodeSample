namespace CodeSample;

/// <summary>
/// The <see cref="IConsumer{TSignal, TResult}"/> implementation 
/// which aggregates the <see cref="TSignal"/> sequence into single <see cref="TResult"/> instance.
/// </summary>
public class AggregareConsumer<TSignal, TResult> : IConsumer<TSignal, TResult>
{
    private readonly Func<TSignal, TResult, TResult> _aggregateLambda;
    private readonly Func<TResult> _initialValueFactory;

    public AggregareConsumer(Func<TSignal, TResult, TResult> aggregate, Func<TResult> initialValueFactory)
    {
        _aggregateLambda = aggregate;
        _initialValueFactory = initialValueFactory;
    }

    /// <inheritdoc />
    public async Task<TResult> Consume(IReceiver<TSignal> receiver)
    {
        TResult result = _initialValueFactory();

        await foreach(TSignal signal in receiver.Connect())
        {
            result = _aggregateLambda(signal, result);
        }

        return result;
    }
}

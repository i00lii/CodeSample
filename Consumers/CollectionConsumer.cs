namespace CodeSample;

/// <summary>
/// The <see cref="IConsumer{TSignal, TResult}"/> implementation 
/// which aggregates the <see cref="TSignal"/> sequence into the <see cref="List{TSignal}"/> instance.
/// </summary>
internal class CollectionConsumer<TSignal> : IConsumer<TSignal, List<TSignal>>
{
    private readonly AggregareConsumer<TSignal, List<TSignal>> _inner;

    public CollectionConsumer() 
        => _inner = new AggregareConsumer<TSignal, List<TSignal>>
        (
            (current, aggregate) => 
            { 
                aggregate.Add(current);
                return aggregate;
            },
            () => new List<TSignal>()
        );

    public Task<List<TSignal>> Consume(IReceiver<TSignal> receiver) => _inner.Consume(receiver);
}

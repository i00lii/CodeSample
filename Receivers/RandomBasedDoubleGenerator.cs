namespace CodeSample;

/// <summary>
/// Provides <see cref="IReceiver{double}"/> based on <see cref="Random"/>.
/// </summary>
public sealed class RandomBasedDoubleGenerator : IReceiver<double>
{
    private readonly Random _random;
    private readonly int _count;

    public RandomBasedDoubleGenerator(Random random, int count) => (_random, _count) = (random, count);
    
    /// <inheritdoc />
    public IAsyncEnumerable<double> Connect(CancellationToken cancellationToken = default) => Yield().AsAsync();

    private IEnumerable<double> Yield()
    {
        foreach (int _ in Enumerable.Range(0, _count))
        {
            yield return _random.NextDouble();
        }
    }
}
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CodeSample;

[TestFixture]
internal class Tests
{
    [Test]   
    public async Task RandomBasedNumberGeneratorProducesSequenceAsExpexted()
    {
        double[] expected = new[]
        {
            0.66810646591154230,
            0.14090729837348093,
            0.12551828945312568,
            0.52276427602524130,
            0.16843422416990353
        };

        IReceiver<double> receiver = new RandomBasedDoubleGenerator(new Random(42), 5);
        IConsumer<double, List<double>> consumer = new CollectionConsumer<double>();

        IReadOnlyCollection<double> result = await consumer.Consume(receiver);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task HttpBasedNumberGeneratorProducesSequenceAsExpexted()
    {
        long[] source = new[]
        {
            66810646591154230,
            14090729837348093,
            12551828945312568,
            52276427602524130,
            16843422416990353
        };

        MockableHttpClientHandler http = Substitute.ForPartsOf<MockableHttpClientHandler>();

        http
            .SendAsyncMockable
            (
                Arg.Any<HttpRequestMessage>(),
                Arg.Any<CancellationToken>()
            )
            .Returns
            (
                ResponseFor(source[0]),
                source.Skip(1).Select(x => ResponseFor(x)).ToArray()
            );
        
        IReceiver<long> receiver = new RestApiBasedInt64Generator(new HttpClient(http), 5);
        IConsumer<long, List<long>> consumer = new CollectionConsumer<long>();

        IReadOnlyCollection<long> result = await consumer.Consume(receiver);
        result.Should().BeEquivalentTo(source);


        static HttpResponseMessage ResponseFor(long value) 
            => new()
            {
                Content = new ByteArrayContent(BitConverter.GetBytes(value))
            };
    }
}

public class MockableHttpClientHandler : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => SendAsyncMockable(request, cancellationToken);

    public virtual Task<HttpResponseMessage> SendAsyncMockable(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromException<HttpResponseMessage>(new NotImplementedException());
}

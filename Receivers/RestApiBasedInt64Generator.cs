using System.Buffers;
using System.Runtime.CompilerServices;

namespace CodeSample;

/// <summary>
/// Provides <see cref="IReceiver{long}"/> based on API provided by 'httpbin.org'.
/// </summary>
public sealed class RestApiBasedInt64Generator : IReceiver<long>
{
    private const int _byteCount = sizeof(long);

    private readonly HttpClient _http;
    private readonly int _count;
    private readonly Uri _url;

    public RestApiBasedInt64Generator(HttpClient http, int count)
    {
        _http = http;
        _count = count;
        _url = new Uri($"https://httpbin.org/bytes/{_byteCount}");
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<long> Connect([EnumeratorCancellation] CancellationToken cancellation = default)
    {
        foreach (int _ in Enumerable.Range(0, _count))
        {
            yield return await InvokeRequest(cancellation);
        }
    }

    private async Task<long> InvokeRequest(CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _url);
        using HttpResponseMessage response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        using Stream content = await response.Content.ReadAsStreamAsync(cancellationToken);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(_byteCount);

        try
        {
            await content.ReadExactlyAsync(buffer, 0, _byteCount, cancellationToken);
            return BitConverter.ToInt64(buffer, 0);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
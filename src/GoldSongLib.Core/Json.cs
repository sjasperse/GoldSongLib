using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoldSongLib.Core;

static class Json
{
    private static readonly JsonSerializerOptions jsonSerializerOptions
        = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public static string Serialize<T>(T value)
        => JsonSerializer.Serialize(value, jsonSerializerOptions);

    public static Task SerializeAsync<T>(Stream target, T value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeAsync(target, value, jsonSerializerOptions, cancellationToken);

    public static T? Deserialize<T>(string value)
        => JsonSerializer.Deserialize<T>(value, jsonSerializerOptions);

    public static ValueTask<T?> DeserializeAsync<T>(Stream value, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync<T>(value, jsonSerializerOptions, cancellationToken);
}

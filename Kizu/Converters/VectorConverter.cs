using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kizu.Converters
{
    internal class VectorConverter : ValueConverter<float[], string>
    {
        public VectorConverter()
            : base(
                  v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                  v => JsonSerializer.Deserialize<float[]>(v, JsonSerializerOptions.Default) ?? new float[768])
        { }
    }
}

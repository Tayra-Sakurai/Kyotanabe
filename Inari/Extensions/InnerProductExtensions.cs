using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inari.Extensions
{
    public static class InnerProductExtensions
    {
        /// <summary>
        /// Gets the inner product of the two array vectors.
        /// </summary>
        /// <param name="vector1">The first vector to be multiplied.</param>
        /// <param name="vector2">The second vector to multiply,</param>
        /// <returns>The inner product value.</returns>
        public static float GetInnerProduct(this IEnumerable<float> vector1, IEnumerable<float> vector2)
        {
            return vector1.Zip(vector2)
                .Sum(v => v.First * v.Second);
        }

        /// <inheritdoc cref="GetInnerProduct(IEnumerable{float}, IEnumerable{float})"/>
        public static float GetInnerProduct(this ReadOnlySpan<float> vector1, ReadOnlySpan<float> vector2)
        {
            return vector1.ToArray().GetInnerProduct(vector2.ToArray());
        }
    }
}

using Inari.Extensions;
using Inari.Options;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inari.Services
{
    public class GoogleEmbeddingService : IEmbeddingService<TaskType>
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator;

        public GoogleEmbeddingService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            this.embeddingGenerator = embeddingGenerator;
        }

        public async Task<float[]> GetVectorAsync(string query, string? title = null, TaskType taskType = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);

            AdditionalPropertiesDictionary additionalProperties = new();

            switch (taskType)
            {
                case TaskType.SearchResult:
                    additionalProperties["task_type"] = "RETRIEVAL_QUERY";
                    break;

                case TaskType.QuestionAnswering:
                    additionalProperties["task_type"] = "QUESTION_ANSWERING";
                    break;

                case TaskType.FactChecking:
                    additionalProperties["task_type"] = "FACT_VERIFICATION";
                    break;

                case TaskType.CodeRetrieval:
                    additionalProperties["task_type"] = "CODE_RETRIEVAL_QUERY";
                    break;

                case TaskType.Classification:
                    additionalProperties["task_type"] = "CLASSIFICATION";
                    break;

                case TaskType.Clustering:
                    additionalProperties["task_type"] = "CLUSTERING";
                    break;

                case TaskType.RetrievalDocument:
                    additionalProperties["task_type"] = "RETRIEVAL_DOCUMENT";
                    additionalProperties["title"] = title;
                    break;

                default:
                    additionalProperties["task_type"] = "SEMANTIC_SIMILARITY";
                    break;
            }

            EmbeddingGenerationOptions options = new()
            {
                Dimensions = 768,
                AdditionalProperties = additionalProperties,
            };

            Embedding<float> embedding = await embeddingGenerator.GenerateAsync(query, options);

            return embedding.Vector.ToArray();
        }

        public async IAsyncEnumerable<(T Value, float MatchRate, int Rank)> SearchAsync<T>(string query, IEnumerable<float[]> vectors, IEnumerable<T> values)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);

            if (vectors.Count() != values.Count())
                throw new InvalidOperationException("Unmatched length of the data.");

            float[] vec1 = await GetVectorAsync(query, null, TaskType.SearchResult);

            List<float> matchRates = [];

            foreach (var (value, vector) in values.Zip(vectors))
            {
                if (value is T t && vector is float[] vec2)
                {
                    int rank = matchRates.Count(v => v > vec1.GetInnerProduct(vec2));
                    matchRates.Add(vec1.GetInnerProduct(vec2));
                    yield return (t, vec1.GetInnerProduct(vec2), rank);
                }
            }
        }
    }
}

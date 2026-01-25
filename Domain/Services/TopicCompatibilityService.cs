using Domain.Entities;

namespace Domain.Services;

// Get both groups and serialize them into dictionaries: Dictionary<TKey, List<string>>.
// // Then, retrieve all topic keys and loop through each one,
// // comparing the corresponding answer lists (List<string>) from both dictionaries.

public class TopicCompatibilityService
{
    public static Dictionary<Guid?, decimal> CompareByTopicPercent(
        IEnumerable<IGrouping<Guid?, TopicStats>> parsed1,
        IEnumerable<IGrouping<Guid?, TopicStats>> parsed2
        )
    {
        // / Transformo em dicionários pra acessar rápido por TopicId
        Dictionary<Guid?, List<string?>> d1 = parsed1.ToDictionary(g => g.Key, g => g.Select(x => x.Answer).ToList());
        Dictionary<Guid?, List<string?>> d2 = parsed2.ToDictionary(g => g.Key, g => g.Select(x => x.Answer).ToList());

        // Conjunto de todos os tópicos presentes em qualquer lado
        IEnumerable<Guid?> allTopics = d1.Keys.Union(d2.Keys);
        var result = new Dictionary<Guid?, decimal>();

        foreach (var topicId in allTopics)
        {
            // get string list of d1
            d1.TryGetValue(topicId, out var a1);
            d2.TryGetValue(topicId, out var a2);

            a1 ??= new List<string?>();
            a2 ??= new List<string?>();

            // compara só até o menor tamanho (evita estourar)
            var total = Math.Min(a1.Count, a2.Count);

            if (total == 0)
            {
                result[topicId] = 0m;
                continue;
            }

            int matches = 0;

            for (int i = 0; i < total; i++)
            {
                // normaliza (opcional): trim e case-insensitive
                var s1 = a1[i]?.Trim();
                var s2 = a2[i]?.Trim();

                if (string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase))
                    matches++;
            }

            var percent = (decimal)matches * 100m / (decimal)total;
            result[topicId] = Math.Round(percent, 2);
        }

        return result;

    }
}
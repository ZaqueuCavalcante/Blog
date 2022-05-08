using NUnit.Framework;

namespace Blog.Tests;

public class Teste : TestAttribute
{
    public Teste(string description)
    {
        Description = description;
    }
}

public class Streams
{
    private static List<int> InvalidPostRatings = new List<int>
    {
        -1, 0, 6,
    };

    public static IEnumerable<object[]> InvalidPostRatingsStream()
    {
        foreach (var rating in InvalidPostRatings)
        {
            yield return new object[] { rating };
        }
    }
}

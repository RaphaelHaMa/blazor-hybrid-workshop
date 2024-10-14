namespace MonkeyFinderHybrid.Model;

public class RatingState
{
    public Dictionary<Monkey, int> MonkeyRatings { get; } = [];
    public EventHandler? RatingChanged;

    public void AddOrUpdateRating(Monkey monkey, int rating)
    {
        if(!MonkeyRatings.TryAdd(monkey, rating))
        {
            MonkeyRatings[monkey] = rating;
        }

        RatingChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetRating(Monkey monkey)
    {
        if (MonkeyRatings.TryGetValue(monkey, out int rating))
        {
            return rating;
        }

        return 0;
    }
}

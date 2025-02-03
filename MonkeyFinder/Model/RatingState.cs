namespace MonkeyFinder.Model;

public class RatingState
{
    public Dictionary<Monkey, int> MonkeyRatings { get; } = [];
    public event EventHandler? RatingChanged;

    public void AddOrUpdateRating(Monkey monkey, int rating)
    {
        if (!MonkeyRatings.TryAdd(monkey, rating))
        {
            MonkeyRatings[monkey] = rating;
        }
        RatingChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetRating(Monkey monkey)
    {
        if (MonkeyRatings.TryGetValue(monkey, out var rating))
        {
            return rating;
        }

        return 0;
    }
}
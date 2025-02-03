using MonkeyFinder.Model;

namespace MonkeyFinder.MauiPages;

public partial class MonkeyRatingPage : ContentPage
{
    private readonly Monkey _monkeyToRate;
    private readonly RatingState _ratingState;

    public MonkeyRatingPage(Monkey monkey, RatingState ratingState)
    {
        InitializeComponent();

        _ratingState = ratingState;
        _monkeyToRate = monkey;
        Rating.Value = _ratingState.GetRating(_monkeyToRate);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _ratingState.AddOrUpdateRating(_monkeyToRate, Rating.Value);
    }
}
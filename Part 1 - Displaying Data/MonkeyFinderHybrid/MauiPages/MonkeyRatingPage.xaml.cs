using AlohaKit.Controls;

namespace MonkeyFinderHybrid.MauiPages;

public partial class MonkeyRatingPage : ContentPage
{
    private readonly RatingState _ratingState;
    private readonly Monkey _monkey;

    public MonkeyRatingPage(Monkey monkey, RatingState ratingState)
	{
		InitializeComponent();

        _ratingState = ratingState;
        _monkey = monkey;
        rating.Value = _ratingState.GetRating(_monkey);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        _ratingState.AddOrUpdateRating(_monkey, rating.Value);
    }
}
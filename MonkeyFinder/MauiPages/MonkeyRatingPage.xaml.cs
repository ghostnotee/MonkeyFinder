using System.Diagnostics;

namespace MonkeyFinder.MauiPages;

public partial class MonkeyRatingPage : ContentPage
{
    public MonkeyRatingPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object? sender, EventArgs e)
    {
        Debug.WriteLine("Monkey rating: " + Rating.Value);
        Navigation.PopModalAsync(); 
    }
}
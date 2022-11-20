using Verdure.Kame.Core.Models;

namespace Verdure.Kame.Maui.Assistant.ViewModel;

[QueryProperty(nameof(FaceScreenFrame), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    FaceScreenFrame monkey;

    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
        
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}

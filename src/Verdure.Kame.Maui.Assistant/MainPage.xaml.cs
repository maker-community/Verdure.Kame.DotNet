using Verdure.Kame.Core.Services;

namespace Verdure.Kame.Maui.Assistant
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly DataTransmissionClient _client;

        public MainPage(DataTransmissionClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/jpg", "application/png" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".png", ".jpg" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
                FileTypes = customFileType,
            };

            var file = await PickAndShow(options);

            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
            try
            {
                var ret = await _client.SayHelloAsync("hello i am Kame");

                Result.Text = ret;
            }
            catch(Exception ex)
            {
                Result.Text = ex.Message;
            }
        }
        public async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var image = ImageSource.FromStream(() => stream);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }
    }
}
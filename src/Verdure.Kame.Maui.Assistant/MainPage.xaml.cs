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
    }
}
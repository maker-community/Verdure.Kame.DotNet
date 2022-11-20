﻿using Verdure.Kame.Core.Services;
using Verdure.Kame.Maui.Assistant.Services;

namespace Verdure.Kame.Maui.Assistant
{
    public partial class MainPage : ContentPage
    {

        private readonly DataTransmissionClient _client;

        private readonly IFaceScreenMediaPlayer _faceScreenMediaPlayer;
        public MainPage(DataTransmissionClient client, IFaceScreenMediaPlayer faceScreenMediaPlayer)
        {
            InitializeComponent();
            _client = client;
            _faceScreenMediaPlayer = faceScreenMediaPlayer;
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

            try
            {
                //var ret = await _client.SayHelloAsync("hello i am Kame");

                var file = await PickAndShow(options);

            }
            catch (Exception ex)
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
                        ProcessRing.IsRunning = true;

                        using var stream = await result.OpenReadAsync();

                        var image = ImageSource.FromStream(() => stream);

                        FaceScreenImage.Source = image;
                        var data = await _faceScreenMediaPlayer.ConvertImageStreamToBytesAsync(stream);

                        var ret = await _client.PlayImageOnFaceScreenAsync(data);

                        Result.Text = ret;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ProcessRing.IsRunning = false;
                // The user canceled or something went wrong
            }

            return null;
        }

        private async void OnVideoClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
              new Dictionary<DevicePlatform, IEnumerable<string>>
              {
                    { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/mp4"} }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".mp4" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
              });

            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
                FileTypes = customFileType,
            };

            try
            {
                //var ret = await _client.SayHelloAsync("hello i am Kame");

                var file = await PickAndShowVideoAsync(options);

            }
            catch (Exception ex)
            {
                VideoResult.Text = ex.Message;
            }
            finally
            {
                ProcessRing.IsRunning = false;
            }
        }

        public async Task<FileResult> PickAndShowVideoAsync(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);

                if (result != null)
                {
                    if (result.FileName.EndsWith("mp4", StringComparison.OrdinalIgnoreCase))
                    {
                        //using var stream = await result.OpenReadAsync();

                        ProcessRing.IsRunning = true;

                        var dataList = await _faceScreenMediaPlayer.ConvertVideoToFaceScreenFramesAsync(result.FullPath);

                        var ret = await _client.PlayVideoOnFaceScreenAsync(dataList.ToList());

                        VideoResult.Text = ret;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var actionName = button.StyleId;

                try
                {
                    await _client.ControlQuadrupedPostureAsync(actionName);
                }
                catch (Exception ex)
                {
                    VideoResult.Text = ex.Message;
                }
            }
        }
    }
}
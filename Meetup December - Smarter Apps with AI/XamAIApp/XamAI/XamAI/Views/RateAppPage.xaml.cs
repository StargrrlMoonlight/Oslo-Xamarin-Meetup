using System;
using System.Diagnostics;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XamAI.Models;
using XamAI.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamAI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RateAppPage : ContentPage
	{

        MediaFile photo;
        IFaceRecognitionService _faceRecognitionService;


        public RateAppPage ()
		{
			InitializeComponent ();
            _faceRecognitionService = new FaceRecognitionService();
        }

        async void OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                // Select an image from camera roll (if using a emulator)
                photo = await CrossMedia.Current.PickPhotoAsync();
                
                // Take a photo with your device instead
                /*
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "emotion.jpg",
                    PhotoSize = PhotoSize.Small
                });
                */

                if (photo != null)
                {
                    image.Source = ImageSource.FromStream(photo.GetStream);
                }
            }
            else
            {
                await DisplayAlert("No Camera", "Camera unavailable.", "OK");
            }

            // Recognize emotion
            try
            {
                if (photo != null)
                {
                    var faceAttributes = new FaceAttributeType[] { FaceAttributeType.Emotion, FaceAttributeType.Age, FaceAttributeType.Gender };
                    using (var photoStream = photo.GetStream())
                    {
                        ((Button)sender).IsEnabled = false;
                        activityIndicator.IsRunning = true;

                        Face[] faces = await _faceRecognitionService.DetectAsync(photoStream, true, false, faceAttributes);

                        if (faces.Any())
                        {
                            // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            emotionResultLabel.Text = faces.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;

                            ((Button)sender).IsEnabled = true;
                            activityIndicator.IsRunning = false;
                        }
                        photo.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
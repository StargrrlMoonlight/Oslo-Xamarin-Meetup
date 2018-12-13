using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XamAI.Exceptions;
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
                photo = await CrossMedia.Current.PickPhotoAsync();
                
                //photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                //{
                //    Name = "emotion.jpg",
                //    PhotoSize = PhotoSize.Small
                //});

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
                    var faceAttributes = new FaceAttributeType[] { FaceAttributeType.Emotion };
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
            catch (FaceAPIException fx)
            {
                Debug.WriteLine(fx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
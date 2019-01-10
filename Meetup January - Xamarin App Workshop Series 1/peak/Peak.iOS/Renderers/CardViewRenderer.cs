using System;
using System.ComponentModel;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using Peak.Controls;
using Peak.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CardView), typeof(CardViewRenderer))]
namespace Peak.iOS.Renderers
{
    public class CardViewRenderer : VisualElementRenderer<ContentView>
    {
        private CardView _element;

        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                return;

            _element = (CardView)e.NewElement;

            SetupCorners();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            float cornerRadius = _element.CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; // default corner radius

            if (_element != null && _element.HasBackgroundGradient) // perform initial setup
            {
                var gradientLayer = new CAGradientLayer
                {
                    CornerRadius = cornerRadius,
                    Frame = NativeView.Bounds,
                    Colors = new CGColor[] { _element.GradientStartColor.ToCGColor(), _element.GradientEndColor.ToCGColor() }
                };

                NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }
        }

        void SetupCorners()
        {
            float cornerRadius = ((CardView)Element).CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; // default corner radius

            Layer.CornerRadius = cornerRadius;

            if (((CardView)Element).HasShadow)
            {
                Layer.ShadowRadius = 10;
                Layer.ShadowColor = UIColor.Black.CGColor;
                Layer.ShadowOpacity = 0.4f;
                Layer.ShadowOffset = new SizeF();

                Layer.RasterizationScale = UIScreen.MainScreen.Scale;
                Layer.ShouldRasterize = true;
            }
        }

        private CardView FormsControl
        {
            get { return Element as CardView; }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CardView.CornerRadiusProperty.PropertyName)
                this.Layer.CornerRadius = (float)FormsControl.CornerRadius;

            if (e.PropertyName == CardView.HasShadowProperty.PropertyName)
            {
                if (FormsControl.HasShadow)
                {
                    Layer.ShadowRadius = 10;
                    Layer.ShadowColor = UIColor.Black.CGColor;
                    Layer.ShadowOpacity = 0.4f;
                    Layer.ShadowOffset = new SizeF();

                    Layer.RasterizationScale = UIScreen.MainScreen.Scale;
                    Layer.ShouldRasterize = true;
                }
                else
                {
                    this.Layer.ShadowRadius = 0;
                    Layer.ShadowOpacity = 0;
                }
            }
        }
    }
}
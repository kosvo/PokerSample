using System;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace FormsSample
{
	public class App : Xamarin.Forms.Application
	{
		public App()
		{
			MainPage = new NavigationPage(new StartPage { });
		}
	}

	public class StartPage : ContentPage
	{
		public StartPage()
		{
			var b = new Button { Text = "Launch sample" };
			b.Clicked += (sender, e) => Navigation.PushAsync(new UrhoPage());
			Content = new StackLayout { Children = { b }, VerticalOptions = LayoutOptions.Center };
		}
	}


	public class UrhoPage : ContentPage
	{
		UrhoSurface urhoSurface;
		Cards urhoApp;
		Slider selectedBarSlider;

		public UrhoPage()
		{
			urhoSurface = new UrhoSurface();
			urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;


			Slider rotationSlider = new Slider(0, 500, 250);
			rotationSlider.ValueChanged += (s, e) => urhoApp?.Rotate((float)(e.NewValue - e.OldValue));


			Title = " UrhoSharp  Poker Sample";
			Content = new StackLayout
			{
				Padding = new Thickness(12, 12, 12, 40),
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					urhoSurface,
					new Label { Text = "ROTATION::" },
					rotationSlider
				}
			};
		}

		protected override void OnDisappearing()
		{
			UrhoSurface.OnDestroy();
			base.OnDisappearing();
		}


		protected override async void OnAppearing()
		{
			StartUrhoApp();

		}

		async void StartUrhoApp()
		{
			urhoApp = await urhoSurface.Show<Cards>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
		}
	}
}

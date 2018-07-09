using Demo_ReactiveUI_SearchDemo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Demo_ReactiveUI_SearchDemo
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            var bootstrapper = new AppBootsrapper();
            MainPage = bootstrapper.CreateMainPage(); 
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

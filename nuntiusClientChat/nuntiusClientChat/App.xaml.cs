﻿using Xamarin.Forms;
using nuntiusClientChat.Controller;
using LocalNotifications;

namespace nuntiusClientChat
{
	public partial class App : Application
	{
		int appOpend = 0;
		public App()
		{
			InitializeComponent();
			NetworkController.NagTimerRun = false;

			if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
			{
				DependencyService.Get<INotificationManager>().Initialize();
			}

			if (Device.RuntimePlatform == Device.UWP)
			{
				NetworkController.SendLoginRequestAsync("WIN","WIN");				
			}

			if (Controller.UserController.LogedInUser != null)
			{
				MainPage = new NavigationPage(new ChatSelectionPage());
			}
			else
			{
				MainPage = new LoginRegisterPage();
			}

		}

		protected override void OnStart()
		{
			appOpend++;
			if (appOpend == 1)
			{
				StorageController.Loade = true;
			}
			else
			{
				StorageController.Loade = false;
			}
		}

		protected override void OnSleep()
		{
			MainPage.Navigation.PopToRootAsync();
			StorageController.SaveData();
		}

		protected override void OnResume()
		{
			//if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
			//{
			//	StorageController.LoadeData();
			//}
		}
		

	}
}

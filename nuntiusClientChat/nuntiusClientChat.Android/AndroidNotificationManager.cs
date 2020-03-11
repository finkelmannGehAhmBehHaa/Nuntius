﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using nuntiusClientChat.Droid;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(nuntiusClientChat.Droid.AndroidNotificationManager))]
namespace nuntiusClientChat.Droid
{
	public class AndroidNotificationManager : INotificationManager
	{
		private const string channelId = "default";
		private const string channelName = "Default";
		private const string channelDescription = "The default channel for notifications.";
		private const int pendingIntentId = 0;

		public const string TitleKey = "title";
		public const string MessageKey = "message";
		private bool channelInitialized = false;
		private int messageId = -1;
		private NotificationManager manager;

		public event EventHandler NotificationReceived;

		public void Initialize()
		{
			CreateNotificationChannel();
		}

		public int ScheduleNotification(string title, string message)
		{
			if (!channelInitialized)
			{
				CreateNotificationChannel();
			}

			messageId++;

			Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
			intent.PutExtra(TitleKey, title);
			intent.PutExtra(MessageKey, message);

			PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
				.SetContentIntent(pendingIntent)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.notification_icon_background))
				.SetSmallIcon(Resource.Drawable.notification_icon_background)
				.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

			Notification notification = builder.Build();
			manager.Notify(messageId, notification);

			return messageId;
		}

		public void ReceiveNotification(string title, string message)
		{
			var args = new NotificationEventArgs()
			{
				Title = title,
				Message = message,
			};
			NotificationReceived?.Invoke(null, args);
		}

		private void CreateNotificationChannel()
		{
			manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var channelNameJava = new Java.Lang.String(channelName);
				var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
				{
					Description = channelDescription
				};
				manager.CreateNotificationChannel(channel);
			}

			channelInitialized = true;
		}
	}
}
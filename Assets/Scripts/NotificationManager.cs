using UnityEngine;
using Unity.Notifications.Android;
using System;
using System.Collections.Generic;

/// <summary>
/// Generated by GPT，用于管理通知
/// </summary>
public class NotificationManager : MonoBehaviour
{
    private const string channelId = "item_reminder";
    private const string channelName="Item Reminder";
    private const string channelDescription="提醒带全物品的App的通知。";
    private List<int> notificationIds = new List<int>();

    void Start()
    {
        InitializeAndroidNotifications();
        ScheduleNotification("提醒带全物品！","你今天要带666个物品",DateTime.Today+new TimeSpan(21));
    }

    private void InitializeAndroidNotifications()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = channelName,
            Importance = Importance.High,
            Description = channelDescription,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public int ScheduleNotification(string title, string text, DateTime fireTime)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = text,
            FireTime = fireTime,
        };

        int id = AndroidNotificationCenter.SendNotification(notification, channelId);
        notificationIds.Add(id);
        return id;
    }

    public void CancelNotification(int id)
    {
        AndroidNotificationCenter.CancelScheduledNotification(id);
        notificationIds.Remove(id);
    }

    public void CancelAllNotifications()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        notificationIds.Clear();
    }

    public void UpdateNotification(int id, string newTitle, string newText, DateTime newFireTime)
    {
        CancelNotification(id);
        ScheduleNotification(newTitle, newText, newFireTime);
    }
    public NotificationStatus CheckNotificationStatus(int id)
    {
        return AndroidNotificationCenter.CheckScheduledNotificationStatus(id);
    }
}
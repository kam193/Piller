using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Piller.Services;
using System.Threading.Tasks;
using Piller.Data;
using Android.Media;
using Java.Util;
using Piller.Droid.Extensions;

namespace Piller.Droid.Services
{
    public class AndroidNotificationService : INotificationService
    {
        private Context context;

        public AndroidNotificationService(Context context)
        {
            this.context = context;
        }

        public void CancelNotification(int id)
        {
            var alarmManager = (AlarmManager)this.context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(this.context, typeof(NotificationPublisher));
            PendingIntent alarmnIntent = PendingIntent.GetBroadcast(this.context, id, intent, 0);
            alarmManager.Cancel(alarmnIntent);
        }

        public Task<List<int>> ScheduleNotification(Data.MedicationDosage medication)
        {
            var task = new TaskFactory().StartNew(() =>
            {
                var listNotifyIds = new List<int>();
                int notifyNumber = 0;

                foreach (var day in medication.Days.GetListDaysInInts())
                {
                    foreach (var time in medication.DosageHours)
                    {
                        Intent notifyIntent = new Intent(this.context, typeof(NotificationPublisher));
                        var notification = this.BuildNotification(medication);
                        notification.Defaults |= NotificationDefaults.Lights | NotificationDefaults.Sound | NotificationDefaults.Vibrate;

                        var requestId = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                        listNotifyIds.Add(requestId);

                        notifyIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notifyNumber++);
                        notifyIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medication.Id.GetValueOrDefault(0));
                        notifyIntent.PutExtra(NotificationPublisher.ALARM_REQUEST, requestId);
                        notifyIntent.PutExtra(NotificationPublisher.NOTIFICATION, notification);

                        PendingIntent pendingIntent = PendingIntent.GetBroadcast(this.context, requestId, notifyIntent, PendingIntentFlags.CancelCurrent);

                        var firingCal = Calendar.Instance;
                        var currentCal = Calendar.Instance;

                        var test = firingCal.Get(CalendarField.DayOfWeek);
                        while (firingCal.Get(CalendarField.DayOfWeek) != day)
                            firingCal.Add(CalendarField.DayOfMonth, 1);
                        test = firingCal.Get(CalendarField.DayOfWeek);

                        firingCal.Set(CalendarField.HourOfDay, time.Hours);
                        firingCal.Set(CalendarField.Minute, time.Minutes); 
                        firingCal.Set(CalendarField.Second, 0);
                        if (firingCal.CompareTo(currentCal) < 0)
                        {
                            firingCal.Add(CalendarField.DayOfMonth, 7);
                        }
                        var triggerTime = firingCal.TimeInMillis;

                        AlarmManager alarmManager = (AlarmManager)this.context.GetSystemService(Context.AlarmService);
                        alarmManager.SetRepeating(AlarmType.RtcWakeup, triggerTime, AlarmManager.IntervalDay * 7, pendingIntent);
                    }
                }
                return listNotifyIds;
            });

            return task;
        }

        private Notification BuildNotification(MedicationDosage medication)
        {
            var builder = new Notification.Builder(this.context);
            builder.SetContentTitle(medication.Name);
            builder.SetContentText(medication.Dosage);
            builder.SetSmallIcon(Resource.Drawable.Icon);

            builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
            builder.SetPriority((int)NotificationPriority.High);
            builder.SetVisibility(NotificationVisibility.Public); // visible on locked screen
            return builder.Build();
        }

        public void CancelNotifications(IEnumerable<int> ids)
        {
            foreach (var id in ids)
                CancelNotification(id);
        }

    }
}
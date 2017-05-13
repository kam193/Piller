
using Android.App;
using Android.Content;

namespace Piller.Droid
{
    [BroadcastReceiver]
    public class NotificationPublisher : BroadcastReceiver
    {
        /// <summary>
        /// ID (numer) pojedynczego powiadomienia dot. leku (np. powiadomienia o przyjmowaniu we wtorku o 12)
        /// </summary>
        public static string NOTIFICATION_ID = "notification-id";
        /// <summary>
        /// ID leku w bazie aplikacji
        /// </summary>
        public static string MEDICATION_ID = "medication-id";
        /// <summary>
        /// RequestID uzywane przez AlarmManager
        /// </summary>
        public static string ALARM_REQUEST = "alarm-request";
        public static string NOTIFICATION = "notification";

        public override void OnReceive(Context context, Intent intent)
        {
            NotificationManager notifyManager = 
                (NotificationManager)context.GetSystemService(Context.NotificationService);

            var notification = intent.GetParcelableExtra(NOTIFICATION) as Notification;

            if (notification != null)
            {
                var id = intent.GetIntExtra(NOTIFICATION_ID, 0);
                var tag = intent.GetIntExtra(MEDICATION_ID, 0).ToString();
                notifyManager.Notify(tag, id, notification);
            }
            else
            {
                // TODO: actions
            }
        }
    }
}
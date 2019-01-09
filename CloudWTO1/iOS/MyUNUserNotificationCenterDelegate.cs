using System;
using ObjCRuntime;
using UserNotifications;
namespace CloudWTO.iOS
{
    public class MyUNUserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert|UNNotificationPresentationOptions.Badge|UNNotificationPresentationOptions.Sound);
            
        }


       



    }
}

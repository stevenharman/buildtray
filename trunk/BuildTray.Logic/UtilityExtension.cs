using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using BuildTray.Logic.Entities;

namespace BuildTray.Logic
{
    public static class UtilityExtension
    {
        public static int IndexAfter(this string value, string search, int start)
        {
            return value.IndexOf(search, start) + search.Length;
        }

        public static int IndexAfter(this string value, string search)
        {
            return value.IndexAfter(search, 0);
        }

        public static string ToDisplay(this TimeSpan timeSpan)
        {
            string result = string.Empty;
            if (timeSpan.Days > 0)
                result = timeSpan.Days + " day" + (timeSpan.Days > 1 ? "s " : " ");
            if (timeSpan.Hours > 0)
                result += timeSpan.Hours + " hour" + (timeSpan.Hours > 1 ? "s " : " ");
            if (timeSpan.Minutes > 0)
                result += timeSpan.Minutes + " minute" + (timeSpan.Minutes > 1 ? "s " : " ");
            if (timeSpan.Seconds > 0)
                result += timeSpan.Seconds + " second" + (timeSpan.Seconds > 1 ? "s" : "");
            return result;
        }

        public static void Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }

        public static bool CanConvert(this BuildStatus status)
        {
            return !(status == BuildStatus.NotStarted || status == BuildStatus.Stopped);
        }

        public static TrayStatus? ToTrayStatus(this BuildStatus status, TrayStatus currentStatus)
        {
            switch(status)
            {
                default:
                    return null;
                case BuildStatus.Failed:
                case BuildStatus.PartiallySucceeded:
                    return TrayStatus.Failure;
                case BuildStatus.InProgress:
                    if (currentStatus == TrayStatus.Success)
                        return TrayStatus.SuccessInProgress;
                    if (currentStatus == TrayStatus.Failure)
                        return TrayStatus.FailureInProgress;
                    return currentStatus;
                case BuildStatus.Succeeded:
                    return TrayStatus.Success;
            }
        }

        public static TrayStatus Combine(this TrayStatus status, TrayStatus newStatus)
        {
            bool inProgress = status == TrayStatus.FailureInProgress
                              || newStatus == TrayStatus.FailureInProgress
                              || status == TrayStatus.SuccessInProgress
                              || newStatus == TrayStatus.SuccessInProgress;

            bool failure = status == TrayStatus.Failure
                           || status == TrayStatus.FailureInProgress
                           || newStatus == TrayStatus.Failure
                           || newStatus == TrayStatus.FailureInProgress;

            bool success = status == TrayStatus.Success
                           || status == TrayStatus.SuccessInProgress
                           || newStatus == TrayStatus.SuccessInProgress
                           || newStatus == TrayStatus.Success;

            if (inProgress && failure)
                return TrayStatus.FailureInProgress;
            if (failure)
                return TrayStatus.Failure;
            if (inProgress && success)
                return TrayStatus.SuccessInProgress;
            return TrayStatus.Success;
        }

        public static int GetBuildNumber(this IBuildDetail detail)
        {
            string path = detail.Uri.AbsolutePath.Substring(detail.Uri.AbsolutePath.LastIndexOf(@"/") + 1);
            return Convert.ToInt32(path);
        }

        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler == null)
                return;

            handler.Invoke(sender, args);
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T args)
            where T : EventArgs
        {
            if (handler == null)
                return;

            handler.Invoke(sender, args);
        }

        public static BuildStatuses ToStatus(this BuildStatus status)
        {
            if (status == BuildStatus.InProgress || status == BuildStatus.NotStarted)
                return BuildStatuses.InProgress;
            if (status == BuildStatus.PartiallySucceeded || status == BuildStatus.Failed || status == BuildStatus.Stopped)
                return BuildStatuses.Failed;
            return BuildStatuses.Passed;
        }

        public static Build ToBuild(this IBuildDetail detail)
        {
            var build = new Build
                            {
                                BuildNumber = detail.GetBuildNumber(),
                                DropLocation = detail.DropLocation,
                                FinishTime =
                                    (detail.FinishTime != DateTime.MinValue ? (DateTime?) detail.FinishTime : null),
                                StartTime = detail.StartTime,
                                LogLocation = detail.LogLocation,
                                RequestedFor = detail.RequestedFor,
                                Status = detail.Status.ToStatus()
                            };
            return build;
        }
    }
}

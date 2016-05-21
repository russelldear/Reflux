using System;

namespace Reflux.Model
{
    public static class Converter
    {
        public static Reminder ToReminder(this MessageSearchResult searchResult, DateTime reminderDateTime)
        {
            return new Reminder
            {
                Id = searchResult.Id,
                UserId = searchResult.UserId,
                Event = searchResult.Event,
                ThreadId = searchResult.ThreadId,
                FlowId = searchResult.FlowId,
                CreatedAt = searchResult.CreatedAt,
                EditedAt = searchResult.EditedAt,
                OriginalFlowName = searchResult.OriginalFlowName,
                FlowName = searchResult.FlowName,
                UserName = searchResult.UserName,
                Content = searchResult.Content,
                ReminderDateTime = reminderDateTime,
            };
        }
    }
}

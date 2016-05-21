using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Reflux.DataAccess;
using Reflux.Model;

namespace Reflux.Services
{
    public class ReminderService
    {
        private readonly string _apiKey;
        private readonly FlowMessageStore _flowMessageStore;
        private readonly TagService _tagService;

        public ReminderService(string apiKey)
        {
            _apiKey = apiKey;
            _flowMessageStore = new FlowMessageStore();
            _tagService = new TagService(apiKey);
        }

        public void SendReminder(Reminder reminder)
        {
            EnsureConversationVisibility(reminder.UserId);

            var url = $"{Constants.BaseUrl}{"private/"}{reminder.UserId}{"/messages"}";

            var message = $"{{ \"event\": \"message\", \"content\": \"Here you go: {reminder.Url}\" }}";

            var responseText = Internet.Post(url, _apiKey, message);

            var result = JsonConvert.DeserializeObject<MessageSearchResult>(responseText);

            if (result.Id != null)
            {
                _flowMessageStore.Add(reminder.FlowId, reminder.Id);
                _tagService.AddTag(reminder.Id, reminder.OriginalFlowName, "reminder-set");
            }
        }

        private void EnsureConversationVisibility(string userId)
        {
            try
            {
                var url = $"{Constants.BaseUrl}{"private/"}{userId}";

                Internet.Put(url, _apiKey, "{ \"open\": true }");
            }
            catch
            {
                // Probably failed to create a conversation with itself - surprise!
            }
        }
    }
}

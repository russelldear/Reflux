using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Reflux.Model;

namespace Reflux.Services
{
    public class ReminderService
    {
        private readonly string _apiKey;
        private readonly TagService _tagService;

        public ReminderService(string apiKey)
        {
            _apiKey = apiKey;
            _tagService = new TagService(apiKey);
        }

        public void SendReminder(Reminder reminder)
        {
            EnsureConversationVisibility(reminder.UserId);

            var url = $"{Constants.ApiUrl}{"private/"}{reminder.UserId}{"/messages"}";

            var message = $"{{ \"event\": \"message\", \"content\": \"Here you go: {reminder.Url}\" }}";

            var responseText = Internet.Post(url, _apiKey, message);

            var result = JsonConvert.DeserializeObject<MessageSearchResult>(responseText);

            if (result.Id != null)
            {
                _tagService.AddTag(reminder.Id, reminder.OriginalFlowName, Constants.RemindMeSent);
            }
        }

        private void EnsureConversationVisibility(string userId)
        {
            try
            {
                var url = $"{Constants.ApiUrl}{"private/"}{userId}";

                Internet.Put(url, _apiKey, "{ \"open\": true }");
            }
            catch
            {
                // Probably failed to create a conversation with itself - surprise!
            }
        }
    }
}

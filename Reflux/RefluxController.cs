using System;
using Reflux.Model;
using Reflux.Services;
using System.Configuration;
using System.Linq;

namespace Reflux
{
    public static class RefluxController
    {
        public static void Go()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            
            try
            {
                var apiKey = ConfigurationManager.AppSettings["FlowdockAPIKey"];
                var knownCommands = ConfigurationManager.AppSettings["KnownCommands"].Split('|');

                var searchService = new SearchService(apiKey);
                var reminderService = new ReminderService(apiKey);
                var tagService = new TagService(apiKey);

                var searchResults = searchService.Search(Constants.RemindMe);

                searchResults = searchResults.Where(r => !r.Tags.Contains(Constants.RemindMeSent)).ToList();

                foreach (var searchResult in searchResults)
                {
                    var reminderDateTime = Parse.Time(searchResult.Content, searchResult.CreatedAt);

                    if (reminderDateTime.HasValue)
                    {
                        tagService.AddTag(searchResult.Id, searchResult.OriginalFlowName, Constants.RemindMeWillSend);

                        if (reminderDateTime.Value < DateTime.UtcNow)
                        {
                            var reminder = searchResult.ToReminder(reminderDateTime.Value);

                            reminderService.SendReminder(reminder);
                        }
                    }
                    else
                    {
                        if (!knownCommands.Any(c => searchResult.Content.Contains(c)))
                        {
                            tagService.AddTag(searchResult.Id, searchResult.OriginalFlowName, Constants.RemindMeUnknown);
                        }
                    }
                }

                logger.Info(searchResults.Count + " search results returned from " + searchResults.OrderBy(r => r.CreatedAt).FirstOrDefault()?.CreatedAtString + " onward.");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
            }
        }
    }
}

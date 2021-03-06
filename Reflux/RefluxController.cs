﻿using System;
using Reflux.Model;
using Reflux.Services;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using StatsdClient;

namespace Reflux
{
    public static class RefluxController
    {
        public static void Go()
        {
            RefluxStatsdConfig.Configure();

            var logger = NLog.LogManager.GetCurrentClassLogger();

            using (DogStatsd.StartTimer("RunRemindMe"))
            {
                try
                {
                    var apiKey = ConfigurationManager.AppSettings["FlowdockAPIKey"];

                    var searchService = new SearchService(apiKey);
                    var reminderService = new ReminderService(apiKey);
                    var tagService = new TagService(apiKey);

                    var searchResults = searchService.Search(Constants.RemindMe);

                    searchResults = searchResults.Where(r => !r.Tags.Contains(Constants.RemindMeSent)).ToList();

                    DogStatsd.Gauge("RemindMe.SearchResults", searchResults.Count);
                    logger.Info(searchResults.Count + " search results returned from " + searchResults.OrderBy(r => r.CreatedAt).FirstOrDefault()?.CreatedAtString + " onward.");

                    List<Exception> errors = new List<Exception>();

                    foreach (var searchResult in searchResults)
                    {
                        try
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
                        }
                        catch (Exception ex)
                        {
                            // Don't break for everyone if one reminder doesn't work.
                            errors.Add(ex);
                        }
                    }

                    DogStatsd.Gauge("RemindMe.Errors", errors.Count);

                    if (errors.Count > 0)
                    {
                        var errorText = string.Join(Environment.NewLine, errors.Select(e => e.Message).ToList());
                        throw new Exception("The following errors were encountered while reminding: " + errorText);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message + ex.StackTrace);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using static System.Double;

namespace Reflux
{
    public static class Parse
    {
        public static int Second = 1;
        public static int Minute = Second * 60;
        public static int Hour = Minute * 60;
        public static int Day = Hour * 24;
        public static int Week = Day * 7;
        public static int Month = Day * 30;
        public static int Year = Day * 365;

        public static DateTime? Time(string content, DateTime createdDateTime)
        {
            var result = (DateTime?)null;

            if (content.ToUpper().Contains(Constants.RemindMe.ToUpper()))
            {
                var reminderSeconds = GetReminderSeconds(content);

                if (reminderSeconds.HasValue)
                {
                    result = createdDateTime.AddSeconds(reminderSeconds.Value);
                }
            }

            return result;
        }

        private static double? GetReminderSeconds(string content)
        {
            var trailingText = content.Substring(content.IndexOf(Constants.RemindMe) + Constants.RemindMe.Length);

            foreach (var phrase in KnownPhrases.Keys)
            {
                if (trailingText.IndexOf(phrase) > -1)
                {
                    return KnownPhrases[phrase];
                }
            }

            var bits = new List<string>(trailingText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            if (bits.Count < 1)
            {
                return null;
            }

            for (var i = 0; i < bits.Count; i++)
            {
                try
                {
                    var quantity = Parse(bits[i]);

                    break;
                }
                catch (Exception)
                {
                    bits.RemoveAt(i);

                    if (bits.Count > 0)
                    {
                        i = i - 1;
                    }
                    else return null;
                }
            }

            double quantity;

            if (!TryParse(bits[0], out quantity))
            {
                return null;
            }

            double multiplier;

            if (!KnownUnits.ContainsKey(bits[1]))
            {
                return null;
            }
            else
            {
                multiplier = KnownUnits[bits[1]];
            }

            return quantity*multiplier;
        }
        
        private static readonly Dictionary<string, int> KnownUnits = new Dictionary<string, int>
        {
            { "minute", Minute },
            { "minutes", Minute },
            { "mins", Minute },
            { "hour", Hour },
            { "hours", Hour },
            { "day", Day },
            { "days", Day },
            { "week", Week },
            { "weeks", Week },
            { "month", Month },
            { "months", Month },
            { "year", Year },
            { "years", Year }
        };

        private static readonly Dictionary<string, int> KnownPhrases = new Dictionary<string, int>
        {
            { "tomorrow", Day },
            { "next week", Week },
            { "next month", Month },
            { "next year", Year },
            { "in a week", Week }
        };
    }
}
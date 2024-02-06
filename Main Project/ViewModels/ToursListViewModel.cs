using Main_Project.Models;

namespace Main_Project.ViewModels
{
    public class ToursListViewModel
    {
        public List<Tour> Tours { get; set; }

        public List<int> PurchasedToursIds { get; set; }

        public string GetTimeLeft(DateTime tourDate)
        {
            TimeSpan timeLeft = tourDate - DateTime.Now;
            if (timeLeft.TotalSeconds > 0)
            {
                return $"{timeLeft.Days} days, {timeLeft.Hours} hours, {timeLeft.Minutes} minutes left";
            }
            else
            {
                return "The date has already passed.";
            }
        }
    }
}
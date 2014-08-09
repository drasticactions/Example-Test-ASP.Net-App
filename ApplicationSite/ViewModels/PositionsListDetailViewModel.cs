using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class PositionsListDetailViewModel
    {
        public Positions Position { get; set; }

        public bool HasAlreadyApplied { get; set; }

        public bool IsLoggedIn { get; set; }

        public void MapTo(Positions position, bool hasApplied, bool isLoggedIn)
        {
            Position = position;
            HasAlreadyApplied = hasApplied;
            IsLoggedIn = isLoggedIn;
        }
    }
}
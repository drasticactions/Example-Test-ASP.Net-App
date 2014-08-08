using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            this.Position = position;
            this.HasAlreadyApplied = hasApplied;
            this.IsLoggedIn = isLoggedIn;
        }
    }
}
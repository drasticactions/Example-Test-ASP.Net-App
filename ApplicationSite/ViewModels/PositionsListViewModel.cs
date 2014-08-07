using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class PositionsListViewModel
    {
        public List<Positions> PositionsList { get; set; }

        public void MapTo(List<Positions> positions)
        {
            this.PositionsList = positions;
        }
    }
}
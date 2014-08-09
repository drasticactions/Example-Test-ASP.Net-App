using System.Collections.Generic;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class PositionsListViewModel
    {
        public List<Positions> PositionsList { get; set; }

        public void MapTo(List<Positions> positions)
        {
            PositionsList = positions;
        }
    }
}
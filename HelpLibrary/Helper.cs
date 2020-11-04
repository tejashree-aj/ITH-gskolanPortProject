using DataModels;
using PortAdministrationProject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpLibrary
{
    public static class Helper
    {
        public static List<ParkingPosition> GetParkingList(this int parkingCount)
        {
            return Enumerable.Range(1, parkingCount).Select(x => new ParkingPosition
            {
                ParkingId = x,
                BoatIds = new List<string>()//empty list
            }).ToList();
        }
    }
}

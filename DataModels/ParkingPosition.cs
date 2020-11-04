using PortAdministrationProject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    //Data Model class for each parking block
    public class ParkingPosition
    {
        public int ParkingId { get; set; }
        public List<String> BoatIds { get; set; }
    }
}

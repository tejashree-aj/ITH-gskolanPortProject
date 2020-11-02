using System;
using System.Collections.Generic;
using System.Text;

namespace PortAdministrationProject
{
    public class SailBoat : BaseClassBoat
    {
        public int BoatLength { get; set; }

        public SailBoat()
        {

        }
        public SailBoat(TypeOfBoat typeOfBoat, string id, decimal boatWeight, double maxSpeed, int boatLength,
            List<int> parkingId, DateTime deparureTime, DateTime arrivalTime) : base(typeOfBoat, id, boatWeight, maxSpeed, parkingId, deparureTime, arrivalTime)
        {
            BoatLength = boatLength;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PortAdministrationProject
{
    public class RowingBoat : BaseClassBoat
    {
        public int NumberOfPassengers { get; set; }

        public RowingBoat()
        {

        }

        public RowingBoat(TypeOfBoat typeOfBoat, string id, decimal boatWeight, double maxSpeed, int numberOfPassengers,
            List<int> parkingId, DateTime deparureTime, DateTime arriveAt) : base(typeOfBoat, id, boatWeight, maxSpeed, parkingId, deparureTime,arriveAt)
        { 
            NumberOfPassengers = numberOfPassengers;
        }
    }
}

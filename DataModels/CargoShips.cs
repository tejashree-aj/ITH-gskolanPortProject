using System;
using System.Collections.Generic;
using System.Text;

namespace PortAdministrationProject
{
    public class CargoShips : BaseClassBoat
    {
        public int NumberOfContainers { get; set; }

        public CargoShips()
        {

        }

        public CargoShips(TypeOfBoat typeOfBoat, string id, decimal boatWeight, double maxSpeed, int numberOfContainers,
            List<int> parkingId, DateTime deparureTime, DateTime arrivalTime) : base(typeOfBoat, id, boatWeight, maxSpeed, parkingId, deparureTime, arrivalTime)
        {
            NumberOfContainers = numberOfContainers;
        }
    }
}

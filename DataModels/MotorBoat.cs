using System;
using System.Collections.Generic;
using System.Text;

namespace PortAdministrationProject
{
    public class MotorBoat : BaseClassBoat
    {
        public int MaxHorsePower { get; set; }

        public MotorBoat()
        {
                
        }
        public MotorBoat(TypeOfBoat typeOfBoat, string id, decimal boatWeight, double maxSpeed, int maxHorsePower,
            List<int> parkingId, DateTime deparureTime, DateTime arrivalTime) : base(typeOfBoat, id, boatWeight, maxSpeed, parkingId, deparureTime, arrivalTime)
        {
            MaxHorsePower = maxHorsePower; 
        }
    }
}

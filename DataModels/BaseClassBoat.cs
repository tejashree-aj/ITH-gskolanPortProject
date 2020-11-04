using System;
using System.Collections.Generic;
using System.Text;

namespace PortAdministrationProject
{
    public enum TypeOfBoat
    {
        None,
        RowingBoat,
        MotorBoat,
        SailBoat,
        CargoShip
    }
    public class BaseClassBoat 
    {
        public TypeOfBoat TypeOfBoat { get; set; }
        public string IDNumber { get; set; }
        public decimal BoatWeight { get; set; }
        public double MaximumSpeed { get; set; }
        public List<int> ParkingId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string Miscellaneous { get; set; }

        public BaseClassBoat()
        {

        }
        public BaseClassBoat(TypeOfBoat typeOfBoat, string id, decimal boatWeight, double maxSpeed, List<int> parkingId, DateTime deparureTime, DateTime arrivalTime)
        {
            TypeOfBoat = typeOfBoat;
            IDNumber = id;
            BoatWeight = boatWeight;
            MaximumSpeed = maxSpeed;
            ParkingId = parkingId;
            DepartureTime = deparureTime;
            ArrivalTime = arrivalTime;
        }
    }
}

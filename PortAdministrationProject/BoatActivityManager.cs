using DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PortAdministrationProject
{
    public static class BoatActivityManager
    {
        public static void AddBoatToList(List<BaseClassBoat> boats)
        {
            string filePath = @"C:\ITHogskolanProjects\ITHögskolanPortProject\PortAdministrationProject\BoatPortData\BoatData.csv";

            int rowCount = 0;
            foreach (string boat in File.ReadLines(filePath, System.Text.Encoding.UTF7))
            {
                string[] boatData = boat.Split(',');

                switch (boatData[0])
                {
                    case "RowingBoat":

                        RowingBoat rb = new RowingBoat();
                        rb.TypeOfBoat = (TypeOfBoat)Enum.Parse(typeof(TypeOfBoat), boatData[0]);
                        rb.IDNumber = boatData[1];
                        rb.BoatWeight = decimal.Parse(boatData[2]);
                        rb.MaximumSpeed = int.Parse(boatData[3]);
                        rb.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();

                        if (boatData[5] != " ")
                        {
                            rb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != " ")
                        {
                            rb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        rb.NumberOfPassengers = int.Parse(boatData[7]);

                        boats.Add(rb);
                        break;

                    case "MotorBoat":
                        MotorBoat mb = new MotorBoat();
                        mb.TypeOfBoat = (TypeOfBoat)Enum.Parse(typeof(TypeOfBoat), boatData[0]);
                        mb.IDNumber = boatData[1];
                        mb.BoatWeight = decimal.Parse(boatData[2]);
                        mb.MaximumSpeed = int.Parse(boatData[3]);
                        mb.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();

                        if (boatData[5] != " ")
                        {
                            mb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != " ")
                        {
                            mb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        mb.MaxHorsePower = int.Parse(boatData[7]);

                        boats.Add(mb);
                        break;

                    case "SailBoat":
                        SailBoat sb = new SailBoat();
                        sb.TypeOfBoat = (TypeOfBoat)Enum.Parse(typeof(TypeOfBoat), boatData[0]);
                        sb.IDNumber = boatData[1];
                        sb.BoatWeight = decimal.Parse(boatData[2]);
                        sb.MaximumSpeed = int.Parse(boatData[3]);
                        sb.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();

                        if (boatData[5] != " ")
                        {
                            sb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != " ")
                        {
                            sb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        sb.BoatLength = int.Parse(boatData[7]);

                        boats.Add(sb);
                        break;

                    case "CargoShip":
                        CargoShips cs = new CargoShips();
                        cs.TypeOfBoat = (TypeOfBoat)Enum.Parse(typeof(TypeOfBoat), boatData[0]);
                        cs.IDNumber = boatData[1];
                        cs.BoatWeight = decimal.Parse(boatData[2]);
                        cs.MaximumSpeed = int.Parse(boatData[3]);
                        cs.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();

                        if (boatData[5] != " ")
                        {
                            cs.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != " ")
                        {
                            cs.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        cs.NumberOfContainers = int.Parse(boatData[7]);

                        boats.Add(cs);
                        break;

                    default:
                        break;
                }
                rowCount++;
            }
        }

        public static List<BaseClassBoat> ParkingBoatList(this List<BaseClassBoat> boats)
        {
            try
            {
                var random = new Random();

                List<BaseClassBoat> randomData = boats.Select(x => x).OrderBy(y => Guid.NewGuid()).Take(2).ToList();
                boats.RemoveAll(x => randomData.Any(y => y.IDNumber == x.IDNumber));

                foreach (BaseClassBoat parkingBoatData in randomData)
                {
                    parkingBoatData.ArrivalTime = DateTime.Now;
                    double maxSpeed = parkingBoatData.MaximumSpeed * 0.621;
                    parkingBoatData.MaximumSpeed = Math.Round(maxSpeed, 2);

                    switch (parkingBoatData.TypeOfBoat)
                    {
                        case TypeOfBoat.RowingBoat:

                            parkingBoatData.DepartureTime = parkingBoatData.ArrivalTime.Value.AddSeconds(5);
                            parkingBoatData.Miscellaneous = $"Passenger count - {((RowingBoat)parkingBoatData).NumberOfPassengers}";
                            
                            break;

                        case TypeOfBoat.MotorBoat:

                            parkingBoatData.DepartureTime = parkingBoatData.ArrivalTime.Value.AddSeconds(15);
                            parkingBoatData.Miscellaneous = $"Number of HorsePower - {((MotorBoat)parkingBoatData).MaxHorsePower}";
                            break;

                        case TypeOfBoat.SailBoat:

                            parkingBoatData.DepartureTime = parkingBoatData.ArrivalTime.Value.AddSeconds(20);
                            parkingBoatData.Miscellaneous = $"Boat Length - {((SailBoat)parkingBoatData).MaximumSpeed}";
                            break;

                        case TypeOfBoat.CargoShip:

                            parkingBoatData.DepartureTime = parkingBoatData.ArrivalTime.Value.AddSeconds(30);
                            parkingBoatData.Miscellaneous = $"Number of Containers - {((CargoShips)parkingBoatData).NumberOfContainers}";
                            break;

                        default:
                            break;
                    }
                }
                return randomData;

                
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static List<BaseClassBoat> MoveBoatBackToSeaList(this List<BaseClassBoat> parkedboats)
        {
            List<BaseClassBoat> backToSeaBoats = (from bts in parkedboats
                                                  where bts.DepartureTime < DateTime.Now
                                                  select bts).ToList();
            parkedboats.RemoveAll(x => backToSeaBoats.Any(y => y.IDNumber == x.IDNumber));

            foreach (BaseClassBoat backToSeaBoatData in backToSeaBoats)
            {
                 backToSeaBoatData.DepartureTime = null;
                backToSeaBoatData.ArrivalTime = null;
            }
            return backToSeaBoats;
        }

        public static List<BoatCountModelClass> CountingParkedBoats(List<BaseClassBoat> parkedboat)
        {
            var parkedBoatsSeperateCount = (from pb in parkedboat
                                     group pb by pb.TypeOfBoat into pbCount
                                     select new BoatCountModelClass
                                     {
                                         TypeOfBoat = pbCount.Key,
                                         Count = pbCount.Count(),
                                     }).ToList();
            return parkedBoatsSeperateCount;

        }

        //public static object CountingParkedBoats(List<BaseClassBoat> parkedboat)
        //{
        //    var parkedBoats = (from pb in parkedboat
        //                             group pb by pb.TypeOfBoat into pbCount
        //                             select new
        //                             {
        //                                 TypeOfBoat = pbCount.Key,
        //                                 Count = pbCount.Count(),
        //                             }).ToList();
        //    return parkedBoats;


        //}

        public static decimal TotalWeightOfParkedBoats (List<BaseClassBoat> parkedboat)
        {
            decimal toatlParkedBoatsWeight = (from boatWt in parkedboat
                                          select boatWt.BoatWeight).Sum();
            return toatlParkedBoatsWeight;
        }

        public static int AverageMaxSpeedOfAllBoats(List<BaseClassBoat> parkedboat)
        {
            int averageMaxSpeedOfParkedBoats = (int)(from boatSpeed in parkedboat
                                                select boatSpeed.MaximumSpeed).Average();
            return averageMaxSpeedOfParkedBoats;
        }





    }
}



//code to generate a csv file - 
//string filePath = @"C:\ITHogskolanProjects\ITHögskolanPortProject\PortAdministrationProject\BoatPortData\BoatData.csv";
//string delimiter = ",";


//string[][] output = new string[][]
//{
//                new string[]{"Col 1 Row 1", "Col 2 Row 1", "Col 3 Row 1" },
//                new string[]{"Col 1 Row 2", "Col 2 Row 2", "Col 3 Row 2" }
//};

//int length = output.GetLength(0);
//StringBuilder sb = new StringBuilder();
//for (int index = 0; index < length; index++)
//{
//    sb.Append(string.Join(delimiter, output[index]));
//    File.WriteAllText(filePath, sb.ToString());
//}


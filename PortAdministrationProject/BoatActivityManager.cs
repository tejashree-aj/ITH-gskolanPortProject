using DataModels;
using HelpLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PortAdministrationProject
{
    public static class BoatActivityManager
    {
        // list of each parking block
        static List<ParkingPosition> parkingPositionList;

        // generates parking positions for boats
        public static void GenerateParkingList(int parkingCount)
        {
            parkingPositionList = parkingCount.GetParkingList();
        }


        //boats move from boat list to parking list
        public static List<BaseClassBoat> MoveToParkingList(this List<BaseClassBoat> boats)
        {
            try
            {
                var random = new Random();

                //selects random boat from boat list
                List<BaseClassBoat> randomData = boats.Select(x => x).OrderBy(y => Guid.NewGuid()).Take(5).ToList();

                foreach (BaseClassBoat parkingBoatData in randomData)
                {

                    List<int> parkingIds = AssignParkingLocation(parkingBoatData.TypeOfBoat, parkingBoatData.IDNumber);

                    if (parkingIds != null)
                    {
                        boats.RemoveAll(x => parkingBoatData.IDNumber == x.IDNumber);

                        parkingBoatData.ParkingId = parkingIds;

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
                                parkingBoatData.Miscellaneous = $"Boat Length - {((SailBoat)parkingBoatData).BoatLength}";
                                break;

                            case TypeOfBoat.CargoShip:

                                parkingBoatData.DepartureTime = parkingBoatData.ArrivalTime.Value.AddSeconds(30);
                                parkingBoatData.Miscellaneous = $"Number of Containers - {((CargoShips)parkingBoatData).NumberOfContainers}";
                                break;

                            default:
                                break;
                        }
                    }
                }
                return randomData.Where(x => x.ParkingId != null).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //boats move from parking list to boat list
        public static List<BaseClassBoat> MoveBoatBackToSeaList(this List<BaseClassBoat> parkedboats)
        {
            List<BaseClassBoat> backToSeaBoats = (from bts in parkedboats
                                                  where bts.DepartureTime < DateTime.Now
                                                  select bts).ToList();
            parkedboats.RemoveAll(x => backToSeaBoats.Any(y => y.IDNumber == x.IDNumber));

            foreach (BaseClassBoat backToSeaBoatData in backToSeaBoats)
            {
                RemoveParkingIdOfBoatsFromParkingList(backToSeaBoatData.ParkingId, backToSeaBoatData.IDNumber);
                backToSeaBoatData.ParkingId = null;
                backToSeaBoatData.DepartureTime = null;
                backToSeaBoatData.ArrivalTime = null;
            }
            return backToSeaBoats;
        }


        //removing parking ids of boats to move them back to boat list
        private static void RemoveParkingIdOfBoatsFromParkingList(List<int> parkingIds, string boatIdNumber)
        {
            parkingPositionList.Where(x => parkingIds.Contains(x.ParkingId)).ToList().ForEach(x =>
            {
                x.BoatIds.Remove(boatIdNumber);
            });
        }

        //adding parking ids to boats in parking list
        private static void AddParkingIdOfBoatsToParkingList(List<int> parkingIds, string boatIdNumber, TypeOfBoat boatType)
        {
            parkingPositionList.Where(x => parkingIds.Contains(x.ParkingId)).ToList().ForEach(x =>
            {
                x.BoatIds.Add(boatIdNumber);
            });
        }



        //reports on boats
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

        public static decimal TotalWeightOfParkedBoats(List<BaseClassBoat> parkedboat)
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




        //boats from boat csv to boat list
        public static void AddBoatToList(List<BaseClassBoat> boats)
        {
            string filePath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BoatPortData\BoatData.csv");
            List<string> csvData = File.ReadLines(filePath, System.Text.Encoding.UTF7).ToList();
            ImportDataFromCSVFile(boats, csvData);
        }

        //TODO (boats from parking csv to parked boat list import function - pending)
        public static void AddParkedBoatToList(List<BaseClassBoat> parkedBoats)
        {
            string filePath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BoatPortData\ParkedBoatData.csv");
            List<string> csvData = File.ReadLines(filePath, System.Text.Encoding.UTF7).ToList();
            ImportDataFromCSVFile(parkedBoats, csvData);
        }

        //data from csv to lists
        private static void ImportDataFromCSVFile(List<BaseClassBoat> boats, List<string> csvData)
        {
            int rowCount = 0;
            foreach (string boat in csvData)
            {
                string[] boatData = boat.Split(',');

                switch (boatData[0])
                {
                    case "RowingBoat":

                        RowingBoat rb = new RowingBoat();
                        rb.TypeOfBoat = TypeOfBoat.RowingBoat;
                        rb.IDNumber = boatData[1];
                        rb.BoatWeight = decimal.Parse(boatData[2]);
                        rb.MaximumSpeed = double.Parse(boatData[3]);

                        if (boatData[4] != "")
                        {
                            rb.ParkingId = (boatData[4].Split('|').Select(s => int.Parse(s)).ToList());
                            // TODO
                            AddParkingIdOfBoatsToParkingList(rb.ParkingId, rb.IDNumber, rb.TypeOfBoat);
                        }

                        if (boatData[5] != "")
                        {
                            rb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != "")
                        {
                            rb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        rb.NumberOfPassengers = int.Parse(boatData[7].Replace("Passenger count - ", ""));

                        boats.Add(rb);
                        break;

                    case "MotorBoat":
                        MotorBoat mb = new MotorBoat();
                        mb.TypeOfBoat = TypeOfBoat.MotorBoat;
                        mb.IDNumber = boatData[1];
                        mb.BoatWeight = decimal.Parse(boatData[2]);
                        mb.MaximumSpeed = double.Parse(boatData[3]);
                        if (boatData[4] != "")
                        {
                            mb.ParkingId = (boatData[4].Split('|').Select(s => int.Parse(s)).ToList());
                            // TODO
                            AddParkingIdOfBoatsToParkingList(mb.ParkingId, mb.IDNumber, mb.TypeOfBoat);
                        }

                        if (boatData[5] != "")
                        {
                            mb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != "")
                        {
                            mb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        mb.MaxHorsePower = int.Parse(boatData[7].Replace("Number of HorsePower - ", ""));

                        boats.Add(mb);
                        break;

                    case "SailBoat":
                        SailBoat sb = new SailBoat();
                        sb.TypeOfBoat = TypeOfBoat.SailBoat;
                        sb.IDNumber = boatData[1];
                        sb.BoatWeight = decimal.Parse(boatData[2]);
                        sb.MaximumSpeed = double.Parse(boatData[3]);

                        if (boatData[4] != "")
                        {
                            sb.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();
                            // TODO
                            AddParkingIdOfBoatsToParkingList(sb.ParkingId, sb.IDNumber, sb.TypeOfBoat);
                        }

                        if (boatData[5] != "")
                        {
                            sb.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != "")
                        {
                            sb.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        sb.BoatLength = int.Parse(boatData[7].Replace("Boat Length - ", ""));

                        boats.Add(sb);
                        break;

                    case "CargoShip":
                        CargoShips cs = new CargoShips();
                        cs.TypeOfBoat = TypeOfBoat.CargoShip;
                        cs.IDNumber = boatData[1];
                        cs.BoatWeight = decimal.Parse(boatData[2]);
                        cs.MaximumSpeed = double.Parse(boatData[3]);
                        if (boatData[4] != "")
                        {
                            cs.ParkingId = boatData[4].Split('|').Select(s => int.Parse(s)).ToList();
                            // TODO
                            AddParkingIdOfBoatsToParkingList(cs.ParkingId, cs.IDNumber, cs.TypeOfBoat);
                        }
                        if (boatData[5] != "")
                        {
                            cs.ArrivalTime = DateTime.Parse(boatData[5]);
                        }

                        if (boatData[6] != "")
                        {
                            cs.DepartureTime = DateTime.Parse(boatData[6]);
                        }

                        cs.NumberOfContainers = int.Parse(boatData[7].Replace("Number of Containers - ", ""));

                        boats.Add(cs);
                        break;

                    default:
                        break;
                }
                rowCount++;
            }
        }

        //TODO write parked boat csv file(unable to save - pending)
        public static void ExportParkedBoatListToParkedBoatCsvFile(List<BaseClassBoat> parkedboat, string parkedBoatFilePath)
        {
            var lines = new List<string>();
            lines.Add("TypeOfBoat,IDNumber, Weight, MaximumSpeed, ParkingId, ArrivalTime, DepartureTime, Miscellaneous");
            foreach (var objBoat in parkedboat)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{objBoat.TypeOfBoat},{objBoat.IDNumber},{objBoat.BoatWeight}," +
                    $"{objBoat.MaximumSpeed},{string.Join('|', objBoat.ParkingId)},{objBoat.ArrivalTime},{objBoat.DepartureTime},"+
                    $"{objBoat.Miscellaneous.Replace("Passenger count - ", "").Replace("Number of Containers - ", "").Replace("Boat Length - ", "").Replace("Number of HorsePower - ", "")}");
                lines.Add(sb.ToString());
            }
            File.WriteAllLines(parkedBoatFilePath, lines.ToArray());
        }

        //TODO update boat csv pending
        public static void ExportBoatListToBoatListCsvFile(List<BaseClassBoat> boats, string boatsFilePath)
        {
            var boatLines = new List<string>();
            boatLines.Add("TypeOfBoat,IDNumber, Weight, MaximumSpeed, ParkingId, ArrivalTime, DepartureTime, Miscellaneous");
            foreach (var item in boats)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{item.TypeOfBoat},{item.IDNumber},{item.BoatWeight}," +
                    $"{item.MaximumSpeed},,,,{item.Miscellaneous}");
                boatLines.Add(sb.ToString());
            }
            File.WriteAllLines(boatsFilePath, boatLines.ToArray());

        }



        //assign available parking number(depending on type of baot)
        private static List<int> AssignParkingLocation(TypeOfBoat boatType, string boatIdNumber)
        {
            switch (boatType)
            {
                case TypeOfBoat.RowingBoat:
                    {
                        ParkingPosition availablePosition = parkingPositionList.Where(x => (x.BoatIds.Count == 1 && x.BoatIds[0].StartsWith("R-")) || x.BoatIds.Count == 0)
                             .OrderBy(x => x.ParkingId).FirstOrDefault();
                        if (availablePosition != null)
                        {
                            availablePosition.BoatIds.Add(boatIdNumber);
                            return new List<int> { availablePosition.ParkingId };
                        }
                    }
                    break;
                case TypeOfBoat.MotorBoat:
                    {
                        ParkingPosition availablePosition = parkingPositionList.Where(x => x.BoatIds.Count == 0).OrderBy(x => x.ParkingId).FirstOrDefault();
                        if (availablePosition != null)
                        {
                            availablePosition.BoatIds.Add(boatIdNumber);
                            return new List<int> { availablePosition.ParkingId };
                        }
                    }
                    break;
                case TypeOfBoat.SailBoat:
                    {
                        var availableBlockIds = parkingPositionList.Where(x => x.BoatIds.Count == 0).Select(x => x.ParkingId).ToList();
                        if (availableBlockIds.Count > 0)
                        {
                            List<int> ids = GetBlockId(availableBlockIds, 2);
                            if (ids != null)
                            {
                                parkingPositionList.Where(x => ids.Contains(x.ParkingId)).ToList().ForEach(x =>
                                  {
                                      x.BoatIds.Add(boatIdNumber);
                                  });
                                return ids;
                            }
                        }
                    }
                    break;
                case TypeOfBoat.CargoShip:
                    {
                        var availableBlockIds = parkingPositionList.Where(x => x.BoatIds.Count == 0).Select(x => x.ParkingId).ToList();
                        if (availableBlockIds.Count > 0)
                        {
                            List<int> ids = GetBlockId(availableBlockIds, 4);
                            if (ids != null)
                            {
                                parkingPositionList.Where(x => ids.Contains(x.ParkingId)).ToList().ForEach(x =>
                                {
                                    x.BoatIds.Add(boatIdNumber);
                                });
                                return ids;
                            }
                        }
                    }
                    break;
            }

            return null;
        }
   
        //search parking location for sailboat and cargo(continous 2 and 4 numbers in parkingid list)
        private static List<int> GetBlockId(List<int> availableBlockIds, int continousLocation = 2)
        {
            List<int> ids = new List<int>();

            if (availableBlockIds.Count() >= continousLocation)
            {
                int[] numbers = Enumerable.Range(1, parkingPositionList.Count).ToArray();

                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers.Length >= i + continousLocation)
                    {
                        ids.Clear();
                        if (availableBlockIds.Contains(numbers[i]))
                        {
                            bool isValid = true;
                            for (int j = 0; j < continousLocation; j++)
                            {
                                if (!availableBlockIds.Contains(numbers[i + j]))
                                    isValid = false;
                                else
                                    ids.Add(numbers[i + j]);
                            }
                            if (isValid)
                            {
                                return ids;
                            }
                        }
                    }
                }
            }
            return null;
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


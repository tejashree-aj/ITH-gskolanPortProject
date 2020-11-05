using DataModels;
using HelpLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;



namespace PortAdministrationProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int parkingCount = 64;
        List<BaseClassBoat> boats = new List<BaseClassBoat>();
        List<BaseClassBoat> parkingBoats = new List<BaseClassBoat>();
        private readonly Timer timer;
        private readonly Timer parkingTimer;
        private readonly Timer seaTimer;
        private readonly Timer updateCSVTimer;

        public MainWindow()
        {
            InitializeComponent();
            BoatActivityManager.GenerateParkingList(parkingCount);
            BoatActivityManager.AddBoatToList(boats);
            BoatActivityManager.AddParkedBoatToList(parkingBoats);

            //TimerCallback delegate - it is a background process which repeats after the schedule time(grid gets updated after 1sec)
            this.timer = new Timer(UpdateGrid, null, 500, 1000);

            ////TimerCallbackdelegate - repeate funct(threading timer library)-updates parking list
            this.parkingTimer = new Timer(startParkingListTimer, null, 500, 5000);

            ////updates boat list
            this.seaTimer = new Timer(boatsBackToSea, null, 500, 3000);

            this.updateCSVTimer = new Timer(ExportBoatDataToCSV, null, 2000, 6000);
        }

        private void UpdateGrid(object state)
        {
            try
            {
                //boats.First().ArrivalTime = DateTime.Now;
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        boatgrid.DataContext = null;
                        boatgrid.DataContext = parkingBoats.ToList();
                    }
                    catch (Exception ex)
                    {

                    }

                    parkedboatcount.Text = $" {parkingBoats.Count()}";

                    var cnt = BoatActivityManager.CountingParkedBoats(parkingBoats);
                    seperateboatcount.DataContext = cnt;

                    decimal totalBtWt = BoatActivityManager.TotalWeightOfParkedBoats(parkingBoats);
                    parkedbaotweight.Text = $" {totalBtWt} Kg";

                    int avgSpeedOfAllBoats = BoatActivityManager.AverageMaxSpeedOfAllBoats(parkingBoats);
                    parkedboatavgspeed.Text = $"{avgSpeedOfAllBoats} Km/hr";

                });
            }
            catch (Exception ex)
            {

            }
        }


        private void startParkingListTimer(object state)
        {
            List<BaseClassBoat> boatsinParkingList = boats.MoveToParkingList();
            if (boatsinParkingList != null)
                parkingBoats.AddRange(boatsinParkingList);

        }

        private void boatsBackToSea(object state)
        {
            List<BaseClassBoat> boatsBackToSeaList = parkingBoats.MoveBoatBackToSeaList();
            if (boatsBackToSeaList != null)
                boats.AddRange(boatsBackToSeaList);
        }

        private void ExportBoatDataToCSV(object state)
        {
            //TODO
            string basePath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            List<BaseClassBoat> parkingBoatsToExport = new List<BaseClassBoat>(parkingBoats);
            List<BaseClassBoat> seaBoatsToExport = new List<BaseClassBoat>(boats);

            string filePath = System.IO.Path.Combine(basePath, @"BoatPortData\ParkedBoatData.csv");
            BoatActivityManager.ExportParkedBoatListToParkedBoatCsvFile(parkingBoatsToExport, filePath);

            filePath = System.IO.Path.Combine(basePath, @"BoatPortData\BoatData.csv");
            BoatActivityManager.ExportBoatListToBoatListCsvFile(seaBoatsToExport, filePath);

            parkingBoatsToExport = null;
            seaBoatsToExport = null;
        }
    }
}

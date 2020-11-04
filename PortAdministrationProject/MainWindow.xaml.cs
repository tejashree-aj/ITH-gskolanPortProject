using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace PortAdministrationProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ObservableCollection data = new ObservableCollection();

        List<BaseClassBoat> boats = new List<BaseClassBoat>();
        List<BaseClassBoat> parkingBoats = new List<BaseClassBoat>();
        private readonly Timer timer;
        private readonly Timer parkingTimer;
        private readonly Timer seaTimer;

        public MainWindow()
        {

            InitializeComponent();

            BoatActivityManager.AddBoatToList(boats);
            BoatActivityManager.ParkingBoatList(parkingBoats);

            //TimerCallback delegate - it is a background process which repeats after the schedule time(grid gets updated after 1sec)
            this.timer = new Timer(UpdateGrid, null, 500, 1000);

            //TimerCallbackdelegate - repeate funct(threading timer library)
            this.parkingTimer = new Timer(parkingList, null, 500, 4000);

            this.seaTimer = new Timer(boatsBackToSea, null, 500, 1000);

            
        }


        private void UpdateGrid(object state)
        {
            try
            {
                //boats.First().ArrivalTime = DateTime.Now;
                this.Dispatcher.Invoke(() =>
                {
                    boatgrid.DataContext = null;
                    boatgrid.DataContext = parkingBoats;

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


        private void parkingList(object state)
        {
            List<BaseClassBoat> boatsinParkingList = boats.ParkingBoatList();
            parkingBoats.AddRange(boatsinParkingList);

            BoatActivityManager.ExportParkedBoatListToParkedBoatCsvFile(parkingBoats.ToList(), @"BoatPortData/ParkedBoatData.csv");
        }

        int saveFileCounter = 0;
        private void boatsBackToSea(object state)
        {
            List<BaseClassBoat> boatsBackToSeaList = parkingBoats.MoveBoatBackToSeaList();
            boats.AddRange(boatsBackToSeaList);

            if (saveFileCounter > 5)
            {
                BoatActivityManager.UpdateBoatListCsvFile(boats.ToList(), @"BoatPortData\BoatData.csv");
                saveFileCounter = 0;
            }
            saveFileCounter++;
        }


    }

}

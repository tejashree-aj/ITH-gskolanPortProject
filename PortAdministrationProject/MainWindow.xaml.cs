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


            

            //TimerCallbackdelegate - repeate funct(threading timer library)
            this.parkingTimer = new Timer(parking, null, 500, 4000);

            this.seaTimer = new Timer(boatsBackToSea, null, 500, 1000);

            //TimerCallback delegate - it is a background process which repeats after the schedule time(grid gets updated after 1sec)
            this.timer = new Timer(UpdateGrid, null, 500, 1000);


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

                    parkedboatcount.Text = $"Total number of boats parked: {parkingBoats.Count()}";

                    var cnt = BoatActivityManager.CountingParkedBoats(parkingBoats);
                    seperateboatcount.DataContext = cnt;

                    decimal totalBtWt = BoatActivityManager.TotalWeightOfParkedBoats(parkingBoats);
                    parkedbaotweight.Text = $"Total weight of boats parked: {totalBtWt} Kg";

                    int avgSpeedOfAllBoats = BoatActivityManager.AverageMaxSpeedOfAllBoats(parkingBoats);
                    parkedboatavgspeed.Text = $"Average speed of parked boats: {avgSpeedOfAllBoats} Km/hr";

                });
            }
            catch (Exception ex)
            {

            }
        }


        private void parking(object state)
        {
            List<BaseClassBoat> boatsinParkingList = boats.ParkingBoatList();
            parkingBoats.AddRange(boatsinParkingList);            
        }

        private void boatsBackToSea(object state)
        {
            List<BaseClassBoat> boatsBackToSeaList = parkingBoats.MoveBoatBackToSeaList();
            boats.AddRange(boatsBackToSeaList);            
        }

    }
}


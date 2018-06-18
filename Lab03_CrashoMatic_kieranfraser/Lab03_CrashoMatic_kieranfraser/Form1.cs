using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDIDrawer;
using DrawerLib;
using System.Threading;

namespace Lab03_CrashoMatic_kieranfraser
{

    
    public partial class Form1 : Form
    {
        PicDrawer gdiCanvas = new PicDrawer(Properties.Resources.Crash, false);         // drawer that will be used and sent to Car static member 
        List<Car> allVechList = new List<Car>(0);                                       // all the dervied cars will be in this base car list
        Random rand = new Random();                                         // random variable used in time to pick car to add
        int elsaped;                                                        // count for timer to drop a new car
        int lives = 3;                                                      // lives count when zero game is done
        int score = 0;
        int[] vspeeds = { 5, -5, 6, -6 };       // differnt speeds to get two up two down for vsedans
        int[] speeds = { 7, -7, 10, -10 };          // differnt speeds to get two left two right for ambulances
        int[] jspeeds = { 5, -5, 6, -6 };       // same as sedans but for bikes
        public Form1()
        {
            InitializeComponent();
           // Car.BuildLanes();
            Car._canvas = gdiCanvas;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int check=1;        // checking used in removeing cars loops
            bool flag = true;   // flag to reset lists if there is a collison
            Car._canvas.Clear();        // clear drawer
            Point clickPo;              // point variable to given users click
            elsaped += timer1.Interval;     // increase eslaped count
           
            //if (allVechList.Count == 0)       // testing block
            //{
            //    VSedan nVSedan = new VSedan(2);
            //    allVechList.Add(nVSedan);
            //    HAmbulance nHAmbulance = new HAmbulance();
            //    allVechList.Add(nHAmbulance);
            //}

            if (elsaped % 2000 == 0) // every 2 seconds add a car
            {
                int ran = rand.Next(4);     // random number reprsenting on the four different cars
                switch (ran)                // switch   on the number
                {
                    case 0:
                        
                        VSedan nVSedan = new VSedan(vspeeds[rand.Next(vspeeds.Length)]);    // give it one of the speeds create vSedan
                        allVechList.Add(nVSedan);                                   // add the sedan
                        break;
                    case 1:
                       
                        HAmbulance nHAmbulance = new HAmbulance(speeds[rand.Next(speeds.Length)]); // give it one of the speeds create ambulance
                        allVechList.Add(nHAmbulance);                                   // add the ambulance
                        break;
                    case 2:
                        JackpotCar njackPot = new JackpotCar();     // create jackpot car
                        allVechList.Add(njackPot);              // add the car
                        break;
                    case 3:
                                                      
                        JumperBike njumpCar = new JumperBike(jspeeds[rand.Next(jspeeds.Length)]);
                        allVechList.Add(njumpCar);
                        break;

                }
            }

            if (Car._canvas.GetLastMouseLeftClick(out clickPo))         // look for click on any car
            {

                foreach (Car c in allVechList)      // iterate through all cars
                {
                    if (c.PointOnCar(clickPo) && !(c is JackpotCar) &&!(c is JumperBike))    // check if just horizontal or vertical car
                        c.ToggleSpeed();
                    if (c.PointOnCar(clickPo) && (c is JackpotCar))     // check if its jackpot car
                        (c as IReactable).DisplayChange();              // car as reactable jackpot car
                    if (c.PointOnCar(clickPo) && (c is JumperBike))  // check if it is jumper car
                        (c as IReactable).DisplayChange();             // car as reactable jumper car
                }
            }
            while (flag)            // run while flag is hit
            {
                flag = false;       // reset flag
                
                
                foreach(Car c in allVechList)   // iterate thourgh cars
                {
                    check = 0;
                    foreach (Car car in allVechList)
                    {
                        if (car.Equals(c))  // add to check will make check greater than 1 if car intersects with more then itself
                            check++;        // increment check
                    }
                    if (check > 1)  // greater than 1 means car hit 
                    {
                        //allVechList.Remove(c);
                        lives = allVechList.First().GetLives();                // decrement lives counter
                        allVechList.RemoveAll(car => car.Equals(c));  // remove all colliding cars
                        flag = true;            // set flag to true to reset out while loop
                       
                        lifeLabel.Text = lives.ToString();  // change lives display
                        break;
                    }
                }

                
                    
            }
            foreach (Car c in allVechList)
            {
                if (Car.OutofBounds(c))
                    score += c.GetSafeScore();
            }
               
            allVechList.RemoveAll(c => Car.OutofBounds(c));         // check if any cars have hit one of the edges
            allVechList.ForEach(c => c.MoveCar());      // move all vechiles
            foreach (Car c in allVechList)          // 
            {
                if (c is IAnimatable)           // find animatable cars for display animation aka "siren"
                    (c as IAnimatable).Animate();   // call the animate method of Ianimatable
            }
            allVechList.ForEach(c => c.ShowCar());      // show all vechiles



            Car._canvas.Render();       // render the whole drawer

            if(allVechList.Count != 0)
           // scoreLabel.Text =allVechList.First().GetSafeScore().ToString();
            scoreLabel.Text = score.ToString();    // update score display to static score counter

            if (Car.Lives == 0)             // check if game is over
            {
                timer1.Enabled = false;     //stop timer
                label1.Text = "GAME OVER!";     // end game string on form
                Car._canvas.AddText("GAMEOVER!!!", 50, Color.Red);  // end game drawer display
            }
        }
        // Pauses all the action when its just to much, with one button
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)     // switch timer and pause button display
            {
                timer1.Enabled = false;
                pauseButton.Text = "Resume";
            }
            else                            // switch timer and pause button display
            {
                timer1.Enabled = true;
                pauseButton.Text = "Pause";
            }
        }
        // resets game drawer, list, lives, score are all reset
        private void resetButton_Click(object sender, EventArgs e)
        {
            allVechList.Clear();                // clear the list
            Car._canvas.Clear();                
            score = 0;
            scoreLabel.Text = 0.ToString();
            label1.Text = "Lives Remaining";
            Car.Lives = 3;
            lifeLabel.Text = Car.Lives.ToString();
            timer1.Enabled = true;
            elsaped = 0;
        }
    }
}

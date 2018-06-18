using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDIDrawer;
using DrawerLib;
using System.Drawing;
using System.Threading;

namespace Lab03_CrashoMatic_kieranfraser
{
    /// <summary>
    ///  base abstract class of Car that  all cars dervie from
    /// </summary>
    public abstract class Car           
    {
        public  static PicDrawer _canvas;       // private static drawer member
        public static PicDrawer canvas      // public static drawer property
        {
            get { return _canvas; }     // get drawer member
            set
            {
                if (_canvas != null) // close and give new drawer when set
                {
                    _canvas.Close();
                    _canvas = value;
                }
            }
        }
        //public static Random _rand   { get; set; }
        public static Random _rand = new Random();      // random to be used throughout Car class hierachry     
        public static  int Score = 0;            // public member for score updating and display
        public static int Lives = 3;

        //static List<int> allDirectionList = new List<int>();

        public static readonly List<int> downList = new List<int>() { 170, 490 };  // list of two vertical downlanes
        public static readonly List<int> upList = new List<int>() { 270, 590 }; // list of two vertical up lanes
        public static readonly List<int> leftList = new List<int>() { 164 };    // left lane
        public static readonly List<int> rightList = new List<int>() { 260 };   // right lane
        

        protected int _Xval;        // x point value used for getrect()
        protected int _Yval;        // y point value used for getrect()
        protected int _width;       // width of rect used for getrect()
        protected int _height;      // height of rect used for getrect()
        protected int _speed;       // how fast the car will be moving across the drawer and used for score increase

        protected enum SpeedType        // three differnt speeds
        {
            QuarterSpeed, HalfSpeed, FullSpeed
        }
        protected SpeedType _cSpeed= SpeedType.FullSpeed;       // protected member for the current speed to be used in drawer


        /// <summary>
        ///  public constructor to initialize all cars rectangle data
        /// </summary>
        /// <param name="speed"> spped of car that will change x or y values</param>
        /// <param name="width">width of car</param>
        /// <param name="height">height of car</param>
        public Car (int speed, int width, int height)      
        {
            _speed = speed;
            _width = width;
            _height = height;
        }
        /// <summary>
        ///  Creates the rectangle of the Car using x,y,width, height
        ///  ovveridden in dervied classes
        /// </summary>
        /// <returns>Will return the rectangle of the Car</returns>
        public abstract Rectangle GetRect();  

        /// <summary>
        ///  Call the dervied classes overrided VShowCar()
        /// ie: Horizontal and Vertical Car has different VShowCar methods
        /// </summary>
        public void ShowCar()
        {
            VShowCar();
        }
        /// <summary>
        ///  abstract method to overriden in derived car classes
        /// </summary>
        protected abstract void VShowCar();
        /// <summary>
        /// Call the dervied classes overrided VMoveCar()
        /// ie: Horizontal and Vertical Car has different VMoveCar methods
        /// </summary>
        public void MoveCar()
        {
            VMoveCar();
        }
        /// <summary>
        /// abstract method to overriden in derived car classes
        /// </summary>
        protected abstract void VMoveCar();
        /// <summary>
        /// If the rectangle of two cars is touching they are equal
        /// </summary>
        /// <param name="obj">Vechile to compare aganist</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Car))
                return false;
            Car arg = obj as Car;
            return this.GetRect().IntersectsWith(arg.GetRect());
        }
        /// <summary>
        /// Whatever gethashcode does still not sure
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
        }
        /// <summary>
        /// Checks if the users click was on a Car
        /// </summary>
        /// <param name="click">click point on the PicDrawer</param>
        /// <returns></returns>
        public bool PointOnCar (Point click)
        {
            return this.GetRect().Contains(click);
        }
        /// <summary>
        /// Check if a vechile has reached the edge of the drawer in any direction.
        /// remove the vechile if it has
        /// </summary>
        /// <param name="car">boundary check on this car</param>
        /// <returns></returns>
        public static bool OutofBounds (Car car )
        {
            Rectangle leftWall = new Rectangle(0-car._width, 0, 1, _canvas.ScaledHeight);      // left wall rectangle boundary
            Rectangle rightWall = new Rectangle(_canvas.ScaledWidth+car._width, 0, 1, _canvas.ScaledHeight);   // right wall rectangle boundary
            Rectangle topWall = new Rectangle(0, 0-car._height, _canvas.ScaledWidth, 1);            // top wall rectangle boundary
            Rectangle botWall = new Rectangle(0, _canvas.ScaledHeight+car._height, _canvas.ScaledWidth, 1);     // bottom wall rectangle boundary

            if (leftWall.IntersectsWith(car.GetRect()))     // left boundary check
            {
                //if (!(car is JackpotCar))
                //    Car.Score += Math.Abs(car._speed);          //score increase
                //else Car.Score += 50;
                return true;
            }   
            if (rightWall.IntersectsWith(car.GetRect()))    // right booundary check
            {
                //if (!(car is JackpotCar))
                //    Car.Score += Math.Abs(car._speed);          //score increase
                //else Car.Score += 50;
                return true;
            }
            if (topWall.IntersectsWith(car.GetRect()))      // top boundary check
            {
                //if (!(car is JackpotCar))
                //    Car.Score += Math.Abs(car._speed);          //score increase
                //else Car.Score += 50;
                return true;
            }
            if (botWall.IntersectsWith(car.GetRect()))       // bottom boundary check
            {
                //if (!(car is JackpotCar))
                //    Car.Score += Math.Abs(car._speed);          //score increase
                //else Car.Score += 50;
                return true;
            }
            return false;           // retrun false if car is within drawer on all sides
        }
        /// <summary>
        /// Will set speed type to one of three options after each click
        /// </summary>
        public void ToggleSpeed()
        {
            if (_cSpeed == SpeedType.QuarterSpeed)
                _cSpeed = SpeedType.HalfSpeed;
            else if (_cSpeed == SpeedType.HalfSpeed)
                _cSpeed = SpeedType.FullSpeed;
            else
                _cSpeed = SpeedType.QuarterSpeed;
        }
        /// <summary>
        /// Simple abstract method to be overidden 
        /// that will return the user's total score from a static memeber
        /// </summary>
        /// <returns></returns>
        public abstract int GetSafeScore(); 
        /// <summary>
        /// Simple abstract method to be overriden
        /// that will return the user's total lives left after decrement
        /// </summary>
        /// <returns></returns>
        public abstract int GetLives();    // my equilivalent hit score 
        

      
    }

    /// <summary>
    ///  horizontal cars will only spawn in the horizontal lanes
    ///  their x_values will be changed with Vmovecar based on CAR class's speed
    /// </summary>
    public abstract class HorizontalCar : Car
    {
        /// <summary>
        ///  Horizontal Car constructor that will place the car on either right or left lanes
        ///  uses base Car class's base constructor for rectangle data. This constructor
        ///  sets starting point of car based on speed
        /// </summary>
        /// <param name="speed">will determine direction and start point of car</param>
        /// <param name="width">already assigned in base ctor</param>
        /// <param name="height">already assigned in base ctor</param>
        public HorizontalCar(int speed, int width, int height) : base(speed, width, height)
        {
            if (speed < 0) // car going towards the left of the drawer
            {
                _Xval = Car._canvas.ScaledWidth-1;
                _Yval = Car.leftList[Car._rand.Next(leftList.Count)];
            }
            else  // car driving toward the right of the drawer 
            {
                _Xval = 1;
                _Yval = Car.rightList[Car._rand.Next(rightList.Count)];
            }
        }
        /// <summary>
        /// Check enum for to get display. Speed then move car by display speed
        /// horizontal movement for horizontal cars  ie:change xval
        /// </summary>
        protected override void VMoveCar() 
        {
            int displaySpeed = _speed; // will be determined by base speed and current enum speed
            switch (_cSpeed)
            {
                case SpeedType.QuarterSpeed:
                    if (displaySpeed / 2 != 0)
                         displaySpeed = _speed / 4;
                    else
                        _speed = 1;
                    break;
                case SpeedType.HalfSpeed:
                    if (displaySpeed / 2 != 0)
                        displaySpeed= _speed / 2;
                    else
                        _speed = 1;
                    break;
                case SpeedType.FullSpeed:
                    displaySpeed = _speed;
                    break;
            }
            _Xval += displaySpeed;      // update by display speed
        }

       

    }

    /// <summary>
    ///  Vertical cars will only spawn in the vertical lanes
    ///  their y_values will be changed with Vmovecar based on CAR class's speed
    /// </summary>
   public abstract class VerticalCar: Car
    {
        /// <summary>
        ///  Vertical Car constructor that will place the car on either right or left lanes
        ///  uses base Car class's base constructor for rectangle data. This constructor
        ///  sets starting point of car based on speed
        /// </summary>
        /// <param name="speed">will determine direction and start point of car</param>
        /// <param name="width">already assigned in base ctor</param>
        /// <param name="height">already assigned in base ctor</param>
        public VerticalCar (int speed, int width, int height) : base(speed, width, height)
        {
            if (speed < 0) // cars driving toward top of street
            {
                _Yval = Car._canvas.ScaledHeight - 1;       // set y point to just above bottom of drawer
                _Xval = Car.upList[Car._rand.Next(upList.Count)];   // set to random vertical lane
            }
            else // car driving toward bottom of street/drawer
            {
                _Yval =  0-height +1;       // set y point to just below top of drawer
                _Xval = Car.downList[Car._rand.Next(downList.Count)];   // set to random vertical lane
            }

            
        }
        /// <summary>
        /// Check enum for to get display. Speed then move car by display speed
        /// vertical movement for vertical cars ie:change yval
        /// </summary>
        protected override void VMoveCar() 
        {
            int displaySpeed = _speed; // will be determined by base speed and current enum speed
            switch (_cSpeed)        // swtich on enum for quarter, half or full speed
            {
                case SpeedType.QuarterSpeed:
                    if (displaySpeed / 2 != 0)
                        displaySpeed = _speed / 4;
                    else
                        _speed = 1;
                    break;
                case SpeedType.HalfSpeed:
                    if (displaySpeed / 2 != 0)
                        displaySpeed = _speed / 2;
                    else
                        _speed = 1;
                    break;
                case SpeedType.FullSpeed:
                    displaySpeed = _speed;
                    break;
            }
            _Yval += displaySpeed;      // update by display speed
        }
    }

    /// <summary>
    ///  Plain vertical moving car dervied from verticalCar 
    /// </summary>
    public class VSedan : VerticalCar
    {
        protected Color _color = RandColor.GetColor();  // random car color member


        /// <summary>
        /// Vsedan ctor that gets all needed data from its base classes
        /// </summary>
        /// <param name="speed">already used in base ctor</param>
        /// <param name="width">already assigned in base ctor</param>
        /// <param name="height">already assigned in base ctor</param>
        public VSedan(int speed, int width = 40, int height = 70) : base(speed, width, height)
        {
            
        }
        /// <summary>
        /// Get the rectangle of the Vsedan based on x,y,width,height base class members
        /// </summary>
        /// <returns>Returns the rectangle based on current base member values</returns>
        public override Rectangle GetRect()
        {
            Rectangle tempR = new Rectangle(_Xval, _Yval, _width, _height);
            return tempR;
        }
        /// <summary>
        /// Show the car on the drawer. This method is called by the base class
        /// </summary>
        protected override void VShowCar()
        {
            Rectangle tempR = GetRect();
            Car._canvas.AddRectangle(tempR.X, tempR.Y, tempR.Width, tempR.Height, _color);
            Car._canvas.AddRectangle(tempR.X, tempR.Y+10, 8, 8, Color.Black);       // all four wheels
            Car._canvas.AddRectangle(tempR.X+33, tempR.Y+10, 8, 8, Color.Black);    // drawen for the vedan
            Car._canvas.AddRectangle(tempR.X, tempR.Y + 58, 8, 8, Color.Black);
            Car._canvas.AddRectangle(tempR.X+33, tempR.Y + 58, 8, 8, Color.Black);

        }
        /// <summary>
        ///  Override for abstract score method that just returns cars speed
        /// </summary>
        /// <returns></returns>
        public override int GetSafeScore()
        {
            return Math.Abs(this._speed);
        }
        /// <summary>
        /// Override for abstract Lives method that  decrements lives
        /// cause of the collision and returns it
        /// </summary>
        /// <returns></returns>
        public override int GetLives()
        {
            Car.Lives--;
            return Car.Lives;
        }
    }

    /// <summary>
    /// Animate method to be used for switching of colors or shapes
    /// </summary>
    public interface IAnimatable
    {
        void Animate();
    }

    public class HAmbulance : HorizontalCar, IAnimatable
    {
        protected Color _color = RandColor.GetColor();    // random car color member
        private bool RB = true;         // bool for animatie switching 

        /// <summary>
        /// Hambulance ctor that gets all needed data from its base classes
        /// </summary>
        /// <param name="speed">already used in base ctor</param>
        /// <param name="width">already assigned in base ctor</param>
        /// <param name="height">already assigned in base ctor</param>
        public HAmbulance(int speed=7, int width = 90, int height = 40) : base(speed, width, height)
        {

        }
        /// <summary>
        /// Get the rectangle of the Vsedan based on x,y,width,height base class members
        /// </summary>
        /// <returns>Returns the rectangle based on current base member values</returns>
        public override Rectangle GetRect()
        {
            Rectangle tempR = new Rectangle(_Xval, _Yval, _width, _height);
            return tempR;
        }
        /// <summary>
        /// Show the car on the drawer. Two versions of siren color for animation
        /// based on RB bool that switches in animate
        /// This method is called by the base class
        /// </summary>
        protected override void VShowCar()
        {
            Rectangle tempR = GetRect();
            Car._canvas.AddRectangle(tempR.X, tempR.Y, tempR.Width, tempR.Height, _color);
            if (RB)
                Car._canvas.AddLine(tempR.X + (tempR.Width/2), tempR.Y, tempR.X + (tempR.Width / 2), tempR.Y + tempR.Height, Color.Red, 5);
           else  
                Car._canvas.AddLine(tempR.X + (tempR.Width / 2), tempR.Y, tempR.X + (tempR.Width / 2), tempR.Y + tempR.Height, Color.Yellow, 5);
           
        }
        /// <summary>
        ///  Switches a bool for ShowCar to check if it for dispaly of red or yellow Siren
        /// </summary>
        public void Animate()
        {
            if (RB) RB = false; 
            else RB = true;                  
        }

        /// <summary>
        /// Override for abstract score method that just returns cars speed
        /// </summary>
        /// <returns></returns>
        public override int GetSafeScore()
        {
            return Math.Abs(this._speed);
        }
        /// <summary>
        /// Override for abstract Lives method that  decrements lives
        /// cause of the collision and returns it
        /// </summary>
        /// <returns></returns>
        public override int GetLives()
        {
            Car.Lives--;
            return Car.Lives;
        }
    }
    /// <summary>
    /// Interface for cars that want to react to a user click
    /// rather then change speed
    /// </summary>
    public interface IReactable
    {
        void DisplayChange();
    }

    /// <summary>
    /// JackPot cars are horizontal cars that isnt able to change its speed
    /// they are worth more points and will display a message 
    /// </summary>
    public class JackpotCar: HorizontalCar,IReactable
    {
        protected Color _color = Color.Gold;    // gold car color member
        private bool clicked = false;           // bool to check user click
        public JackpotCar (int speed = 7, int width = 130, int height = 30) : base(speed, width, height)
        {
            if (speed < 0) // car going towards the left of the drawer
            {
                _Xval = Car._canvas.ScaledWidth - width - 2;
                _Yval = 222;
            }
            else  // car driving toward the right of the drawer 
            {
                _Xval = 10;
                _Yval = 222 ;
            }
        }
        /// <summary>
        /// Get the rectangle of the Vsedan based on x,y,width,height base class members
        /// </summary>
        /// <returns>Returns the rectangle based on current base member values</returns>
        public override Rectangle GetRect()
        {
            Rectangle tempR = new Rectangle(_Xval, _Yval, _width, _height);
            return tempR;
        }
        /// <summary>
        /// Show the car on the drawer. Two versions of siren color for animation
        /// based on RB bool that switches in animate
        /// This method is called by the base class
        /// </summary>
        protected override void VShowCar()
        {
            Rectangle tempR = GetRect();            // get the rect()                
            Car._canvas.AddRectangle(tempR.X, tempR.Y, tempR.Width, tempR.Height, _color);
            if (!clicked)
            Car._canvas.AddText("Protect the Payload", 8, tempR.X, tempR.Y, tempR.Width, tempR.Height);     // change the display
            else
            Car._canvas.AddText("Cant change Speed", 8, tempR.X, tempR.Y, tempR.Width, tempR.Height);       // 
        }
        /// <summary>
        /// Simple bool swtich that will change the display on the jackpot car
        /// </summary>
        public void DisplayChange()
        {
            if (!clicked)
                clicked = true;
            else clicked = false;

        }
        /// <summary>
        /// Override for abstract score method that return jackpot of 50 points
        /// </summary>
        /// <returns></returns>
        public override int GetSafeScore()
        {
            return 50;
        }
        /// <summary>
        /// Override for abstract Lives method that  decrements lives
        /// cause of the collision and returns it
        /// </summary>
        /// <returns></returns>
        public override int GetLives()
        {
            Car.Lives--;
            return Car.Lives;
        }
    }
    /// <summary>
    /// Jumper cars are little red cars that can jump forward or backwards 
    /// based on whether they are in down or up lanes
    /// </summary>
    public class JumperBike: VerticalCar , IReactable
    {
        protected Color _color = Color.Red;    // gold car color member
        private bool clicked = false;           // bool to check user click
        /// <summary>
        /// CTOR for bikes already has all needed data from dervied classes
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public JumperBike(int speed =7, int width =25, int height = 55) :base(speed, width, height)
        {

        }
        /// <summary>
        /// Get the rectangle of the JumperCar based on x,y,width,height base class members
        /// </summary>
        /// <returns>Returns the rectangle based on current base member values</returns>
        public override Rectangle GetRect()     
        {
            Rectangle tempR = new Rectangle(_Xval, _Yval, _width, _height); // creating rectangle
            return tempR;                           // return it
        }
        /// <summary>
        /// Show the car on the drawer. Two versions of siren color for animation
        /// based on RB bool that switches in animate
        /// This method is called by the base class
        /// </summary>
        protected override void VShowCar()
        {
            Rectangle tempR = GetRect();                                                    // return the jumper cars rectangle
            Car._canvas.AddRectangle(tempR.X, tempR.Y, tempR.Width, tempR.Height, _color);  // draw it
            Car._canvas.AddRectangle(tempR.X + (tempR.Width / 2)-4, tempR.Y, 7, 7, Color.Black);                    // two wheels for the bike
            Car._canvas.AddRectangle(tempR.X + (tempR.Width / 2)-4, tempR.Y+tempR.Height-3, 7, 7, Color.Black);
            if (!clicked)
                Car._canvas.AddText("Jump", 8, tempR.X, tempR.Y, tempR.Width, tempR.Height);        // change the cars display
            else
                Car._canvas.AddText("YeaHAW", 8, tempR.X, tempR.Y, tempR.Width, tempR.Height);     // 

        }
        /// <summary>
        /// When called by a click jump the car forward or backwards base on its height and the lane its in
        /// will also change the text display
        /// </summary>
        public void DisplayChange()
        {
            if (!clicked)
            {
                clicked = true;
                this._Yval=this._Yval + this._height*2;         // jump the cars y value
            }
                
            else clicked = false;                       // switch back to not clicked

        }
        /// <summary>
        ///  Override for abstract score method that just returns cars speed
        /// </summary>
        /// <returns></returns>
        public override int GetSafeScore()
        {
            return Math.Abs(this._speed);
        }
        /// <summary>
        /// Override for abstract Lives method that  decrements lives
        /// cause of the collision and returns it
        /// </summary>
        /// <returns></returns>
        public override int GetLives()
        {
            Car.Lives--;
            return Car.Lives;
        }



    }



}

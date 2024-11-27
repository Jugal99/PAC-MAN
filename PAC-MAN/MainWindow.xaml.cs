using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace PAC_MAN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        bool Left, Right, Down, Up; 
        bool nLeft, nRight, nDown, nUp;

        int speed = 8;

        Rect pacmancoll;
        int ghosts = 10;
        int ghostSteps = 130;
        int currentstep;
        int score = 0;
        public MainWindow()
        {
            InitializeComponent();
            GameUp();
        }
        
        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && nLeft == false)
            {
                Right = Up = Down = false;
                nRight = nUp = nDown = false;

                Left = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);

            }

            if (e.Key == Key.Right && nRight == false)
            {
            
                nLeft = nUp = nDown = false; 
                Left = Up = Down = false; 

                Right = true; 

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);

            }


            if (e.Key == Key.Up && nUp == false)
            {
                
                nRight = nDown = nLeft = false; 
                Right = Down = Left = false;

                Up = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && nDown == false)
            {
                
                nUp = nLeft = nRight = false; 
                Up = Left = Right = false; 

                Down = true; 

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); 
            }
        }

        private void GameUp()
        {
            MyCanvas.Focus();
            gameTimer.Tick += loop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            currentstep = ghostSteps;

            ImageBrush pacmanimage = new ImageBrush();
            pacmanimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanimage;

            ImageBrush redG = new ImageBrush();
            redG.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            red.Fill = redG;

            ImageBrush orangeG = new ImageBrush();
            orangeG.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orange.Fill = orangeG;

            ImageBrush pinkG = new ImageBrush();
            pinkG.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pink.Fill = pinkG;

        }

        private void loop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            if (Right)
            {
    
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (Left)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (Up)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (Down)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }
            if (Down && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                
                nDown = true;
                Down = false;
            }

            if (Up && Canvas.GetTop(pacman) < 1)
            {
                
                nUp = true;
                Up = false;
            }
            if (Left && Canvas.GetLeft(pacman) - 10 < 1)
            {
                
                nLeft = true;
                Left = false;
            }
            if (Right && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                
                nRight = true;
                Right = false;
            }
            pacmancoll = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect Box = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    if (Left == true && pacmancoll.IntersectsWith(Box))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        nLeft = true;
                        Left = false;
                    }

                    if (Right == true && pacmancoll.IntersectsWith(Box))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        nRight = true;
                        Right = false;
                    }
                    if (Down == true && pacmancoll.IntersectsWith(Box))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        nDown = true;
                        Down = false;
                    }
                    if (Up == true && pacmancoll.IntersectsWith(Box))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        nUp = true;
                        Up = false;
                    }
                }

                if ((string)x.Tag == "coin")
                {
                   
                    if (pacmancoll.IntersectsWith(Box) && x.Visibility == Visibility.Visible)
                    {
                        
                        x.Visibility = Visibility.Hidden;
                        
                        score++;
                    }
                }
                if ((string)x.Tag == "ghost")
                {
                    if (pacmancoll.IntersectsWith(Box))
                    {
                       
                        GameFin("Hard luck!, click ok to play again");
                    }
                    if (x.Name.ToString() == "orange")
                    {
                     
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghosts);

                    }
                    else
                    {
                        
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghosts);
                    }
                    currentstep--;
                    if (currentstep < 1)
                    {
                        currentstep = ghostSteps;
                        ghosts = -ghosts;
                    }
                }

            }
            if (score == 46)
            {
                
                GameFin("WINNER!!");
            }
        }

        private void GameFin(string msg)
        {
            gameTimer.Stop();
            MessageBox.Show(msg, "PAC MAN DONE");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

       
    }
}

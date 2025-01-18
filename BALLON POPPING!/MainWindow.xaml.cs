
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace BALLON_POPPING_
{
  
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        int speed = 3;
        int intervals = 90;
        Random rand = new Random();




        List<Rectangle> itemRemover = new List<Rectangle>();

        ImageBrush backgroundImage = new ImageBrush();

        int balloonSkins;
        int i;

        int missedBalloons;

        bool gameIsActive;

        int score;

        MediaPlayer player = new MediaPlayer();





        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/12.jpg"));
            MyCanvas.Background = backgroundImage;

            RestartGame();
        }

        private void GameEngine(object sender, EventArgs e)
        {



            scoreText.Content = "Score: " + score;

            intervals -= 10;

            if (intervals < 1)
            {
                ImageBrush balloonImage = new ImageBrush();

                balloonSkins += 1;

                if (balloonSkins > 5)
                {
                    balloonSkins = 1;
                }

                switch (balloonSkins)
                {
                    case 1:


                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/balloon1.png"));
                        break;
                    case 2:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/balloon2.png"));
                        break;
                    case 3:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/balloon3.png"));
                        break;
                    case 4:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/balloon4.png"));
                        break;
                    case 5:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/balloon5.png"));
                        break;
                }



                Rectangle newBalloon = new Rectangle
                {
                    Tag = "balloon",
                    Height = 70,
                    Width = 50,
                    Fill = balloonImage
                };






                Canvas.SetLeft(newBalloon, rand.Next(50, 400));
                Canvas.SetTop(newBalloon, 600);

                MyCanvas.Children.Add(newBalloon);

                intervals = rand.Next(90, 150);
            }




            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "balloon")
                {

                    i = rand.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));

                }

                if (Canvas.GetTop(x) < 20)
                {
                    itemRemover.Add(x);

                    missedBalloons += 1;
                    HealthBar.Value -= 10;
                }


            }


            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }



            if (missedBalloons > 10)
            {
                gameIsActive = false;
                gameTimer.Stop();
                Storyboard fadeInStoryBoard = (Storyboard)Resources["FadeInStoryBoard"];
                fadeInStoryBoard.SetValue(Storyboard.TargetProperty, GameEndScreen);
                fadeInStoryBoard.Begin();
                Score.Text = "You Popped " + score + " Balloons!";
                GameEndScreen.Visibility= Visibility.Visible;
                HealthBar.Visibility= Visibility.Collapsed;
                



            }

            if (score > 3)
            {
                speed = 7;
            }

            //this was made in c# without any ither engine so please consider voting

        }

        private void PopBalloons(object sender, MouseButtonEventArgs e)
        {
            if (gameIsActive)
            {

                if (e.OriginalSource is Rectangle)
                {

                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    player = new MediaPlayer();
                    player.Open(new Uri("pack://Application:,,,/images/pop_sound.mp3"));
                    player.Play();

                    MyCanvas.Children.Remove(activeRec);

                    score += 1;

                }
            }
        }

        private void StartGame()
        {
            gameTimer.Start();
            GameEndScreen.Visibility= Visibility.Collapsed;
            HealthBar.Visibility = Visibility.Visible;
            HealthBar.Value = 100;

            missedBalloons = 0;
            score = 0;
            intervals = 90;
            gameIsActive = true;
            speed = 3;
        }

        private void RestartGame()
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }

            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }

            itemRemover.Clear();


            StartGame();



        }
        
private void RestartGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameEndScreen.Visibility = Visibility.Collapsed;
            RestartGame();

        }
    }
}

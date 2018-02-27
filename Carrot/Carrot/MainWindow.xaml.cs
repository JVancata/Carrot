using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Carrot
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string currentMap = "bg1.png";
        static Player player = new Player("Knedlik", "hrac", 100, 1, 1);
        public int windowWidth = 525;
        public int windowHeight = 350;


        public MainWindow()
        {
            InitializeComponent();
            //this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            Render();
            //timer
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            //timer
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Render();
            CheckInputs();
        }
        public void CheckInputs()
        {
            /*if (e.Key == Key.Left)
            {
                player.X -= player.Velocity;
            }
            if (e.Key == Key.Right)
            {
                player.X += player.Velocity;
            }*/
            
                Debug.WriteLine(player.X + player.Velocity + 60);
                if (((Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))) && ((player.X + player.Velocity+72) < windowWidth))
                {
                    player.X += player.Velocity;
                }
                if (((Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))) && ((player.X - player.Velocity) > 0))
                {
                    player.X -= player.Velocity;
                }
            
        }
        public void Render()
        {
            Board.Children.Clear();

            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/"+currentMap, UriKind.Relative));
            bg.Height = 350;
            //Panel.SetZIndex(bg, 1);
            Board.Children.Add(bg);

            Image image = new Image();
            image.Source = player.SpriteImage;
            image.Width = 60;
            Canvas.SetLeft(image, player.X);
            Canvas.SetTop(image, player.Y+125);
            //Panel.SetZIndex(image, 100);
            Board.Children.Add(image);
            
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

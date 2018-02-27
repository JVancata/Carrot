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


        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            Render();
            //timer
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            //timer
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Render();
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
            image.Height = 100;
            Canvas.SetLeft(image, player.X);
            Canvas.SetTop(image, player.Y+125);
            //Panel.SetZIndex(image, 100);
            Board.Children.Add(image);
            
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                player.X -= player.Velocity;
            }
            if (e.Key == Key.Right)
            {
                player.X += player.Velocity;
            }
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

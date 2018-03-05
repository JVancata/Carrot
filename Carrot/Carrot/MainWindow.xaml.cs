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
        public int currentMapNumber = 0;
        public int maxMapNumber = 1;
        public bool canSwitch = true;
        public string currentMap = "bg0.png";
        static Player player = new Player("Knedlik", "hrac", 100, 1, 1, 10);
        public int windowWidth = 1000;
        public int windowHeight = 350;
        public List<NPC> NPCList = new List<NPC>();
        public MainWindow()
        {
            InitializeComponent();
            fillNpc();
            //timer
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            //timer
        }
        private void fillNpc()
        {
            NPCList.Add(new NPC("Bulmír", "npc", 0, 200, 0, 0));
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckInputs();
            switchMaps();
            Render();
        }
        public void switchMaps()
        {
            if(player.X <= 64 && currentMapNumber > 0 && canSwitch) {
                currentMapNumber--;
                player.X = windowWidth - 100;
                canSwitch = false;
            }
            else if (player.X >= windowWidth-72-player.Velocity && currentMapNumber < maxMapNumber && canSwitch)
            {
                currentMapNumber++;
                player.X = 100;
                canSwitch = false;
            }
            else
            {
                canSwitch = true;
            }
            Debug.WriteLine(currentMapNumber);
        }
        public void CheckInputs()
        {
            //64 - 514
            Debug.WriteLine(player.X);
            if (((Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))) && ((player.X + player.Velocity + 72) < windowWidth))
            {
                player.X += player.Velocity;
            }
            if (((Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))) && ((player.X - player.Velocity) > 0))
            {
                player.X -= player.Velocity;
            }
        }

        public void MapInteraction()
        {
            switch (currentMapNumber)
            {
                case 0:
                    StoryLabel.Content = "Zde začínáš";
                    break;
                case 1:
                    StoryLabel.Content = "Zde pokračuješ";
                    break;
                default:
                    break;
            }
        }

        public void Render()
        {
            Board.Children.Clear();
            MapInteraction();
            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/" + "bg"+currentMapNumber+".png", UriKind.Relative));
            bg.Height = 350;
            //Panel.SetZIndex(bg, 1);
            Board.Children.Add(bg);

            Image playerImg = new Image();
            playerImg.Source = player.SpriteImage;
            playerImg.Width = 60;
            Canvas.SetLeft(playerImg, player.X);
            Canvas.SetTop(playerImg, player.Y + 150);
            //Panel.SetZIndex(image, 100);
            Board.Children.Add(playerImg);

            foreach(NPC npc in NPCList)
            {
                if(npc.Map == currentMapNumber)
                {
                    Image image = new Image();
                    image.Source = npc.SpriteImage;
                    image.Width = 60;
                    Canvas.SetLeft(image, npc.X);
                    Canvas.SetTop(image, npc.Y + 150);
                    //Panel.SetZIndex(image, 100);
                    Board.Children.Add(image);

                }
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

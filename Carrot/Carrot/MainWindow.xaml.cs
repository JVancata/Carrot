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
        public Game game = new Game();
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
            NPCList.Add(new NPC("Bulmír", "npc", 1, 800, 0, 0, "npc.png"));
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckInputs();
            switchMaps();
            Render();
        }
        public void switchMaps()
        {
            if (player.X <= 20 && game.currentMapNumber > 0 && game.canSwitch)
            {
                game.currentMapNumber--;
                player.X = windowWidth - 100;
                game.canSwitch = false;
            }
            else if (player.X >= windowWidth - 72 - player.Velocity && game.currentMapNumber < game.maxMapNumber && game.canSwitch)
            {
                game.currentMapNumber++;
                player.X = 30;
                game.canSwitch = false;
            }
            else
            {
                game.canSwitch = true;
            }
            Debug.WriteLine(game.currentMapNumber);
        }
        public void CheckInputs()
        {
            //64 - 514
            Debug.WriteLine(player.X);
            if (((Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))) && ((player.X + player.Velocity + 72) < windowWidth))
            {
                player.X += player.Velocity;
                player.Direction = true;
            }
            if (((Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))) && ((player.X - player.Velocity) > 0))
            {
                player.X -= player.Velocity;
                player.Direction = false;
            }
        }

        public void hideButtons()
        {
            Button1.Visibility = Visibility.Hidden;
            Button2.Visibility = Visibility.Hidden;
            Button3.Visibility = Visibility.Hidden;
            Button4.Visibility = Visibility.Hidden;
        }

        public void MapInteraction()
        {
            hideButtons();
            switch (game.currentMapNumber)
            {
                case 0:
                    if (!game.hasApple && game.storyPosition == 0 && game.arrivedOnSecondMap)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Seber jablko";
                    }
                    break;
                case 1:
                    game.arrivedOnSecondMap = true;
                    if (!game.hasApple && game.storyPosition == 0)
                    {
                        game.currentMessage = "Bulmír chce jablko1!!!\nJinak Tě nepustí dál!";
                    }
                    if (game.hasApple && game.storyPosition == 1)
                    {
                        game.currentMessage = "Máš jablko, které Bulmír chce,\ndej mu ho a on Tě pustí dál.";
                    }
                    if(game.hasApple && game.storyPosition == 1 && player.x > 700)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Daj mu jablko";
                    }
                    if(game.storyPosition == 2)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Díky, brácho";
                    }

                    break;
                default:
                    break;
            }
            StoryLabel.Content = game.currentMessage;
        }

        public void Render()
        {
            Board.Children.Clear();
            MapInteraction();
            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/" + "bg" + game.currentMapNumber + ".png", UriKind.Relative));
            bg.Height = 350;
            //Panel.SetZIndex(bg, 1);
            Board.Children.Add(bg);

            Image playerImg = new Image();
            playerImg.Source = player.SpriteImage;
            playerImg.Width = 60;
            /*if(player.Direction)
            {
                var transform = new ScaleTransform(-1, 1, 0, 0);
                playerImg.RenderTransform = transform;
                if(player.X)
            }*/
            Canvas.SetLeft(playerImg, player.X);
            Canvas.SetTop(playerImg, player.Y + 150);
            Panel.SetZIndex(playerImg, 100);
            Board.Children.Add(playerImg);

            foreach (NPC npc in NPCList)
            {
                if (npc.Map == game.currentMapNumber)
                {
                    Image image = new Image();
                    image.Source = npc.SpriteImage;
                    image.Width = 60;
                    Canvas.SetLeft(image, npc.X);
                    Canvas.SetTop(image, npc.Y + 150);
                    Board.Children.Add(image);
                }
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if(game.currentMapNumber == 0 && game.storyPosition == 0)
            {
                if (!game.hasApple)
                {
                    game.hasApple = true;
                    game.currentMessage = "Sebral jsi jablko";
                    game.storyPosition++;
                }
            }
            if (game.currentMapNumber == 1 && game.storyPosition == 1 && game.hasApple)
            {
                game.hasApple = false;
                game.storyPosition++;
                game.currentMessage = "Hmmmm, fajnový jablko";
            }
            if (game.storyPosition == 2)
            {
                game.currentMessage = "Nz, pouštím Tě dál\n(nepouští, není to nakreslený)";
                game.storyPosition++;
            }
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

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
            
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            //timer
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
            {
                Button1_Click(null, null);
            }
            if (e.Key == Key.D2)
            {
                Button2_Click(null, null);
            }
            if (e.Key == Key.D3)
            {
                Button3_Click(null, null);
            }
            if (e.Key == Key.D4)
            {
                Button4_Click(null, null);
            }
        }
        private void fillNpc()
        {
            NPCList.Add(new NPC("Bulmír", "npc", 1, 800, 0, 0, "npc.png"));
            //NPCList.Add(new NPC("Vlk", "monster", 2, 300, 0, 0, "vlk.png"));
            NPCList.Add(new NPC("Kiddo", "npc", 2, 500, 0, 0, "character-kid.png"));
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
            else if (player.X >= windowWidth - 72 - player.Velocity && game.currentMapNumber < game.currentMaxMapNumber && game.canSwitch)
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
                player.X += game.sprinting ? (int)Math.Floor(player.Velocity * game.sprintSpeed) : player.Velocity;
                player.Direction = true;
            }
            if (((Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))) && ((player.X - player.Velocity) > 0))
            {
                player.X -= game.sprinting ? (int)Math.Floor(player.Velocity * game.sprintSpeed) : player.Velocity;
                player.Direction = false;
            }
            if (((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))))
            {
                game.sprinting = true;
            }
            else
            {
                game.sprinting = false;
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
                    if (!game.hasApple && game.arrivedOnSecondMap && player.X < 300 && player.X > 100)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Seber jablko";
                    }
                    break;
                case 1:
                    game.arrivedOnSecondMap = true;
                    if (!game.hasApple && game.storyPosition == 0)
                    {
                        game.currentMessage = "Bulmír chce jablko!!!\nJinak Tě nepustí dál!";
                    }
                    if (game.hasApple && game.storyPosition == 1)
                    {
                        game.currentMessage = "Máš jablko, které Bulmír chce,\ndej mu ho a on Tě pustí dál.";
                    }
                    if (game.storyPosition <= 2 && player.X > 740)
                    {
                        player.X = 740;
                    } 
                    if(game.hasApple && game.storyPosition == 1 && player.x > 700)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Daj mu jablko";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "*Udeř Bulmíra*";
                    }
                    if(game.storyPosition == 2)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Díky, brácho";


                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Ještě aby ne";


                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Pust mě dál, nebo tě zbiju";


                        
                    }

                    break;
                case 2:
                    if (game.storyPosition == 3 && player.X > 460)
                    {
                        player.X = 460;
                    }
                    if (game.storyPosition == 3 && player.X > 350)
                    {
                        game.currentMessage = "Aaaaa, pomoc, jsem malé dítě!\nMoji rodiče spadli do studny a nemůžou ven!";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Nasere";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Ok, pomůžu Ti";

                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Hmmm a co já s tím?";
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
            Debug.WriteLine(bg.Source.ToString());
            bg.Height = 350;
            //Panel.SetZIndex(bg, 1);
            Board.Children.Add(bg);

            Image playerImg = new Image();
            playerImg.Source = player.SpriteImage;
            playerImg.Width = 60;
            if(player.Direction)
            {
                player.Sprite = "player-right.png";
            }
            else
            {
                player.Sprite = "player-left.png";
            }
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
            if(game.currentMapNumber == 0 && game.arrivedOnSecondMap && player.X < 300 && player.X > 100)
            {
                if (!game.hasApple)
                {
                    game.hasApple = true;
                    game.currentMessage = "Sebral jsi jablko";
                    if(game.storyPosition == 0) game.storyPosition++;
                }
            }
            else if (game.currentMapNumber == 1 && game.storyPosition == 1 && game.hasApple && player.x > 700)
            {
                game.hasApple = false;
                game.storyPosition++;
                game.currentMessage = "Hmmmm, fajnový jablko";
            }
            else if (game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "Nz, pouštím Tě dál";
                game.storyPosition++;
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;
            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "No docela jo, takže mi pomůžeš, blbečku\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (game.storyPosition == 1 && player.x > 700)
            {
                game.currentMessage = "Achich ouvej, to bolí\nKdyž si Bulmíra praštil, sebral Ti jablko.";
                game.hasApple = false;
            }
            if (game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "No jo, no jo, tak jdi dál";
                game.storyPosition++;
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;
            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "Díky!!!\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

            if (game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "Tvl, ty jsi vostrej, jdi dál";
                game.storyPosition++;
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;

            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "No pomůžeš mi, jinak Tě nepustím dál!!!\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

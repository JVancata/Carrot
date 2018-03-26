using Carrot.Classes;
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
        public Player player = new Player("Knedlik", "hrac", 100, 1, 1, 4);
        Random random = new Random();
        public int windowWidth = 1000;
        public int windowHeight = 350;
        public List<NPC> NPCList = new List<NPC>();
        public List<Monster> MonsterList = new List<Monster>();
        public int frame = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Carrot Quest - "+player.Name;
            player.Attack = new NormalAttack();
            fillNpc();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            //timer
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            game.currentMapNumber = 7;
            game.currentMaxMapNumber = 8;
            game.storyPosition = 12;
            player.Lvl = 9;
            player.XP = 80;
            game.hasHoney = true;
            game.hasBlueberry = true;
            game.hasApple = true;
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
            NPCList.Add(new NPC("Kiddo", "npc", 2, 500, 0, 0, "character-kid.png"));
            NPCList.Add(new NPC("Metzen", "npc", 5, 800, -25, 0, "wizard-no-hat.png", false, 120, 200));
            NPCList.Add(new NPC("MedVěd", "npc", 8, 750, -65, 0, "bear-hat.png", false, 170, 250));
            NPCList.Add(new NPC("Karela", "npc", 9, 750, -15, 0, "princess.png", false, 110, 200));

            Monster vlk = new Monster("Vlček", "monster", 3, 500, 20, 0, 10, 100, "vlk-1.png");
            vlk.Attack = new WolfAttack();
            MonsterList.Add(vlk);

            Monster boar = new Monster("Prase", "monster", 6, 400, 20, 0, 10, 100, "boar.png", 80);
            boar.Attack = new BoarAttack();
            MonsterList.Add(boar);

            Monster bat = new Monster("Netopýr", "monster", 7, 400, -70, 0, 10, 100, "bat.png", 80);
            bat.Attack = new BatAttack();
            MonsterList.Add(bat);

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckInputs();
            switchMaps();
            Render();
            if (player.HP <= 0) resetGame();
        }
        public void switchMaps()
        {
            if (player.X <= 10 && game.currentMapNumber > 0 && game.canSwitch)
            {
                game.currentMapNumber--;
                player.X = windowWidth - 110;
                game.canSwitch = false;
            }
            else if (player.X >= windowWidth - 100 - player.Velocity && game.currentMapNumber < game.currentMaxMapNumber && game.canSwitch)
            {
                game.currentMapNumber++;
                player.X = 11;
                game.canSwitch = false;
            }
            else
            {
                game.canSwitch = true;
            }
            //Debug.WriteLine(game.currentMapNumber);
        }
        public void CheckInputs()
        {
            //64 - 514
            //Debug.WriteLine(player.X);
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
            if (game.hasBlueberry && game.storyPosition > 8 && player.HP < 100)
            {
                Button4.Visibility = Visibility.Visible;
                Button4.Content = "*sněz borůvku*";
            }
            if (game.hasApple && game.storyPosition > 8 && player.HP < 100)
            {
                Button4.Visibility = Visibility.Visible;
                Button4.Content = "*sněz jablko*";
            }
            if (game.hasHoney && game.storyPosition > 8 && player.HP < 100)
            {
                Button4.Visibility = Visibility.Visible;
                Button4.Content = "*sněz med*";
            }
            switch (game.currentMapNumber)
            {
                case 0:
                    if (!game.hasApple && game.arrivedOnSecondMap && player.X < 300 && player.X > 100)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*seber jablko*";
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
                    if (game.hasApple && game.storyPosition == 1 && player.x > 700)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Daj mu jablko";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "*udeř Bulmíra*";
                    }
                    if (game.storyPosition == 2)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Díky, brácho";


                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Ještě aby ne";


                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Pust mě dál, nebo tě zbiju";



                    }
                    //blueberry
                    if (game.storyPosition >= 4 && !game.hasBlueberry && player.X > 650 && player.X < 750)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*seber borůvku*";
                    }
                    break;
                case 2:
                    if (game.storyPosition == 3 && player.X > 440)
                    {
                        player.X = 440;
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

                        if (game.hasApple)
                        {
                            Button4.Visibility = Visibility.Visible;
                            Button4.Content = "*hoď po něm jablko*";
                        }
                    }
                    break;
                case 3:
                    if (game.storyPosition <= 5 && player.X > 370)
                    {
                        player.X = 370;
                    }
                    if (game.storyPosition == 4 && player.X > 300)
                    {
                        game.currentMessage = "Grrrrrrrrr, jsem vlk\nOdstup, nebo Tě eliminuji";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Nekřič, omg";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Aha";

                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Já eliminuju Tebe";

                        if (game.hasBlueberry)
                        {
                            Button4.Visibility = Visibility.Visible;
                            Button4.Content = "*dej vlkovi borůvku*";
                        }
                    }
                    if (game.storyPosition == 5 && player.X > 300)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*udeř vlka*";
                    }
                    if (game.storyPosition == 6 && player.X > 500)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*vytáhni rodiče ze studny*";
                    }
                    if (game.storyPosition == 7 && player.X > 500)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Proč jste tam jako byli?";
                    }
                    if (game.storyPosition == 8 && player.X > 500)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Lol, aha";
                    }
                    break;
                case 5:
                    if (game.storyPosition <= 11 && player.X > 740) player.X = 740;
                    if (game.storyPosition == 9 && player.X > 700)
                    {
                        game.currentMessage = "Ahhhh, jsem starý dědek Metzen.\nTy monstra v tomto proklatém lese mi sebrali můj milovaný čepec.";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Jejda";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Pust mě dál, dědo";

                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Jak ti můžu pomoct?";

                    }
                    if (game.storyPosition == 10 && player.X > 700)
                    {

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Oki";
                    }
                    if (game.storyPosition == 11 && player.X > 700)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*přečíst knihu*";
                    }
                    if (game.storyPosition == 31 && player.X > 700 && game.hasHat)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*dej mu čepec*";
                    }
                    break;
                case 6:
                    if (game.storyPosition >= 12 && player.X < MonsterList[1].X + 50 && MonsterList[1].X - 200 < player.X)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*udeř prase*";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "*kopni prase*";
                    }//650 - 800
                    else if (!game.hasHoney && game.killedBoars > 3 && game.storyPosition >= 12 && player.X > 650 && player.X < 800)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*seber med*";
                    }
                    break;
                case 7:
                    if (game.storyPosition >= 12 && player.X < MonsterList[2].X + 50 && MonsterList[2].X - 200 < player.X)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*udeř netopýra*";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "*kopni netopýra*";
                    }
                    break;
                case 8:
                    //bear is stopping him
                    if(game.storyPosition < 32 && player.X > 700)
                    {
                        player.X = 700;
                    }
                    if(game.storyPosition == 12 && player.X > 575)
                    {
                        game.currentMessage = "Brum brum, jsem MedVěd";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Čau, Méďo";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Pěkně hnusnej medvěd";

                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Dej sem ten čepec!";
                    }
                    if (game.storyPosition == 13 && player.X > 575 && player.Lvl >= 10)
                    {
                        game.currentMessage = "No, to je lepší! Já jsem MedVěd!";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Čau, Méďo";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Pěkně hnusnej medvěd";

                        Button3.Visibility = Visibility.Visible;
                        Button3.Content = "Dej sem ten čepec!";
                    }
                    if (player.X > 575)
                    {
                        //ok
                        if(game.storyPosition == 15)
                        {
                            game.currentMessage = "Čau, " + player.Name + ". Jak se máš?";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "V pohodě";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "Nic moc";

                            Button3.Visibility = Visibility.Visible;
                            Button3.Content = "Super, kámo";
                        }
                        else if(game.storyPosition == 16)
                        {
                            game.currentMessage = "Aha. A co potřebuješ?";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Ten čepec";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "Tvojí hučku";

                            Button3.Visibility = Visibility.Visible;
                            Button3.Content = "To, co máš na hlavě";
                        }

                        else if (game.storyPosition == 16)
                        {
                            game.currentMessage = "Co za to?";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Co chceš?";
                        }
                        else if(game.storyPosition == 17)
                        {
                            game.currentMessage = "Mám hlad. Dones mi něco dobrýho, ale ať je toho hodně.";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Tak fajn";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "*udeř méďu*";
                        }
                        //mean
                        else if(game.storyPosition == 20)
                        {
                            game.currentMessage = "Sám seš hnusnej.";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Dej mi tu čepici";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = ":(";

                        }
                        else if (game.storyPosition == 21)
                        {
                            game.currentMessage = "Cože po mně chceš?";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Tu čepici";
                        }
                        else if (game.storyPosition == 22)
                        {
                            game.currentMessage = "Mám hlad. Dones mi něco dobrýho, ať je toho hodně, pak Ti jí možná dám.";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Tak fajn";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "*udeř méďu*";
                        }
                        //greedy
                        else if (game.storyPosition == 25)
                        {
                            game.currentMessage = "Nedám.";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Pls";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "Prosím, méďo";

                            Button3.Visibility = Visibility.Visible;
                            Button3.Content = "Notak";
                        }
                        else if (game.storyPosition == 26)
                        {
                            game.currentMessage = "Mám hlad. Dones mi něco dobrýho, ať je toho hodně, pak Ti jí možná dám.";

                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "Ok";

                            Button2.Visibility = Visibility.Visible;
                            Button2.Content = "*udeř méďu*";
                        }
                        else if (game.storyPosition == 30 && game.hasApple && game.hasBlueberry && game.hasHoney)
                        {
                            Button1.Visibility = Visibility.Visible;
                            Button1.Content = "*podej mu to*";
                        }
                    }
                    break;
                case 9:
                    if (player.X > 700) player.X = 700;
                    if(game.storyPosition == 32 && player.X > 650)
                    {
                        game.currentMessage = "Achjo, achjo, jsem smutná";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Proč?";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Nasere";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Ty jsi chlap?";
                    }
                    else if (game.storyPosition == 33 && player.X > 650)
                    {
                        game.currentMessage = "Můj vlk se ztratil a je někde sám, smutný";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Noa?";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Proč máš vousy?";
                    }
                    else if (game.storyPosition == 34 && player.X > 650)
                    {
                        game.currentMessage = "Ach, kéžby mi někdo dal naději, že můj miláček žije";
                        if (game.princessDialogSaidWolfIsDead) game.currentMessage += "\n(notak, to jí neříkej, omg)";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Žije";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Je mrtvej";
                    }
                    else if (game.storyPosition == 35 && player.X > 650)
                    {
                        game.currentMessage = "Opravdu, půvabný cizinče? Opravdu žije?";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Jo";
                    }
                    else if(game.storyPosition == 36 && player.X > 650)
                    {
                        game.currentMessage = "Och, děkuji! Dám Ti cokoli, co budeš chtít!";

                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "Dej mi mrkev";
                    }
                    else if(game.storyPosition == 37 && game.hasCarrot)
                    {
                        game.currentMessage = "Získal jsi mrkev! Hra je u konce! Gratuluju!";
                    }
                    break;
                default:
                    break;
            }
            StoryLabel.Text = game.currentMessage;
        }
        public bool BattleSystemClick(Monster monster, Player player, int exp, int attackType)
        {
            Random random = new Random();
            int rand = random.Next(-2, 3);

            int damageDone = 0;
            if (attackType == 0) damageDone = player.Attack.Attack(player, monster, rand);
            else if (attackType == 1 && player.SecondAttack != null) damageDone = player.SecondAttack.Attack(player, monster, rand);

            game.currentMessage = "Uhodil jsi " + monster.Name + " a sebral mu " + damageDone + " zdraví";
            if (monster.HP > 0)
            {
                rand = random.Next(-2, 3);
                damageDone = monster.Attack.Attack(player, monster, rand);
                game.currentMessage += "\n" + monster.Name + " Tě uhodil nazpět a sebral Ti " + damageDone + " životů\n" + monster.Name + " zbývá " + monster.HP + " životů";
                return false;
            }
            else
            {
                game.currentMessage += "\nZabil si " + monster.Name + ". Získáváš " + exp + "xp";
                player.addXp(exp);
                return true;
            }
        }
        public void resetGame()
        {
            game = new Game();
            player = new Player("Knedlik", "hrac", 100, 1, 1, 10);
            NPCList = new List<NPC>();
            MonsterList = new List<Monster>();
            player.Attack = new NormalAttack();
            fillNpc();
            game.currentMessage = "Umřels.";
        }
        public void Render()
        {
            //Debug.WriteLine(player.X);

            Board.Children.Clear();
            MapInteraction();
            PlayerHP.Value = player.HP;
            Canvas.SetLeft(PlayerHP, player.X + 45);
            Canvas.SetTop(PlayerHP, 140);

            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/bg/" + "bg" + game.currentMapNumber + ".png", UriKind.Relative));
            bg.Height = 350;
            //Panel.SetZIndex(bg, 1);
            Board.Children.Add(bg);

            Image playerImg = new Image();
            playerImg.Source = player.SpriteImage;
            playerImg.Height = player.Height;
            playerImg.Width = player.Width;
            if (player.Direction)
            {
                player.Sprite = "player-right.png";
            }
            else
            {
                player.Sprite = "player-left.png";
            }

            frame++;

            if (game.hasApple)
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(@"assets/icons/" + "apple.png", UriKind.Relative));
                icon.Width = 20;
                Canvas.SetLeft(icon, 210);
                Canvas.SetTop(icon, 30);
                Board.Children.Add(icon);
            }
            if (game.hasBlueberry)
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(@"assets/icons/" + "blueberry.png", UriKind.Relative));
                icon.Width = 20;
                Canvas.SetLeft(icon, 235);
                Canvas.SetTop(icon, 30);
                Board.Children.Add(icon);
            }
            if (game.hasHoney)
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(@"assets/icons/" + "honey.png", UriKind.Relative));
                icon.Width = 20;
                Canvas.SetLeft(icon, 260);
                Canvas.SetTop(icon, 30);
                Board.Children.Add(icon);
            }
            if (game.hasHat)
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(@"assets/icons/" + "hat.png", UriKind.Relative));
                icon.Width = 20;
                Canvas.SetLeft(icon, 285);
                Canvas.SetTop(icon, 30);
                Board.Children.Add(icon);
            }
            if (game.currentMapNumber == 6)
            {
                Image hive = new Image();
                hive.Source = new BitmapImage(new Uri(@"assets/beehive/" + "hive" + ((int)Math.Floor((double)(frame / 25))) + ".png", UriKind.Relative));
                hive.Width = 200;
                Canvas.SetLeft(hive, 700);
                Canvas.SetTop(hive, 101);
                Board.Children.Add(hive);
            }
            if (game.currentMapNumber < 4 || game.currentMapNumber > 8)
            {
                Image sun = new Image();
                sun.Source = new BitmapImage(new Uri(@"assets/sun/" + "sun" + ((int)Math.Floor((double)(frame / 25))) + ".png", UriKind.Relative));
                sun.Width = 100;
                Canvas.SetLeft(sun, 20);
                Canvas.SetTop(sun, 10);
                Board.Children.Add(sun);
            }
            if (frame >= 99)
            {
                frame = 0;
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
                    image.Height = npc.Height;
                    image.Width = npc.Width;
                    Canvas.SetLeft(image, npc.X);
                    Canvas.SetTop(image, npc.Y + 150);
                    Board.Children.Add(image);
                }
            }
            /*try
            {
                var elements = MainCanvas.Children;
                foreach (Object child in elements)
                {
                    if (child is ProgressBar)
                    {
                        ProgressBar bar = (ProgressBar)child;
                        MainCanvas.Children.Remove(bar);
                    }
                }
            }
            catch { }*/
            foreach (Monster monster in MonsterList)
            {
                if (monster.Map == game.currentMapNumber && monster.HP > 0)
                {
                    Image image = new Image();
                    image.Source = monster.SpriteImage;
                    image.Height = monster.Height;
                    Canvas.SetLeft(image, monster.X);
                    Canvas.SetTop(image, monster.Y + 150);

                   /* ProgressBar hpBar = new ProgressBar
                    {
                        Minimum = 0,
                        Maximum = monster.MaxHp,
                        Value = monster.HP,
                        Width = 60,
                        Height = 8,
                        Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD18484")),
                        Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC20505")),
                        BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD18484"))
                    };
                    Canvas.SetLeft(hpBar, 50);
                    Canvas.SetTop(hpBar, 50);
                    Panel.SetZIndex(hpBar, 1000);
                    MainCanvas.Children.Add(hpBar);*/

                    Board.Children.Add(image);
                                       

                }
            }
            int neededXp = (int)Math.Pow(player.Lvl, 2);
            Level.Content = player.name + " - " + player.Lvl + " Lvl. " + player.XP + "/" + neededXp + " Xp";
            Debug.WriteLine("Map: "+ game.currentMapNumber );
            Debug.WriteLine("Story: " + game.storyPosition);

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (game.currentMapNumber == 0 && game.arrivedOnSecondMap && player.X < 300 && player.X > 100)
            {
                if (!game.hasApple)
                {
                    game.hasApple = true;
                    game.currentMessage = "Sebral jsi jablko";
                    if (game.storyPosition == 0) game.storyPosition++;
                }
            }
            else if (game.currentMapNumber == 1 && game.storyPosition == 1 && game.hasApple && player.x > 700)
            {
                game.hasApple = false;
                game.storyPosition++;
                game.currentMessage = "Hmmmm, fajnový jablko";
            }
            else if (game.currentMapNumber == 1 && game.storyPosition > 3 && player.X > 300 && !game.hasBlueberry)
            {
                game.hasBlueberry = true;
                game.currentMessage = "Sebral jsi borůvku";
            }
            else if (game.currentMapNumber == 1 && game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "Nz, pouštím Tě dál\nZískáváš 2 Xp";
                game.storyPosition++;
                player.addXp(2);
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;
            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "No docela jo, takže mi pomůžeš, blbečku\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 4 && player.X > 300)
            {
                game.currentMessage = "Štěk! Jdeme bojovat!";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 5 && player.X > 300)
            {
                bool result = BattleSystemClick(MonsterList[0], player, 8, 0);
                if (result) game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 6 && player.X > 500)
            {
                game.currentMessage = "Vytáhl jsi rodiče ze studny.";
                NPCList.Add(new NPC("Táta", "npc", 3, 500, 0, 0, "father-right.png"));
                NPCList.Add(new NPC("Máma", "npc", 3, 450, 0, 0, "mama2.png"));
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 7 && player.X > 500)
            {
                game.currentMessage = "Jsme tam spadli.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 8 && player.X > 500)
            {
                game.currentMessage = "No, každopádně díky moc.\nZískáváš 5xp";
                game.storyPosition++;
                game.currentMaxMapNumber += 2;
                player.addXp(5);
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 9 && player.X > 700)
            {
                game.currentMessage = "No, jejda. Pomož mi a já Tě pustím dál, příteli.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 10 && player.X > 700)
            {
                game.currentMessage = "Nenechám Tě jít jen tak. Tady máš knihu dle které se můžeš naučit i speciální útok, kterým budeš moct ty zvířata porazit.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 11 && player.X > 700)
            {
                game.currentMessage = "Naučil jsi se kop s otočkou, který sice způsobí drasticky vyšší poškození, ale samotného Tě zraní.";
                player.SecondAttack = new SpecialKickAttack();
                game.storyPosition++;
                game.currentMaxMapNumber++;
            }
            else if (game.currentMapNumber == 6 && game.storyPosition >= 12 && player.X < MonsterList[1].X + 50 && MonsterList[1].X - 200 < player.X)
            {
                bool result = BattleSystemClick(MonsterList[1], player, 10, 0);
                if (result) {
                    game.killedBoars++;
                    game.currentMessage += "\nZjevilo se nové";
                    MonsterList[1].HP = 100;
                    MonsterList[1].X = random.Next(100, 600);
                    if (game.killedBoars >= 5 && game.currentMaxMapNumber == 6) {
                        game.currentMaxMapNumber++;
                        game.currentMessage = "\nOtevřela se Ti nová lokace";
                    }
                }
            }
            else if (game.currentMapNumber == 6 && game.killedBoars > 0 && game.storyPosition >= 12 && player.X > 650 && player.X < 800)
            {
                game.hasHoney = true;
                game.currentMessage = "Sebral jsi med";
            }
            else if (game.currentMapNumber == 7 && game.storyPosition >= 12 && player.X < MonsterList[2].X + 50 && MonsterList[2].X - 200 < player.X)
            {
                bool result = BattleSystemClick(MonsterList[2], player, 10, 0);
                if (result)
                {
                    game.killedBats++;
                    game.currentMessage += "\nZjevil se nový";
                    MonsterList[2].HP = 100;
                    MonsterList[2].X = random.Next(100, 600);
                    if (game.killedBats >= 5 && game.currentMaxMapNumber == 7)
                    {
                        game.currentMaxMapNumber++;
                        game.currentMessage = "\nOtevřela se Ti nová lokace";
                    }
                }

            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 12 && player.X > 600 && player.Lvl < 10)
            {
                game.currentMessage = "Pff, kdo vůbec jsi? S takovým nenaexpeným noobem se ani nebudu bavit.";
                game.storyPosition+=2;
            }
            else if (game.currentMapNumber == 8 && (game.storyPosition == 12 || game.storyPosition == 13) && player.X > 600 && player.Lvl >= 10)
            {
                game.storyPosition = 15;
                game.currentMessage = "Čau, co chceš?";
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 14 && game.storyPosition < 17 && player.X > 575 && player.Lvl >= 10)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 20 && game.storyPosition < 22 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 25 && game.storyPosition < 26 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 14 && game.storyPosition < 18 && player.X > 575)
            {
                game.currentMessage = "Tak jdi už.";
                game.storyPosition = 30;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 22 && player.X > 575)
            {
                game.currentMessage = "Tak jdi už.";
                game.storyPosition = 30;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 26 && player.X > 575)
            {
                game.currentMessage = "Tak jdi už.";
                game.storyPosition = 30;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 30 && player.X > 575 && game.hasApple && game.hasBlueberry && game.hasHoney)
            {
                game.hasHoney = false;
                game.hasApple = false;
                game.hasBlueberry = false;
                game.currentMessage = "Díky, kámo! Tak tady máš čapku!";
                game.hasHat = true;
                var bear = NPCList.First(x => x.Name == "MedVěd");
                NPC bearTyped = (NPC)bear;
                bearTyped.Sprite = "bear-no-hat.png";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 31 && player.X > 700)
            {
                game.hasHat = false;
                game.currentMessage = "Děkuji, mladíku!\nOdemkla se Ti nová lokace.";
                game.storyPosition++;
                var metzen = NPCList.First(x => x.Name == "Metzen");
                NPC metzenTyped = (NPC)metzen;
                metzenTyped.Sprite = "wizard-hat.png";
                game.currentMaxMapNumber++;
            }

            else if(game.currentMapNumber == 9 && game.storyPosition >= 32 && game.storyPosition < 36 && player.X > 650)
            {
                game.storyPosition++;
            }

            else if (game.currentMapNumber == 9 && game.storyPosition == 36 && player.X > 650)
            {
                game.storyPosition++;
                game.hasCarrot = true;
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (game.currentMapNumber == 1 && game.storyPosition == 1 && player.x > 700)
            {
                game.currentMessage = "Achich ouvej, to bolí\nKdyž si Bulmíra praštil, sebral Ti jablko.";
                game.hasApple = false;
            }
            else if (game.currentMapNumber == 1 && game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "No jo, no jo, tak jdi dál\nZískáváš 2 Xp";
                player.addXp(2);
                game.storyPosition++;
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;
            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "Díky!!!\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 4 && player.X > 300)
            {
                game.currentMessage = "Jak jako aha?! Souboj na život a na smrt!";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 9 && player.X > 700)
            {
                game.currentMessage = "Pustím, mladíku. Ale musíš mi donést můj čepec.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 6 && game.storyPosition >= 12 && player.X < MonsterList[1].X + 50 && MonsterList[1].X - 200 < player.X)
            {
                bool result = BattleSystemClick(MonsterList[1], player, 10, 1);
                if (result)
                {
                    game.killedBoars++;
                    game.currentMessage += "\nZjevilo se nové";
                    MonsterList[1].HP = 100;
                    MonsterList[1].X = random.Next(100, 600);
                    if (game.killedBoars >= 5 && game.currentMaxMapNumber == 6)
                    {
                        game.currentMaxMapNumber++;
                        game.currentMessage = "\nOtevřela se Ti nová lokace";
                    }
                }
            }
            else if (game.currentMapNumber == 7 && game.storyPosition >= 12 && player.X < MonsterList[2].X + 50 && MonsterList[2].X - 200 < player.X)
            {
                bool result = BattleSystemClick(MonsterList[2], player, 10, 1);
                if (result)
                {
                    game.killedBats++;
                    game.currentMessage += "\nZjevil se nový";
                    MonsterList[2].HP = 100;
                    MonsterList[2].X = random.Next(100, 600);
                    if (game.killedBats >= 5 && game.currentMaxMapNumber == 7)
                    {
                        game.currentMaxMapNumber++;
                        game.currentMessage = "\nOtevřela se Ti nová lokace";
                    }
                }
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 12 && player.X > 600 && player.Lvl < 10)
            {
                game.currentMessage = "Pff, kdo vůbec jsi? S takovým nenaexpeným noobem se ani nebudu bavit.";
                game.storyPosition++;
            }
            else if  (game.currentMapNumber == 8 && (game.storyPosition == 13 || game.storyPosition == 12 ) && player.X > 600 && player.Lvl >= 10)
            {
                game.storyPosition = 20;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 15 && game.storyPosition < 17 && player.X > 575 && player.Lvl >= 10)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 14 && player.X > 575)
            {
                game.storyPosition = 20;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 20 && game.storyPosition < 22 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 25 && game.storyPosition < 26 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && (game.storyPosition == 17 || game.storyPosition == 22 || game.storyPosition == 26)  && player.X > 575)
            {
                game.currentMessage = "No tak to ani nezkoušej, kámo.";
                game.storyPosition = 30;
            }
            else if (game.currentMapNumber == 9 && game.storyPosition >= 32 && game.storyPosition < 34 && player.X > 650)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 9 && game.storyPosition == 34 && player.X > 650)
            {
                game.princessDialogSaidWolfIsDead = true;
            }
            else if (game.currentMapNumber == 9 && game.storyPosition >= 34 && game.storyPosition < 35 && player.X > 650)
            {
                game.storyPosition++;
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

            if (game.storyPosition == 2 && player.x > 700)
            {
                game.currentMessage = "Tvl, ty jsi vostrej, jdi dál\nZískáváš 2 Xp";
                game.storyPosition++;
                player.addXp(2);
                if (game.currentMaxMapNumber < game.maxMapNumber) game.currentMaxMapNumber++;

            }
            else if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350)
            {
                game.currentMessage = "No pomůžeš mi, jinak Tě nepustím dál!!!\nStudna je směrem na východ!";
                game.currentMaxMapNumber++;
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 4 && player.X > 300)
            {
                game.currentMessage = "Ani hovno, pojď bojovat!";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 5 && game.storyPosition == 9 && player.X > 700)
            {
                game.currentMessage = "Byl by jsi velice laskav, kdyby si ho našel, prosím.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition == 12 && player.X > 600 && player.Lvl < 10)
            {
                game.currentMessage = "Pff, kdo vůbec jsi? S takovým nenaexpeným noobem se ani nebudu bavit.";
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && (game.storyPosition == 13 ||game.storyPosition == 12) && player.X > 600 && player.Lvl >= 10)
            {
                game.storyPosition = 25;
                game.currentMessage = "Nedám, kámo";
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 15 && game.storyPosition < 17 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.currentMapNumber == 8 && game.storyPosition >= 25 && game.storyPosition < 26 && player.X > 575)
            {
                game.storyPosition++;
            }
            else if (game.storyPosition == 13 && player.X > 600 && player.Lvl >= 10)
            {
                game.storyPosition = 25;
                game.currentMessage = "Nedám, kámo";
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            
            if (game.currentMapNumber == 2 && game.storyPosition == 3 && player.X > 350 && game.hasApple)
            {
                game.currentMessage = "Sis moc nepomohl, stejně mi musíš pomoct.";
                game.currentMaxMapNumber++;
                game.storyPosition++;
                game.hasApple = false;
            }
            else if (game.currentMapNumber == 3 && game.storyPosition == 4 && player.X > 300)
            {
                game.currentMessage = "Cože, borůvku? Ty si někdy viděl žrát vlka borůvku?\nAle dík, bro. Nebudeme fightit.\nZískáváš 8xp";
                player.addXp(8);
                game.storyPosition += 2;
                game.hasBlueberry = false;
            }

            //everything else should be above the eating
            //eating honey
            else if (game.storyPosition > 8 && game.hasHoney && player.HP < 100)
            {
                game.currentMessage = "Snědl jsi med. Obnovil Ti 25% zdraví.";
                game.hasHoney = false;
                player.addHp(25);
            }
            //eating apple
            else if (game.storyPosition > 8 && game.hasApple && player.HP < 100)
            {
                game.currentMessage = "Snědl jsi jablko. Obnovilo Ti 15% zdraví.";
                game.hasApple = false;
                player.addHp(15);
            }
            //eating blueberry
            else if (game.storyPosition > 8 && game.hasBlueberry && player.HP < 100)
            {
                game.currentMessage = "Snědl jsi borůvku. Obnovila Ti 7% zdraví.";
                game.hasBlueberry = false;
                player.addHp(7);
            }
        }
    }
}

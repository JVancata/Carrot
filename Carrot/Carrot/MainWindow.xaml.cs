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
        static Player player = new Player("Knedlik", "hrac", 100, 1, 1, 10);
        public int windowWidth = 1000;
        public int windowHeight = 350;
        public List<NPC> NPCList = new List<NPC>();
        public List<Monster> MonsterList = new List<Monster>();
        public int frame = 0;
        public MainWindow()
        {
            InitializeComponent();
            player.Attack = new NormalAttack();
            fillNpc();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            //timer
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
            //game.currentMapNumber = 5;
            //game.currentMaxMapNumber = 6;
            //game.storyPosition = 12;
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

            Monster vlk = new Monster("Vlček", "monster", 3, 500, 20, 0, 10, 100, "vlk-1.png");
            vlk.Attack = new WolfAttack();
            MonsterList.Add(vlk);

            Monster boar = new Monster("Boar", "monster", 6, 400, 20, 0, 10, 100, "boar.png", 80);
            boar.Attack = new BoarAttack();
            MonsterList.Add(boar);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckInputs();
            switchMaps();
            Render();
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
                    break;
                case 6:
                    //Debug.WriteLine("player: " + player.X);
                    //Debug.WriteLine("boar: " + MonsterList[1].X);
                    if (game.storyPosition >= 12 && player.X < MonsterList[1].X + 50 && MonsterList[1].X - 200 < player.X)
                    {
                        Button1.Visibility = Visibility.Visible;
                        Button1.Content = "*udeř prase*";

                        Button2.Visibility = Visibility.Visible;
                        Button2.Content = "*kopni prase*";
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
        public void Render()
        {
            //Debug.WriteLine(player.X);

            Board.Children.Clear();
            MapInteraction();
            PlayerHP.Value = player.HP;
            Canvas.SetLeft(PlayerHP, player.X + 45);
            Canvas.SetTop(PlayerHP, 140);

            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/" + "bg" + game.currentMapNumber + ".png", UriKind.Relative));
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

            if (game.currentMapNumber == 6)
            {
                Image hive = new Image();
                hive.Source = new BitmapImage(new Uri(@"assets/beehive/" + "hive" + ((int)Math.Floor((double)(frame / 25))) + ".png", UriKind.Relative));
                hive.Width = 200;
                Canvas.SetLeft(hive, 700);
                Canvas.SetTop(hive, 101);
                Board.Children.Add(hive);
            }
            if (game.currentMapNumber < 4 || game.currentMapNumber > 5)
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

            foreach (Monster monster in MonsterList)
            {
                if (monster.Map == game.currentMapNumber && monster.HP > 0)
                {
                    Image image = new Image();
                    image.Source = monster.SpriteImage;
                    image.Height = monster.Height;
                    Canvas.SetLeft(image, monster.X);
                    Canvas.SetTop(image, monster.Y + 150);
                    Board.Children.Add(image);
                }
            }
            int neededXp = (int)Math.Pow(player.Lvl, 2);
            Level.Content = player.name + " - " + player.Lvl + " Lvl. " + player.XP + "/" + neededXp + " Xp";
            //Debug.WriteLine("Map: "+ game.currentMapNumber );
            //Debug.WriteLine("Story: " + game.storyPosition);

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
            else if (game.storyPosition == 2 && player.x > 700)
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
                game.currentMessage = "Nenechám Tě jít jentak. Tady máš knihu dle které se můžeš naučit i speciální útok, kterým budeš moct ty zvířata porazit.";
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
                if (result) game.killedBoars++;
            }


            
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (game.storyPosition == 1 && player.x > 700)
            {
                game.currentMessage = "Achich ouvej, to bolí\nKdyž si Bulmíra praštil, sebral Ti jablko.";
                game.hasApple = false;
            }
            else if (game.storyPosition == 2 && player.x > 700)
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
                if (result) game.killedBoars++;
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
                game.currentMessage = "Byl by jsi velice laskav. Najdi ho, prosím.";
                game.storyPosition++;
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

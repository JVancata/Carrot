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

namespace Carrot
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string currentMap = "bg2.png";
        static Player player = new Player("Knedlik", "hrac", 100, 1, 1);
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            Render();
        }
        public void Render()
        {
            Board.Children.Clear();

            Image bg = new Image();
            bg.Source = new BitmapImage(new Uri(@"assets/"+currentMap, UriKind.Relative));
            bg.Width = 550;

            Board.Children.Add(bg);

            Image image = new Image();
            image.Source = player.SpriteImage;
            image.Height = 100;
            Canvas.SetLeft(image, player.X);
            Canvas.SetBottom(image, player.Y);
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
            Render();
        }
    }
}

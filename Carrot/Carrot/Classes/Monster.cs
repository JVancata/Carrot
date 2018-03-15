using Carrot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Carrot
{
    public class Monster : Creature
    {
        private string spritePath;
        private BitmapImage sprite;
        private string defaultPath = "assets/";
        private int dmg;
        private int hp;
        private int map;
        private IAttack attack;
        private int width;
        private int height;
        public Monster(string name, string type, int map, int x, int y, int z, int dmg = 10, int hp = 100, string sprite = "vlk-1.png", int height = 95, int width = 150)
        {
            this.width = width;
            this.height = height;
            this.name = name;
            this.type = type;
            this.x = x;
            this.y = y;
            this.z = z;
            this.dmg = dmg;
            this.hp = hp;
            this.map = map;
            this.Sprite = sprite;
        }
        public string Sprite
        {
            get
            {
                return this.spritePath;
            }
            set
            {
                this.spritePath = value;

                string toSprite = defaultPath + value;

                this.sprite = new BitmapImage(new Uri(toSprite, UriKind.Relative));
            }
        }
        public BitmapImage SpriteImage
        {
            get
            {
                return this.sprite;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
        public int Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value;
            }
        }
        public bool Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }
        public int Map
        {
            get
            {
                return this.map;
            }
            set
            {
                this.map = value;
            }
        }
        public int HP
        {
            get
            {
                return this.hp;
            }
            set
            {
                this.hp = value;
            }
        }
        public int Dmg
        {
            get
            {
                return this.dmg;
            }
            set
            {
                this.dmg= value;
            }
        }
        public IAttack Attack
        {
            get
            {
                return this.attack;
            }
            set
            {
                this.attack = value;
            }
        }
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
    }
}
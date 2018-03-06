﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Carrot
{
    class Player : Creature
    {
        private string spritePath;
        private BitmapImage sprite;
        private string defaultPath = "assets/";
        private int velocity;
        private int dmg;
        private int xp;
        
        public Player(string name, string type, int x, int y, int z, int velocity = 3, bool direction = true, int dmg = 10, string sprite = "player-right.png")
        {
            this.name = name;
            this.type = type;
            this.x = x;
            this.y = y;
            this.z = z;
            this.velocity = velocity;
            this.dmg = dmg;
            this.direction = direction;

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
        public int Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
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
        public int Xp
        {
            get
            {
                return this.xp;
            }
            set
            {
                this.xp = value;
            }
        }
    }
}

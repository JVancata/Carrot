﻿using Carrot.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Carrot
{
    public class Player : Creature
    {
        private string spritePath;
        private BitmapImage sprite;
        private string defaultPath = "assets/player/";
        private int velocity;
        private int dmg;
        private int xp;
        private int hp;
        private IAttack attack;
        private IAttack secondAttack;
        private int lvl = 1;
        private int width;
        private int height;
        public Player(string name, string type, int x, int y, int z, int velocity = 3, bool direction = true, int dmg = 10, int hp = 100, string sprite = "player-right.png", int height = 95, int width = 150)
        {
            this.width = width;
            this.height = height;
            this.name = name;
            this.type = type;
            this.x = x;
            this.y = y;
            this.z = z;
            this.velocity = velocity;
            this.dmg = dmg;
            this.hp = hp;
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
        public int XP
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
        public void addXp(int xpToAdd)
        {
            int neededXp = (int)Math.Pow(this.lvl, 2);
            Debug.WriteLine("XP NEED: "+neededXp);
            Debug.WriteLine("XP HAD: " + this.xp);
            Debug.WriteLine("XP HAVE: " + this.xp);
            int holder = this.xp + xpToAdd;
            int levelsAdded = (int)Math.Floor((double)(holder / neededXp));
            Debug.WriteLine("LVL LEVELS ADDED: " + levelsAdded);
            if (levelsAdded == 1)
            {
                this.lvl++;
                this.xp += xpToAdd-neededXp;
            }
            else if (levelsAdded > 1)
            {
                this.lvl++;
                addXp(xpToAdd-neededXp);
            }
            else
            {
                this.xp += xpToAdd;
            }
            //Debug.WriteLine("XP have now: " + this.xp);

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
                this.dmg = value;
            }
        }
        public int Lvl
        {
            get
            {
                return this.lvl;
            }
            set
            {
                this.lvl = value;
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
        public IAttack SecondAttack {
            get
            {
                return this.secondAttack;
            }
            set
            {
                this.secondAttack = value;
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
        public void addHp(int hpToAdd)
        {
            if (this.hp + hpToAdd >= 100) this.hp = 100;
            else this.hp += hpToAdd;
        }
    }
}
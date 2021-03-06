﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class Game
    {
        public int currentMapNumber = 0;
        public int maxMapNumber = 9;
        public int currentMaxMapNumber = 1;
        public int storyPosition = 0;
        public bool canSwitch = true;
        public string currentMessage = "";
        public bool sprinting = false;
        public double sprintSpeed = 1.8;

        //game actions
        public bool hasApple = false;
        public bool hasBlueberry = false;
        public bool hasHoney = false;
        public bool hasHat = false;
        public bool hasCarrot = false;
        public bool princessDialogSaidWolfIsDead = false;
        public int appleChoice = 1;
        public bool arrivedOnSecondMap = false;
        public int killedBoars = 0;
        public int killedBats = 0;
    }
}

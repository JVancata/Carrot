using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class Game
    {
        public int currentMapNumber = 0;
        public int maxMapNumber = 5;
        public int currentMaxMapNumber = 1;
        public int storyPosition = 0;
        public bool canSwitch = true;
        public string currentMessage = "";
        public bool sprinting = false;
        public double sprintSpeed = 1.8;

        //game actions
        public bool hasApple = false;
        public bool hasBlueberry = false;
        public int appleChoice = 1;
        public bool arrivedOnSecondMap = false;
    }
}

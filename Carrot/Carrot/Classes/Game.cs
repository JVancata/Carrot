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
        public int maxMapNumber = 1;
        public int storyPosition = 0;
        public bool canSwitch = true;
        public string currentMessage = "";

        //game actions
        public bool hasApple = false;
        public bool arrivedOnSecondMap = false;
    }
}

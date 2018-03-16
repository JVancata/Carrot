using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class BoarAttack : IAttack
    {
        public void Attack(Player player, Monster monster, int random = 0)
        {   player.HP = player.HP - (monster.Dmg + random);
        }
    }
}

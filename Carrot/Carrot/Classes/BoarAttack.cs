using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class BoarAttack : IAttack
    {
        public int Attack(Player player, Monster monster, int random = 0)
        {
            int damageDone = (monster.Dmg + random);
            player.HP = player.HP - damageDone;
            return damageDone;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class SpecialKickAttack : IAttack
    {
        public int Attack(Player player, Monster monster, int random = 0)
        {
            int lvlDamage = player.Lvl * 4;
            Debug.WriteLine("xd");
            int damageDone = (player.Dmg + lvlDamage + random);
            monster.HP -= damageDone;
            player.HP -= player.Lvl > 20 ? 20 : player.Lvl;
            return damageDone;
        }
    }
}

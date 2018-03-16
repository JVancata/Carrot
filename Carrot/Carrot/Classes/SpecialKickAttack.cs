using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class SpecialKickAttack : IAttack
    {
        public void Attack(Player player, Monster monster, int random = 0)
        {
            int lvlDamage = player.Lvl * 4;
            monster.HP -= (player.Dmg + lvlDamage + random);
            player.HP -= player.Lvl > 20 ? 20 : player.Lvl;
        }
    }
}

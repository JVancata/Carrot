using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class NormalAttack : IAttack
    {
        public int Attack(Player player, Monster monster, int random = 0)
        {
            int lvlDamage = player.Lvl*2;
            int damageDone = (player.Dmg + lvlDamage + random);
            monster.HP = monster.HP - damageDone;
            return damageDone;
        }
    }
}

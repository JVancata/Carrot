using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class WolfAttack : IAttack
    {
        public void Attack(Player player, Monster monster)
        {
            player.HP = player.HP - monster.Dmg;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    class NormalAttack : IAttack
    {
        public void Attack(Player player, Monster monster)
        {
            monster.HP = monster.HP - player.Dmg;
        }
    }
}

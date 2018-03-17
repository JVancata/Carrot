using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot.Classes
{
    public interface IAttack
    {
        int Attack(Player player, Monster monster, int random = 0);
    }
}

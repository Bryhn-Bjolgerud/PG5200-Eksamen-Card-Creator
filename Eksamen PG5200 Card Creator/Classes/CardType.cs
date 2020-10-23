using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eksamen_PG5200_Card_Creator.Classes
{
    public class CardType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string cardType { get; set; }
        public int maxManaCost { get; set; }
        public int maxDamage { get; set; }
        public int maxHealth { get; set; }
        public byte[] typeImage { get; set; }
    }
}

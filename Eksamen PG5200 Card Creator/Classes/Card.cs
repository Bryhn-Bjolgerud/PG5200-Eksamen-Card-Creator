using SQLite;

namespace Eksamen_PG5200_Card_Creator.Classes
{
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string cardName { get; set; }
        public string cardType { get; set; }
        public string cardAbility { get; set; }
        public string manaCost { get; set; }
        public string damage { get; set; }
        public string health { get; set; }
        public byte[] cardImage{ get; set; }
    }
}

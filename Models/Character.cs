namespace rpg_api.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoint { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intellingence { get; set; } = 10;
        public RPGClass Class { get; set; } = RPGClass.Knight;
    }
}
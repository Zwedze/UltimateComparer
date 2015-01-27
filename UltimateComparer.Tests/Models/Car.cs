namespace UltimateComparer.Tests.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int Price { get; set; }
        public Color Color { get; set; }
    }

    public enum Color
    {
        Red,
        Blue,
        Green
    }
}
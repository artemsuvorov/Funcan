namespace Funcan.Domain
{
    public class Style
    {
        public class DisplayingType
        {
            public string Value { get; }

            private DisplayingType(string value) => Value = value;
            public static DisplayingType Line => new("Line");
            public static DisplayingType Dots => new("Dots");
        }

        public Color Color { get; }
        public DisplayingType Type { get; }

        public Style(Color color, DisplayingType type)
        {
            Color = color;
            Type = type;
        }
    }
}
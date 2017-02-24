using Zerds.Enums;

namespace Zerds.Graphics
{
    public struct BodyPart
    {
        public BodyPartType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public BodyPart(BodyPartType type, int width, int height)
        {
            Type = type;
            Width = width;
            Height = height;
        }
    }
}

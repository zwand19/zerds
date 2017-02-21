using Zerds.Enums;

namespace Zerds.Graphics
{
    public struct ZerdBodyPart
    {
        public ZerdBodyPartTypes Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ZerdBodyPart(ZerdBodyPartTypes type, int width, int height)
        {
            Type = type;
            Width = width;
            Height = height;
        }
    }
}

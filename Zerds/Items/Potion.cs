using Microsoft.Xna.Framework;
using Zerds.Entities;

namespace Zerds.Items
{
    public abstract class Potion : PickupItem
    {
        protected Potion(string file, Enemy dropper, bool cache = true) : base(file, dropper, cache)
        {
        }
    }
}

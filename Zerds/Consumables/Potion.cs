using Zerds.Entities;

namespace Zerds.Consumables
{
    public abstract class Potion : PickupItem
    {
        protected Potion(string file, Enemy dropper, bool cache = true) : base(file, dropper, cache)
        {
        }
    }
}

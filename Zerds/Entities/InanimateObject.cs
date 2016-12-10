namespace Zerds.Entities
{
    public abstract class InanimateObject : Entity
    {
        protected InanimateObject(string file) : base(file, true)
        {
            
        }

        public override void Draw()
        {

        }
    }
}

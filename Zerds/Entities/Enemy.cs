namespace Zerds.Entities
{
    public abstract class Enemy : Being
    {
        public int MinCreatedHealth { get; set; }
        public int MaxCreatedHealth { get; set; }
        public int MinCreatedMana { get; set; }
        public int MaxCreatedMana { get; set; }
        public bool Spawned { get; set; }

        public abstract void InitializeEnemy();
        public abstract void RunAI();
        public abstract void Spawn();

        protected Enemy(string file) : base(file, true)
        {
            
        }
    }
}

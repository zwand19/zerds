using System.Collections.Generic;
using System.Linq;

namespace Zerds.Graphics
{
    public class AnimationList
    {
        public List<Animation> Animations { get; set; }

        public AnimationList()
        {
            Animations = new List<Animation>();
        }

        public void Add(Animation animation)
        {
            if (Animations.Any(a => a.Name == animation.Name))
                throw new System.Exception("Invalid animation - already added");
            Animations.Add(animation);
        }

        public Animation Get(string name)
        {
            return Animations.First(a => a.Name == name);
        }

        public void ResetAll()
        {
            Animations.ForEach(a => a.ResetAnimation());
        }
    }
}

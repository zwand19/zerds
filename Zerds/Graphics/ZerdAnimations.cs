using System.Collections.Generic;
using Zerds.Enums;
using System.Linq;
using System;

namespace Zerds.Graphics
{
    public class ZerdAnimations
    {
        public Dictionary<string, Dictionary<ZerdBodyPartTypes, Animation>> Animations { get; set; }

        public ZerdAnimations()
        {
            Animations = new Dictionary<string, Dictionary<ZerdBodyPartTypes, Animation>>();
        }

        public void AddAnimation(Dictionary<ZerdBodyPartTypes, Animation> animation)
        {
            if (Animations.Keys.Contains(animation.Values.First().Name))
                throw new Exception("Invalid animation - already added");
            Animations[animation.Values.First().Name] = animation;
        }

        public void ResetAll()
        {
            foreach (var anim in Animations.Values.SelectMany(a => a.Values))
                anim.ResetAnimation();
        }

        public void Reset(string name)
        {
            foreach (var anim in Animations[name].Values)
                anim.ResetAnimation();
        }
    }
}

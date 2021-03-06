﻿using System;
using System.Collections.Generic;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.Graphics
{
    public static class ZerdAnimationHelpers
    {
        public static BodyPart Feet;
        public static BodyPart Hands;
        public static BodyPart Head;
        public static BodyPart Chest;

        public static ZerdAnimations GetAnimations()
        {
            var animations = new ZerdAnimations();
            Feet = new BodyPart(BodyPartType.Feet, 80, 100);
            Hands = new BodyPart(BodyPartType.Hands, 100, 160);
            Head = new BodyPart(BodyPartType.Head, 100, 100);
            Chest = new BodyPart(BodyPartType.Chest, 110, 60);

            animations.AddAnimation(GetDamagedAnimation());
            animations.AddAnimation(GetStandAnimation());
            animations.AddAnimation(GetWalkAnimation());

            return animations;
        }

        public static void AddDragonsBreathAnimation(Zerd zerd, Func<bool> casted, Func<bool> execute, Func<bool> makeMissile)
        {
            var animation = new Dictionary<BodyPartType, Animation>();
            var animationLength = AbilityConstants.DragonBreathDuration;

            var headAnimation = new Animation(AnimationTypes.FireBreath, Head);
            headAnimation.AddFrame(0, 1, animationLength);
            animation[BodyPartType.Head] = headAnimation;

            var chestAnimation = new Animation(AnimationTypes.FireBreath, Chest);
            chestAnimation.AddFrame(0, 0, TimeSpan.FromSeconds(0.04));
            chestAnimation.AddFrame(0, 0, TimeSpan.FromSeconds(0.04), execute);
            for (var i = 0; i < AbilityConstants.DragonBreathDuration.TotalSeconds / AbilityConstants.DragonBreathInterval.TotalSeconds; i++)
                chestAnimation.AddFrame(0, 0, AbilityConstants.DragonBreathInterval, makeMissile);
            chestAnimation.AddFrame(0, 0, TimeSpan.FromSeconds(0.04), casted);
            animation[BodyPartType.Chest] = chestAnimation;

            zerd.ZerdAnimations.AddAnimation(animation);
        }

        public static Dictionary<BodyPartType, Animation> GetCastingAnimation(string name, TimeSpan castTime, TimeSpan followThroughTime, Func<bool> executeFunc, Func<bool> castedFunc)
        {
            var animation = new Dictionary<BodyPartType, Animation>();
            var animationLength = castTime + followThroughTime;

            var headAnimation = new Animation(name, Head);
            headAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Head] = headAnimation;

            var chestAnimation = new Animation(name, Chest);
            chestAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Chest] = chestAnimation;

            var handsAnimation = new Animation(name, Hands);
            var timeIntoAnimation = TimeSpan.Zero;
            var handSpeed = TimeSpan.FromSeconds(0.1);
            var frame = 0;
            while (timeIntoAnimation < castTime)
            {
                handsAnimation.AddFrame(frame++ % 4 + 1, 0, handSpeed);
                timeIntoAnimation += handSpeed;
            }
            handsAnimation.AddFrame(0, 1, followThroughTime.Split(5), executeFunc);
            handsAnimation.AddFrame(1, 1, followThroughTime.Split(5));
            handsAnimation.AddFrame(2, 1, followThroughTime.Split(5));
            handsAnimation.AddFrame(3, 1, followThroughTime.Split(5));
            handsAnimation.AddFrame(4, 1, followThroughTime.Split(5));
            handsAnimation.AddFrame(4, 1, followThroughTime.Split(5), castedFunc);
            animation[BodyPartType.Hands] = handsAnimation;

            return animation;
        }

        private static Dictionary<BodyPartType, Animation> GetDamagedAnimation()
        {
            var animation = new Dictionary<BodyPartType, Animation>();
            var animationLength = TimeSpan.FromSeconds(5); //irrelevant

            var headAnimation = new Animation(AnimationTypes.Damaged, Head);
            headAnimation.AddFrame(1, 0, animationLength);
            animation[BodyPartType.Head] = headAnimation;

            var chestAnimation = new Animation(AnimationTypes.Damaged, Chest);
            chestAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Chest] = chestAnimation;

            var handAnimation = new Animation(AnimationTypes.Damaged, Hands);
            handAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Hands] = handAnimation;

            return animation;
        }

        private static Dictionary<BodyPartType, Animation> GetStandAnimation()
        {
            var animation = new Dictionary<BodyPartType, Animation>();
            var animationLength = TimeSpan.FromSeconds(5); //irrelevant

            var headAnimation = new Animation(AnimationTypes.Stand, Head);
            headAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Head] = headAnimation;

            var chestAnimation = new Animation(AnimationTypes.Stand, Chest);
            chestAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Chest] = chestAnimation;

            var handAnimation = new Animation(AnimationTypes.Stand, Hands);
            handAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Hands] = handAnimation;

            return animation;
        }

        private static Dictionary<BodyPartType, Animation> GetWalkAnimation()
        {
            var animation = new Dictionary<BodyPartType, Animation>();
            var animationLength = TimeSpan.FromSeconds(1);

            var feetAnimation = new Animation(AnimationTypes.Move, Feet);
            feetAnimation.AddFrame(0, 0, animationLength.Split(8));
            feetAnimation.AddFrame(1, 0, animationLength.Split(8));
            feetAnimation.AddFrame(2, 0, animationLength.Split(8));
            feetAnimation.AddFrame(1, 0, animationLength.Split(8));
            feetAnimation.AddFrame(0, 0, animationLength.Split(8));
            feetAnimation.AddFrame(0, 1, animationLength.Split(8));
            feetAnimation.AddFrame(1, 1, animationLength.Split(8));
            feetAnimation.AddFrame(0, 1, animationLength.Split(8));
            animation[BodyPartType.Feet] = feetAnimation;

            var headAnimation = new Animation(AnimationTypes.Move, Head);
            headAnimation.AddFrame(0, 0, animationLength);
            animation[BodyPartType.Head] = headAnimation;

            var chestAnimation = new Animation(AnimationTypes.Move, Chest);
            chestAnimation.AddFrame(0, 0, animationLength.Split(8));
            chestAnimation.AddFrame(1, 0, animationLength.Split(8));
            chestAnimation.AddFrame(0, 1, animationLength.Split(8));
            chestAnimation.AddFrame(1, 0, animationLength.Split(8));
            chestAnimation.AddFrame(0, 0, animationLength.Split(8));
            chestAnimation.AddFrame(1, 1, animationLength.Split(8));
            chestAnimation.AddFrame(0, 2, animationLength.Split(8));
            chestAnimation.AddFrame(1, 1, animationLength.Split(8));
            animation[BodyPartType.Chest] = chestAnimation;

            var handAnimation = new Animation(AnimationTypes.Move, Hands);
            handAnimation.AddFrame(0, 0, animationLength.Split(8));
            handAnimation.AddFrame(0, 2, animationLength.Split(8));
            handAnimation.AddFrame(1, 2, animationLength.Split(8));
            handAnimation.AddFrame(0, 2, animationLength.Split(8));
            handAnimation.AddFrame(0, 0, animationLength.Split(8));
            handAnimation.AddFrame(2, 2, animationLength.Split(8));
            handAnimation.AddFrame(3, 2, animationLength.Split(8));
            handAnimation.AddFrame(2, 2, animationLength.Split(8));
            animation[BodyPartType.Hands] = handAnimation;

            return animation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Graphics/Unit Sprite Map", order = 0)]
    public class UnitSpriteMap : ScriptableObject
    {
        public const int Directions = 16;

        private const float AngleStep = 360f / Directions;
        
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private List<UnitAnimation> _animations;
        [SerializeField] private float _spriteHeight;

        private Dictionary<string, UnitAnimation> _animationsDic;
        
        public float SpriteHeight => _spriteHeight;
        
        public Sprite GenericSprite => GetSprite("idle", 0, 225);

        public Sprite GetSprite(string action, float timestamp, float angle)
        {
            _animationsDic ??= AnimationsToDictionary(_animations);

            if ( ! _animationsDic.TryGetValue(action, out UnitAnimation animation))
            {
                _animationsDic = AnimationsToDictionary(_animations);
                if ( ! _animationsDic.TryGetValue(action, out animation))
                {
                    _animationsDic = AnimationsToDictionary(_animations);
                    return null;
                }
            }

            int frame = Mathf.RoundToInt(timestamp / animation.FramesTimeGap);
            frame = animation.Loop
                ? frame.RepeatIndex(animation.Frames.Length)
                : Mathf.Min(frame, animation.Frames.Length - 1);
            
            angle = angle.Snap(AngleStep);
            int direction = Mathf.RoundToInt(angle / AngleStep).RepeatIndex(Directions);
            
            return animation.GetSprite(frame, direction);
        }

        private Dictionary<string, UnitAnimation> AnimationsToDictionary(List<UnitAnimation> animations)
        {
            return animations.ToDictionary(a => a.Action, a => a);
        }

        [Button("Update Animations")]
        public void UpdateAnimations()
        {
            Dictionary<string, List<UnitRawFrame>> rawAnimations = new();
            
            foreach (Sprite sprite in _sprites)
            {
                string[] args = sprite.name.Split('_');
                string animation = args[0];
                int direction = int.Parse(args[1]);
                int frame = int.Parse(args[2]);
                
                UnitRawFrame rawFrane = new(frame, direction, sprite);
                
                if (rawAnimations.TryGetValue(animation, out List<UnitRawFrame> rawFrames))
                    rawFrames.Add(rawFrane);
                else
                    rawAnimations.Add(animation, new List<UnitRawFrame> { rawFrane });
            }

            foreach (KeyValuePair<string, List<UnitRawFrame>> rawAnimation in rawAnimations)
            {
                int framesCount = rawAnimation.Value.Max(a => a.Frame) + 1;

                UnitAnimationFrame[] animationFrames = new UnitAnimationFrame[framesCount];
                
                for (int i = 0; i < framesCount; i++)
                {
                    UnitRawFrame[] directions = rawAnimation.Value.Where(a => a.Frame == i).ToArray();
                    directions = directions.OrderBy(d => d.Direction).ToArray();
                    animationFrames[i] = new UnitAnimationFrame(directions.Select(a => a.Sprite).ToArray());
                }

                UnitAnimation animation = _animations.FirstOrDefault(a => a.Name.Equals(rawAnimation.Key));
                if (animation != null)
                {
                    animation.UpdateFrames(animationFrames);
                }
                else
                {
                    animation = new UnitAnimation(rawAnimation.Key, animationFrames);
                    _animations.Add(animation);
                }
            }
        }
        
        private struct UnitRawFrame
        {
            public int Frame { get; private set; }
            public int Direction { get; private set; }
            public Sprite Sprite { get; private set; }

            public UnitRawFrame(int frame, int direction, Sprite sprite)
            {
                Frame = frame;
                Direction = direction;
                Sprite = sprite;
            }
        }

        private void OnValidate()
        {
            HashSet<string> actions = _animations.Select(a => a.Action).ToHashSet();
            if (actions.Count != _animations.Count)
                throw new ArgumentException("Animations must have unique Actions");
        }
    }
    
    [Serializable]
    public class UnitAnimation
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Action { get; private set; }
        [field: SerializeField] public float FrameRate { get; private set; } = 5;
        [field: SerializeField] public bool Loop { get; private set; } = true;
        [field: SerializeField] public UnitAnimationFrame[] Frames { get; private set; }
        
        public float FramesTimeGap => FrameRate == 0 ? 0.1f : (1 / FrameRate);

        public UnitAnimation(string name, UnitAnimationFrame[] frames)
        {
            Name = name;
            Frames = frames;
        }

        public Sprite GetSprite(int frame, int direction) => Frames[frame].GetDirection(direction);

        public void UpdateFrames(UnitAnimationFrame[] frames)
        {
            Frames = frames;
        }
    }

    [Serializable]
    public class UnitAnimationFrame
    {
        [field: SerializeField] public Sprite[] Directions { get; private set; }
        
        public UnitAnimationFrame(Sprite[] direction)
        {
            if (direction.Length != UnitSpriteMap.Directions && direction.Length != 1)
                throw new ArgumentException($"Incorrect number of Directions. Must be {UnitSpriteMap.Directions} or 1");
            Directions = direction;
        }

        public Sprite GetDirection(int direction) => Directions.Length == 1 ? Directions[0] : Directions[direction];
    }
}
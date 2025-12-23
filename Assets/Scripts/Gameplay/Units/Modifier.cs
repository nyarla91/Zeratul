using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Units
{
    public class Modifier
    {
        private Dictionary<object, ModifierEntry> _entries = new();
        
        public float DefaultValue { get; }
        
        public float Value
        {
            get
            {
                Func<float, float>[] processors = _entries.Values.OrderBy(e => e.Priority).Select(e => e.Processor).ToArray();
                float result = DefaultValue;
                foreach (Func<float, float> processor in processors)
                {
                    result = processor(result);
                }
                return result;
            }
        }

        public Modifier(float defaultValue = 1)
        {
            DefaultValue = defaultValue;
        }

        public void AddEntry(ModifierEntry entry)
        {
            _entries.TryAdd(entry.Source, entry);
        }

        public void RemoveEntry(object source)
        {
            _entries.Remove(source);
        }
    }

    public class ModifierEntry
    {
        public object Source { get; }
        public Func<float, float> Processor { get; }
        public int Priority { get; }

        public ModifierEntry(object source, Func<float, float> processor, int priority)
        {
            Source = source;
            Processor = processor;
            Priority = priority;
        }
    }
}
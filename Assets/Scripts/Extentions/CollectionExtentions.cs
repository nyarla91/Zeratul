using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Extentions
{
    public static class CollectionExtentions
    {
        public static T Random<T>(this IEnumerable<T> collection) => collection.Random<T>(1)[0];

        public static List<T> Random<T>(this IEnumerable<T> collection, int count)
        {
            List<T> list = collection.ToList();
            List<T> choosed = new List<T>();
            for (int i = 0; i < count; i++)
            {
                if (list.Count > 0)
                {
                    T element = list[UnityEngine.Random.Range(0, list.Count)];
                    choosed.Add(element);
                    list.Remove(element);
                }
                else
                {
                    break;
                }
            }
            return choosed;
        }

        public static List<T> TakeAwayRandom<T>(ref List<T> collection, int ammount)
        {
            List<T> choosed = new List<T>();
            for (int i = 0; i < ammount; i++)
            {
                if (collection.Count > 0)
                {
                    T element = collection[UnityEngine.Random.Range(0, collection.Count)];
                    choosed.Add(element);
                    collection.Remove(element);
                }
                else
                {
                    break;
                }
            }
            return choosed;
        }

        public static T[] Copy<T>(this T[] originCollection)
        {
            T[] finalCollection = new T[originCollection.Length];
            for (int i = 0; i < finalCollection.Length; i++)
            {
                finalCollection[i] = originCollection[i];
            }
            return finalCollection;
        }
        
        public static T[] TakeRange<T>(this IEnumerable<T> collection, int from /*inclusive*/, int to /*exclusive*/)
        {
            T[] array = collection.ToArray();
            if (from < 0 || from > array.Length || to < 0 || to > array.Length)
            {
                throw new IndexOutOfRangeException();
            }
            if (from > to)
            {
                throw new Exception("'from' argument must lesser or equal then 'to' argument");
            }

            T[] final = new T[to - from];
            for (int i = from; i < to; i++)
            {
                final[i - from] = array[i];
            }
            return final;
        }

        public static T[] Shuffle<T>(this List<T> collection) => collection.OrderBy(t => Guid.NewGuid()).ToArray();

        public static int RepeatIndex(this int index, int length)
        {
            if (length == 0)
                return 0;
            
            while (index < 0)
                index += length;
            while (index >= length)
                index -= length;
            return index;
        }

        public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T element in collection)
                action.Invoke(element);
        }

        public static T GetIndexOrLast<T>(this IEnumerable<T> enumerable, int index)
        {
            T[] array = enumerable.ToArray();
            return index < array.Length ? array[index] : array.Last();
        }

        public static IEnumerable<T> Fill<T>(int length, Func<int, T> filler)
        {
            T[] array = new T[length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = filler.Invoke(i);
            }
            return array;
        }

        public static T[] ClearNull<T>(this IEnumerable<T> source) => source.Where(element => element != null).ToArray();

        public static T MaxElement<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            return source.MinElement(element => - selector(element));
        }
        
        public static T MinElement<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            T[] array = source.ToArray();
            T result = default;
            float minValue = Single.MaxValue;
            foreach (T element in array)
            {
                float value = selector.Invoke(element);
                if (value >= minValue)
                    continue;
                minValue = value;
                result = element;
            }
            return result;
        }

        public static string Enumerate<T>(this IEnumerable<T> list, string separator = ", ", string endWith = "", Func<T, string> toString = null)
        {
            T[] array = list.ToArray();
            toString ??= t => t?.ToString();
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (i > 0)
                    result += separator;
                result += toString.Invoke(array[i]);
            }
            result += endWith;
            return result;
        }
    }
}

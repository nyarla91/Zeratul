using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Source.Extentions
{
    public static class CollectionExtentions
    {
        public static T PickRandom<T>(this IEnumerable<T> collection) => collection.PickRandom<T>(1)[0];

        public static List<T> PickRandom<T>(this IEnumerable<T> collection, int ammount)
        {
            List<T> list = collection.ToList();
            List<T> choosed = new List<T>();
            for (int i = 0; i < ammount; i++)
            {
                if (list.Count > 0)
                {
                    T element = list[Random.Range(0, list.Count)];
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

        public static List<T> TakeAwayRandomElements<T>(ref List<T> collection, int ammount)
        {
            List<T> choosed = new List<T>();
            for (int i = 0; i < ammount; i++)
            {
                if (collection.Count > 0)
                {
                    T element = collection[Random.Range(0, collection.Count)];
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

        public static string CollectionToString<T>(T[] collection)
        {
            string result = "";
            for (int i = 0; i < collection.Length; i++)
            {
                result += collection[i].ToString();
                if (i < collection.Length - 1)
                    result += ",\n";
            }
            return result;
        }

        public static T[] CreateArrayOfContent<T>(int length, T content)
        {
            T[] collection = new T[length];
            for (int i = 0; i < collection.Length; i++)
            {
                collection[i] = content;
            }
            return collection;
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

        public static bool HasIndex<T>(this IEnumerable<T> collection, int index) => index >= 0 && index < collection.ToArray().Length;

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

        public static T FirstValid<T>(this IEnumerable<T> collection, Predicate<T> validator, T defaultValue)
        {
            T[] array = collection.ToArray();
            foreach (T element in array)
            {
                if (validator.Invoke(element))
                    return element;
            }
            return defaultValue;
        }

        public static bool TryRemove<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
                return false;
            list.Remove(element);
            return true;
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

        public static string ToStringList<T>(this IEnumerable<T> source)
        {
            T[] array = source.ToArray();
            string result = "";
            for (var i = 0; i < array.Length; i++)
            {
                result += $"{array[0]} ";
                if (i < array.Length - 1)
                    result += ",";
            }
            return result;
        }

        public static IEnumerable<T> ClearCopies<T>(this IEnumerable<T> source)
        {
            T[] array = source.ToArray();
            List<T> result = new();
            foreach (T element in array)
            {
                if (result.Contains(element))
                    continue;
                result.Add(element);
            }
            return result;
        }
        
        public static T[] ClearNull<T>(this IEnumerable<T> source) => source.Where(element => element != null).ToArray();
    }
}

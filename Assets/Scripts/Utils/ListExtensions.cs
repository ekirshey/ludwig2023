
using System.Collections.Generic;
using UnityEngine;

namespace SadBrains.Utils
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static T RandomElement<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        
        public static T PopRandomElement<T>(this IList<T> list)
        {
            var idx = Random.Range(0, list.Count);
            var element = list[idx];
            list.RemoveAt(idx);
            return element;
        }
    }
}
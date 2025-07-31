using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Extensions.Collections
{
    public static class ListExtensions
    {
        public static T RandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new System.InvalidOperationException("List is null or empty");

            return list[Random.Range(0, list.Count)];
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ARAM.Utils
{

    public static class DebugExtensions
    {
        public static void DebugShowList<T>(IEnumerable<T> list)
        {
            if (list == null)
            {
                Debug.Log("null");
                return;
            }
            var enumerable = list as T[] ?? list.ToArray();
            Debug.Log("{" + (enumerable.Any() ? string.Join(", ", enumerable.Select(obj => obj != null ? obj.ToString() : "")) : "") + "}");
        }

        public static void DebugShowList<T>(IEnumerable<IEnumerable<T>> lists)
        {
            if (lists == null)
            {
                Debug.Log(null);
                return;
            }
            var enumerable = lists as IEnumerable<T>[] ?? lists.ToArray();
            if(enumerable.Any())
                Debug.Log(enumerable.Aggregate("", (current, list)
                    => current + ("{" + string.Join(", ", list.Select(obj => obj.ToString())) + "} ")));
        }
    }
}


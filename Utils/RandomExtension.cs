using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARAM.Utils
{
    public static class RandomExtension
    {
        public static Vector2 OnUnitCircle()
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            var x = Mathf.Sin(angle);
            var y = Mathf.Cos(angle);

            return new Vector2 (x, y);
        }
        
        public static int Lotto(this IEnumerable<int> itemWeightPairs)
        {
            var sortedPairs = itemWeightPairs.OrderByDescending(x => x).ToArray();

            var total = sortedPairs.Select(x => x).Sum();

            var randomPoint = Random.Range(0, total);

            var cnt = 0;
            foreach (var elem in sortedPairs)
            {
                if (randomPoint < elem)
                    return cnt;
                
                randomPoint -= elem;
                cnt++;
            }
            
            return sortedPairs.Length - 1;
        }
        
        public static int Lotto(this IEnumerable<float> itemWeightPairs)
        {
            var sortedPairs = itemWeightPairs.OrderByDescending(x => x).ToArray();

            var total = sortedPairs.Select(x => x).Sum();

            var randomPoint = Random.Range(0, total);

            var cnt = 0;
            foreach (var elem in sortedPairs)
            {
                if (randomPoint < elem)
                    return cnt;
                
                randomPoint -= elem;
                cnt++;
            }

            return sortedPairs.Length - 1;
        }
    }
}
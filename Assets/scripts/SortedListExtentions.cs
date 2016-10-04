using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    static class SortedListExtentions
    {
        public static T EqualOrNextGreater<T>(this SortedList<float, T> prefabsByDamage, float token)
        {
            var shotPrefab = prefabsByDamage.Values[0];
            foreach (KeyValuePair<float, T> shot in prefabsByDamage)
            {
                if (shot.Key > token)
                    return shotPrefab;

                shotPrefab = shot.Value;
            }

            return shotPrefab;
        }
    }
}

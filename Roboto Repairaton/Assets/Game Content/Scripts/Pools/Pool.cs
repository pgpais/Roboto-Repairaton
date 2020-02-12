using UnityEngine;

/// <summary>
/// Defines a pool, a random collection of objects that exist and can be picekd at random.
/// </summary>
namespace WhalesAndGames.Pools
{
    public class Pool
    {
        /// <summary>
        /// Makes a new generation and picks a variable of any given table.
        /// </summary>
        public static T Fetch<T>(PoolTable poolTable) where T : Object
        {
            // Defines local variables for this calculation.
            int totalChanceSum = 0;
            int[] intervals = new int[poolTable.poolVariables.Count + 1];

            #region Weight Sums
            // Gets the weight of each variable and get's the interval of each object.
            for (int i = 0; i < poolTable.poolVariables.Count; i++)
            {
                totalChanceSum += poolTable.poolVariables[i].chance;
                intervals[i] = totalChanceSum;
            }
            #endregion

            // Generates a random value from all the weight limit.
            int randomValue = Random.Range(0, totalChanceSum + 1);

            #region Interval Calculations
            // Checks if the value is in any interval.
            for (int i = 0; i < intervals.Length - 1; i++)
            {
                // Assigns the upper and lower limit of the random check.
                int lowerLimit = 0;
                int upperLimit = 0;

                // Fail-safe in case this is the first object being checked, so it doesn't give a non-existing
                // memory error.
                if (i == 0)
                {
                    lowerLimit = 0;
                }
                else
                {
                    lowerLimit = intervals[i - 1];
                }

                // Assigns the upper limit.
                upperLimit = intervals[i];

                // Checks if the random value belong to this limit.
                if (randomValue >= lowerLimit && randomValue <= upperLimit)
                {
                    if(poolTable.poolVariables[i].variable == null)
                    {
                        return null;
                    }

                    return (T)poolTable.poolVariables[i].variable;
                }
            }
            #endregion

            /* Returns null in worse case scenario. This only happens if we do something stupid.
            / Let's not do something stupid.
            / Please. */
            Debug.Log("Pool didn't find a variable!");
            return null;
        }
    }
}

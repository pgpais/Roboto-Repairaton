using UnityEngine;

namespace WhalesAndGames.Pools
{
    /// <summary>
    /// Template for a generator instance. To be used together with other generator classes.
    /// </summary>
    [System.Serializable]
    public class PoolVariable
    {
        public Object variable;
        public int chance;

        public PoolVariable(Object group, int chance)
        {
            this.variable = group;
            this.chance = chance;
        }
    }
}

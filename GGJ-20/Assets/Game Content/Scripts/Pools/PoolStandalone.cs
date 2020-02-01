using UnityEngine;
using System.Collections;

namespace WhalesAndGames.Pools
{
    /// <summary>
    /// Scriptable Object that holds an independant pool table.
    /// </summary>
    [CreateAssetMenu(fileName = "New Pool Table", menuName = "Pool Table")]
    public class PoolStandalone : ScriptableObject
    {
        public PoolTable poolTable;
    }
}

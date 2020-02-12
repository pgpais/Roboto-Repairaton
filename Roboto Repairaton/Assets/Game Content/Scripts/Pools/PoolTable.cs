using UnityEngine;
using System.Collections.Generic;

namespace WhalesAndGames.Pools
{
    /// <summary>
    /// Scriptable Object that holds the different variables that can be generated.
    /// </summary>
    [System.Serializable]
    public class PoolTable
    {
        // List of Variables that can be picked up from the generation.
        public List<PoolVariable> poolVariables = new List<PoolVariable>();

        /// <summary>
        /// Used for adding a new variable to this list.
        /// </summary>
        public void AddVariable(PoolVariable newVariable)
        {
            // Adds a new variable to the pool chance.
            if (newVariable.variable != null)
            {
                PoolVariable existingVariable = poolVariables.Find(x => x.variable == newVariable.variable);
                if (existingVariable != null)
                {
                    existingVariable.chance += newVariable.chance;
                }
                else
                {
                    poolVariables.Add(newVariable);
                }
            }
            else
            {
                Debug.LogError("Variable has no variable assigned!");
            }
        }

        /// <summary>
        /// Checks if a variable already exists on the pool.
        /// </summary>
        public bool Contains(Object checkObject)
        {
            // Checks each variable in the list until it finds the Remove Variable.
            PoolVariable existingVariable = poolVariables.Find(x => x.variable == checkObject);
            if (existingVariable != null)
            {
                return true;
            }

            // Action is not in the list, let's go back.
            return false;
        }

        /// <summary>
        /// Used for removing a variable from the pool.
        /// </summary>
        public void RemoveVariable(Object removeVariable)
        {
            // Checks each variable in the list until it finds the Remove Variable.
            foreach (PoolVariable variable in poolVariables)
            {
                if (Equals(variable.variable, removeVariable))
                {
                    poolVariables.Remove(variable);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets a list of variables as a specific type.
        /// TODO: Fix to be better acessible.
        /// </summary>
        public List<T> GetVariablesAsList<T>() where T : UnityEngine.Object
        {
            List<T> list = new List<T>();
            foreach(PoolVariable variable in poolVariables)
            {
                if(variable.variable.GetType() == typeof(T))
                {
                    list.Add((T)variable.variable);
                }
            }

            return list;
        }
    }
}

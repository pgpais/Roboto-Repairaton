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
        /// Adds a variable to the pool from unconstructed objects. If variable already exists, changes it chance.
        /// </summary>
        public void AddOrChangeVariable(Object newObject, int chance)
        {
            if(newObject == null)
            {
                Debug.LogError("Can't assigned an empty variable!");
                return;
            }

            PoolVariable existingVariable = poolVariables.Find(x => x.variable == newObject);
            if (existingVariable != null)
            {
                existingVariable.chance = chance;
            }
            else
            {
                PoolVariable poolVariable = new PoolVariable(newObject, chance);
                poolVariables.Add(poolVariable);
            }
        }

        /// <summary>
        /// Adds a new variable to the pool. If variable already exists, changes it chance.
        /// </summary>
        public void AddOrChangeVariable(PoolVariable newVariable)
        {
            // Adds a new variable to the pool chance.
            if (newVariable.variable == null)
            {
                Debug.LogError("Variable has no variable assigned!");
                return;
            }

            PoolVariable existingVariable = poolVariables.Find(x => x.variable == newVariable.variable);
            if (existingVariable != null)
            {
                existingVariable.chance = newVariable.chance;
            }
            else
            {
                poolVariables.Add(newVariable);
            }
        }

        /// <summary>
        /// Checks if a variable already exists on the pool.
        /// </summary>
        public bool ContainsVariable(Object checkObject)
        {
            // Checks each variable in the list until it finds the Remove Variable.
            PoolVariable existingVariable = poolVariables.Find(x => x.variable == checkObject);
            if (existingVariable != null)
            {
                return true;
            }

            // The variable does not exist in the pool.
            Debug.LogError("There is no variable with " + checkObject + " assigned to it!");
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
        /// Gets a list of the variables as long as if they're of the parsed type.
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

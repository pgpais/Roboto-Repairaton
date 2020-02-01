using UnityEngine;

/// <summary>
/// Defines a singleton behaviour which extends Monobehaviour to be it's own instance type.
/// </summary>
public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            return _instance;
        }
    }
}
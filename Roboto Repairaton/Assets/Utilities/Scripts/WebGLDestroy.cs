using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhalesAndGames.Shipyard.Helpers
{
    /// <summary>
    /// Destroys this object if the current platform is WebGL.
    /// </summary>
    public class WebGLDestroy : MonoBehaviour
    {
#if UNITY_WEBGL
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Destroy(gameObject);
        }
#endif
    }
}

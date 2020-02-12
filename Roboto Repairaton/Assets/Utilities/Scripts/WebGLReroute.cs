using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WhalesAndGames.Shipyard.Helpers
{
    /// <summary>
    /// Used in WebGL to reroute buttons when they are deleted. 
    /// </summary>
    public class WebGLReroute : MonoBehaviour
    {
        [Header("Re-Routes")]
        public Selectable upNavigation;
        public Selectable downNavigation;
        public Selectable leftNavigation;
        public Selectable rightNavigation;

#if UNITY_WEBGL
        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Selectable thisButton = GetComponent<Selectable>();
            Navigation navigation = thisButton.navigation;

            // For each of the directions checks if there's a button assigned.
            if (upNavigation != null)
            {
                navigation.selectOnUp = upNavigation;
            }
            if (downNavigation != null)
            {
                navigation.selectOnDown = downNavigation;
            }
            if (rightNavigation != null)
            {
                navigation.selectOnRight = rightNavigation;
            }
            if (leftNavigation != null)
            {
                navigation.selectOnLeft = leftNavigation;
            }

            thisButton.navigation = navigation;
        }
#endif
    }
}

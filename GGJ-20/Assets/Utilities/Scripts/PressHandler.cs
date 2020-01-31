using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Events;

namespace WhalesAndGames.Shipyard.Helpers
{
    /// <summary>
    /// Overrides the method through which WebGL links are opened.
    /// </summary>
    public class PressHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void OpenWindow(string url);
#endif

        [Serializable]
        public class ButtonPressEvent : UnityEvent { }

        // Stores the type of press event.
        public ButtonPressEvent OnPress = new ButtonPressEvent();

        /// <summary>
        /// Checks for pointer events (WebGL).
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_WEBGL
            OnPress.Invoke();
#endif
        }

        /// <summary>
        /// Checks for click events (Click).
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
#if !UNITY_WEBGL
            OnPress.Invoke();
#endif
        }

        /// <summary>
        /// Opens a given in URL link in different ways depending on the platform.
        /// </summary>
        public void OpenLink(string url)
        {
#if !UNITY_WEBGL
            Application.OpenURL(url);
#else
            OpenWindow(url);
#endif
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WhalesAndGames.Shipyard.Helpers
{
    /// <summary>
    /// Displays a cursor in-game, and allows the image to be swapped.
    /// </summary>
    public class GameCursor : MonoBehaviour
    {
        [Header("Canvas Render")]
        private Vector2 mousePixelPosition;

        // Internal References
        private Image cursorImage;
        private static GameCursor instance;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            // Gets the local references.
            cursorImage = GetComponentInChildren<Image>();
            mousePixelPosition = Input.mousePosition;

#if UNITY_ANDROID
            Destroy(transform.parent.gameObject);
#else
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(cursorImage.gameObject);
            }
            else
            {
                Destroy(cursorImage.gameObject);
            }
#endif
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }

            // Updates the cursor position.
            if (cursorImage != null)
                UpdateCursorPosition();
        }

        /// <summary>
        /// Updates the cursor's transform position to match that of the mouse.
        /// </summary>
        private void UpdateCursorPosition()
        {
            // Gets the mouse in pixel size.
            mousePixelPosition = Input.mousePosition;

            // Updates the UI to reflect the new Mouse Rect.
            transform.position = mousePixelPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation events allow animations to perform specific actions.
/// </summary>
public class AnimationEvents : MonoBehaviour {

    /// <summary>
    /// Destroys this Game Object in a given animation frame.
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Destroys the parent game object of this animator in a given animation frame.
    /// </summary>
    public void DestoryParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    /// <summary>
    /// Disables the Animator itself in a given animation frame. 
    /// This will stop, for example, Animated Canvas from setting themselves as dirty.
    /// </summary>
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

    /// <summary>
    /// Disables this Game Object in a given animation frame.
    /// </summary>
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Disables the parent Game Object in a given animation frame.
    /// </summary>
    public void DisableParentGameObject()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Disables the child Game Object of this animator in a given animation frame.
    /// </summary>
    public void DisableChildGameObject()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary>
    /// Shakes the Camera in a given animation frame.
    /// </summary>
    public void ShakeCamera()
    {
        if(Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().ShakeCamera(0.10f, 0.15f);
        }
    }

    /// <summary>
    /// Plays an Audio Clip through the Global Manager in a given animation frame.
    /// </summary>
    public void PlayAudioClip(AudioClip clip)
    {
        GlobalManager.Instance.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}

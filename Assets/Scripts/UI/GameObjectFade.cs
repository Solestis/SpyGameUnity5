using System.Collections;
using UnityEngine;

/// <summary>
/// Helper script to fade 3D objects in and out with the UI.
/// </summary>
public class GameObjectFade : MonoBehaviour {

    #region Linked in Editor
    public GameObject model; 
    #endregion

    #region Public Methods
    /// <summary>
    /// Calls the fade out animation for GameObjects
    /// </summary>
    public void GameObjectFadeOut()
    {
        StartCoroutine(FadeOutSmooth());
    }

    /// <summary>
    /// Calls the fade in animation for GameObjects
    /// </summary>
    public void GameObjectFadeIn()
    {
        StartCoroutine(FadeInSmooth());
    } 
    #endregion

    #region Coroutines
    /// <summary>
    /// Sets the alpha to 0 and increases the alpha of the material.color of a GameObject over 0.1 seconds.
    /// </summary>
    private IEnumerator FadeInSmooth()
    {
        Renderer r = model.GetComponent<Renderer>();
        float duration = 0.1f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            Color c = r.material.color;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            r.material.color = c;
            yield return null;
        }
    }

    /// <summary>
    /// Decreases the alpha of the material.color of a GameObject over 0.1 seconds.
    /// </summary>
    private IEnumerator FadeOutSmooth()
    {
        Renderer r = model.GetComponent<Renderer>();
        float duration = 0.1f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            Color c = r.material.color;
            c.a = Mathf.Lerp(1f, 0f, t / duration);
            r.material.color = c;
            yield return null;
        }
    } 
    #endregion

}

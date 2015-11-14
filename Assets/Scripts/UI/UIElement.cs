using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameObject class used on UI screens for use with UIGroup and UICanvas.
/// </summary>
public class UIElement : MonoBehaviour {

    public GameObject uiContent;
    public bool inDictionary = true;
    public Action onDeactivate;
    public Action onActivate;
    public bool isActive = false;

    protected float fadeDuration = 0.3f;

    #region Public Methods
    /// <summary>
    /// Shows (activates) the UIElement on the screen.
    /// </summary>
    public virtual void Activate()
    {
        uiContent.SetActive(true);
        isActive = true;
        if (onActivate != null)
        {
            onActivate();
        }
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Closes (hides) the UIElement.
    /// </summary>
    public virtual void Deactivate()
    {
        isActive = false;
        if (onDeactivate != null)
        {
            onDeactivate();
        }
        StartCoroutine(FadeOut());
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine which adds a fadeout animation to a UIElement object and then sets it inactive.
    /// </summary>
    protected IEnumerator FadeOut()
    {
        BroadcastMessage("GameObjectFadeOut", SendMessageOptions.DontRequireReceiver);
        float t = 0;
        CanvasGroup cg = uiContent.GetComponent<CanvasGroup>();
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        uiContent.SetActive(false);
    }

    /// <summary>
    /// Coroutine which adds a fadeout animation to a UIElement object.
    /// </summary>
    protected IEnumerator FadeIn()
    {
        BroadcastMessage("GameObjectFadeIn", SendMessageOptions.DontRequireReceiver);
        float t = 0;
        CanvasGroup cg = uiContent.GetComponent<CanvasGroup>();
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
    }
    #endregion
}


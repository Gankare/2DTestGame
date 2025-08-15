using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class AutoTypewriterTMP : MonoBehaviour
{
    [Header("Typing Settings")]
    public float charsPerSecond = 10f;
    public float punctuationPauseMultiplier = 4f;
    public bool useUnscaledTime = false;
    public KeyCode skipKey = KeyCode.Space;

    TMP_Text label;
    string lastText = "";
    Coroutine typingRoutine;

    void Awake()
    {
        label = GetComponent<TMP_Text>();
        lastText = label.text;
        StartTyping(lastText);
    }

    void Update()
    {
        if (label.text != lastText && label.text != null)
        {
            // Text has changed externally
            lastText = label.text;
            StartTyping(lastText);
        }

        if (typingRoutine != null && skipKey != KeyCode.None && Input.GetKeyDown(skipKey))
        {
            CompleteInstantly();
        }
    }

    void StartTyping(string fullText)
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeRoutine(fullText));
    }

    void CompleteInstantly()
    {
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }

        label.ForceMeshUpdate();
        label.maxVisibleCharacters = label.textInfo.characterCount;
    }

    IEnumerator TypeRoutine(string fullText)
    {
        label.ForceMeshUpdate();
        int totalChars = label.textInfo.characterCount;
        label.maxVisibleCharacters = 0;

        if (charsPerSecond <= 0f)
        {
            label.maxVisibleCharacters = totalChars;
            typingRoutine = null;
            yield break;
        }

        float baseDelay = 1f / Mathf.Max(1f, charsPerSecond);

        for (int visible = 0; visible <= totalChars; visible++)
        {
            label.maxVisibleCharacters = visible;

            if (visible < totalChars)
            {
                char c = label.textInfo.characterInfo[visible].character;
                float delay = baseDelay;
                if (IsPunctuation(c))
                    delay *= punctuationPauseMultiplier;

                float elapsed = 0f;
                while (elapsed < delay)
                {
                    if (skipKey != KeyCode.None && Input.GetKeyDown(skipKey))
                    {
                        CompleteInstantly();
                        yield break;
                    }

                    elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    yield return null;
                }
            }
        }

        typingRoutine = null;
    }

    static bool IsPunctuation(char c)
    {
        return c == '.' || c == ',' || c == '!' || c == '?' || c == ':' || c == ';';
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] GameObject FadePanelPrefab;
    [SerializeField] float fade_in_time;

    Image FadePanel;

    void Awake()
    {
        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        FadePanel = Instantiate(
            FadePanelPrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            canvas
        ).GetComponent<Image>();
        FadePanel.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void FadeIn(float fade_time)
    {
        StartCoroutine(FadeRoutine(1f, 0f, fade_time));
    }

    public void FadeOut(float fade_time)
    {
        FadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0f, 1f, fade_time));
    }

    private IEnumerator FadeRoutine(float start, float end, float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            FadePanel.color = new Color(
                FadePanel.color.r, 
                FadePanel.color.g,
                FadePanel.color.b,
                Mathf.Lerp(start, end, t / time)
            );
            yield return null;
        }
        FadePanel.color = new Color(
            FadePanel.color.r, 
            FadePanel.color.g,
            FadePanel.color.b,
            end
        );
        if (end <= 0.01f) FadePanel.gameObject.SetActive(false);
    }

    [ContextMenu("Fade In")]  void FadeInTest()  { FadeIn(1f); }
    [ContextMenu("Fade Out")] void FadeOutTest() { FadeOut(1f); }
}

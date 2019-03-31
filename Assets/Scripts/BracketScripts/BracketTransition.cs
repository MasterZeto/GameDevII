using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BracketTransition : MonoBehaviour
{
    [System.Serializable]
    private struct BracketUI
    {
        public GameObject name1;
        public GameObject name2;
        public GameObject button;
    }

    [SerializeField] RectTransform bracket;
    [SerializeField] float transition_time;
    [SerializeField] float time_between_reveal;
    [SerializeField] Ease easing_function;

    [SerializeField] BracketUI SawyerUI;
    [SerializeField] Vector2 SawyerBracketPosition;

    [SerializeField] BracketUI SaraUI;
    [SerializeField] Vector2 SaraBracketPosition;

    [SerializeField] BracketUI BajaUI;
    [SerializeField] Vector2 BajaBracketPosition;

    [ContextMenu("Trigger Sawyer Animation")]
    public void TriggerSawyerAnimation(float delay)
    {
        RevealElement(SawyerUI.name1,  delay + 1f * time_between_reveal);
        RevealElement(SawyerUI.name2,  delay + 2f * time_between_reveal);
        RevealElement(SawyerUI.button, delay + 3f * time_between_reveal);
    }

    [ContextMenu("Trigger Sara Animation")]
    public void TriggerSaraAnimation(float delay)
    {
        MoveTo(SawyerBracketPosition, SaraBracketPosition, delay);
        RevealElement(SaraUI.name1,  delay + transition_time + 1f * time_between_reveal);
        RevealElement(SaraUI.name2,  delay + transition_time + 2f * time_between_reveal);
        RevealElement(SaraUI.button, delay + transition_time + 3f * time_between_reveal);
    }

    [ContextMenu("Trigger Baja Animation")]
    public void TriggerBajaAnimation(float delay)
    {
        MoveTo(SaraBracketPosition, BajaBracketPosition, delay);
        RevealElement(BajaUI.name1,  delay + transition_time + 1f * time_between_reveal);
        RevealElement(BajaUI.name2,  delay + transition_time + 2f * time_between_reveal);
        RevealElement(BajaUI.button, delay + transition_time + 3f * time_between_reveal);
    }

    private void MoveTo(Vector2 from, Vector2 to, float delay)
    {
        StartCoroutine(MoveUIElementRoutine(from, to, delay));
    }

    private IEnumerator MoveUIElementRoutine(Vector2 start, Vector2 dest, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (float t = 0; t < transition_time; t += Time.deltaTime)
        {
            bracket.anchoredPosition = Tween.EaseVector2(
                start, 
                dest, 
                t / transition_time, 
                easing_function
            );
            yield return null;
        }
        bracket.anchoredPosition = dest;
    }

    private void RevealElement(GameObject element, float delay)
    {
        StartCoroutine(RevealAfterDelay(element, delay));
    }

    private IEnumerator RevealAfterDelay(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        obj.SetActive(true);
    }

}

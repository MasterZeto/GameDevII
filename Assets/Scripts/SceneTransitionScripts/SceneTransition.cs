using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string target_scene;
    [SerializeField] float transition_time;
    [SerializeField] SceneFade fade;

    [ContextMenu("Transition Scene")]
    public void Transition()
    {
        fade.FadeOut(transition_time);
        StartCoroutine(LoadAfterDelay(transition_time + 0.1f));
    }

    private IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(target_scene);
    }
}

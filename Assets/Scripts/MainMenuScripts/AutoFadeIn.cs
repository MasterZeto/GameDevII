using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidFunction();

public class AutoFadeIn : MonoBehaviour
{
    [SerializeField] SceneFade fade;

    void Start()
    {
        StartCoroutine(ExecAfterDelay(fade.FadeIn, 0.01f));
    }

    private IEnumerator ExecAfterDelay(VoidFunction func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }
}

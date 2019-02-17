using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitCallback(float damage);

public class Hurtbox : MonoBehaviour
{
    [SerializeField] Collider _collider;

    float hitstop_multiplier = 0.1f;
    HitCallback callback;

    public void Initialize(HitCallback callback)
    {
        this.callback = callback;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("HIT");
        StartCoroutine(Hitstop(damage));  
    }

    private IEnumerator Hitstop(float damage)
    {
        Time.timeScale = 0f;
        for (float t = 0; t < hitstop_multiplier * damage; t += Time.unscaledDeltaTime) 
        {
            yield return null;
        }
        Time.timeScale = 1f;
        //callback(damage);
    }
}

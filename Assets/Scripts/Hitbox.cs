using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// make sure this is on the same layer as only other hurt boxes.
// so the players hitboxes are gonna be on the enemy hurtbox layer
// and the enemy hitboxes are gonna be on the player hurtbox layer
public class Hitbox : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] Collider _collider;

    public bool active { get; private set; }

    void Start() { active = false; }

    void OnTriggerEnter(Collider c)
    {
        if (active)
        {
            Debug.Log("entered and active");
            Hurtbox h = c.gameObject.GetComponent<Hurtbox>();
            if (h != null)
            {
                Debug.Log("oof");
                active = false;
                h.TakeDamage(_damage);
            }
        }
    }

    public void Fire(float duration)
    {
        active = true;
        StartCoroutine(FireRoutine(duration));
    }

    IEnumerator FireRoutine(float duration)
    {
        float t = 0f;
        while (t < duration && active)
        {
            t += Time.deltaTime;
            yield return null;
        }
        active = false;
    }
}

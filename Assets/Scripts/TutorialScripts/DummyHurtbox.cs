using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DummyHitCallback(string box);

public class DummyHurtbox : MonoBehaviour
{
    [SerializeField] float notify_cooldown;

    DummyHitCallback callback;
    float timer;

    public void Initialize(DummyHitCallback callback)
    {
        this.callback = callback;
        timer = 0f;
    }

    void OnTriggerStay(Collider c)
    {
        if (timer <= 0f)
        {
            Hitbox h = c.gameObject.GetComponent<Hitbox>();

            if (h != null && h.active)
            {
                callback(c.name);
                timer = notify_cooldown;
            }
        }
    }

    void Update()
    {
        if (timer > 0f) timer -= Time.unscaledDeltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitCallback(float damage);

public class Hurtbox : MonoBehaviour
{
    [SerializeField] Collider _collider;

    HitCallback callback;

    public void Initialize(HitCallback callback)
    {
        this.callback = callback;
    }

    public void TakeDamage(float damage)
    {
        callback(damage);    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float maxHitPoints;
    [SerializeField] Image uiHealthBar;

    float hitPoints;

    private void Start()
    {
        hitPoints = maxHitPoints;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = hitPoints / maxHitPoints;
        uiHealthBar.rectTransform.localScale = new Vector3(ratio,1,1);
    }

    void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if(hitPoints<0)
        {
            hitPoints = 0;
            Debug.Log("Die");
        }
        UpdateHealthBar();
    }

    void HealDamage(float heal)
    {
        hitPoints += heal;
        if (hitPoints >maxHitPoints)
        {
            hitPoints = maxHitPoints;

        }
        UpdateHealthBar();
    }



}

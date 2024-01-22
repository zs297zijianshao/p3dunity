using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 100f;
    public void objectHitDamage(float amount)
    {
        objectHealth = objectHealth-amount;
        Debug.Log("HP"+objectHealth);
        if(objectHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;

    [SerializeField] protected float maxHealth;

    protected float health;


    /// <summary>
    /// protected 表示子类可以访问这个函数
    /// virtual表示子类可以重写这个函数
    /// </summary>
    protected virtual private void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        Debug.Log(health); 
    }

    public virtual void Die()
    {
        health = 0f;
        PoolManager.Release(deathVFX, transform.position);
        //gameObject.SetActive(false);
    }

    /// <summary>
    /// 恢复生命值
    /// </summary>
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;

        /*health += value;
        health = Mathf.Clamp(health, 0f, maxHealth);*/
        health = Mathf.Clamp(health + value, 0f, maxHealth);
    }

    /// <summary>
    /// HOT （heal over time
    /// 生命值再生的持续回血
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    /// <summary>
    /// DOT （demage over time 持续受伤
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerable DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0)
        {
            yield return waitTime;

            TakeDamage(maxHealth * percent);
        }
    }
}

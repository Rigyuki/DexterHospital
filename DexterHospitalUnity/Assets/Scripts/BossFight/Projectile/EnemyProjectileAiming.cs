using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAiming : Projectile
{
    //[SerializeField] float damage;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirctionCoroutine));

        base.OnEnable();
    }

    IEnumerator MoveDirctionCoroutine()
    {
        yield return null;//�ȴ�һ֡��ʱ��

        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}

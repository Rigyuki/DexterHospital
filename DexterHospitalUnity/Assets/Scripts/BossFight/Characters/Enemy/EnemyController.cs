using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("-----FIRE-------")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;//��ʼ������λ�ã�ǹ�ڣ�

    [SerializeField] float minFireInterval = 0f;
    [SerializeField] float maxFireInterval = 1f;

    private void OnEnable()
    {
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator RandomlyFireCoroutine()
    {
        // WaitForSeconds waitForRandomFireInterval = new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

        while (gameObject.activeSelf)
        {

            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            //yield return new WaitForSeconds(0.5f);//����ʱ��


            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}

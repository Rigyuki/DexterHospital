using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("-----FIRE-------")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;//开始攻击的位置（枪口）

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
            //yield return new WaitForSeconds(0.5f);//挂起时间


            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}

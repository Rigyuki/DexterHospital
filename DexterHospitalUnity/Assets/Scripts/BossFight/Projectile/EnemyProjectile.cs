using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        //当子弹不是直直往下飞时
        if (moveDirection != Vector2.down)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.down, moveDirection);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        //���ӵ�����ֱֱ���·�ʱ
        if (moveDirection != Vector2.down)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.down, moveDirection);
        }
    }
}

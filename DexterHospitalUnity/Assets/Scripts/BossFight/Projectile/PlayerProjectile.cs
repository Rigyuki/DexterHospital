using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
   
    protected virtual void Awake()
    {
         

        if (moveDirection != Vector2.up)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.up, moveDirection);
        }
    }

    /*protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }*/
}

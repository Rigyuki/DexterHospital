using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// get a list of detectors inside our enemy ai script
public class ObstacleDetector : Detector
{
    [SerializeField]
    private float detectionRadius = 2; //obstacles in the distance of 2 spaces from the enemy

    [SerializeField]
    private LayerMask layerMask; //layer mask representing the obstacles

    [SerializeField]
    private bool showGizmos = true;

    Collider2D[] colliders;

    public override void Detect(AIData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}

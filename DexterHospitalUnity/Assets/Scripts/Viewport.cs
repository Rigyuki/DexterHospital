using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinleton<T> : MonoBehaviour where T : Component 
{ 
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this as T;
    }
}

public class Viewport : Sinleton<Viewport>
{
    float minX, minY;
    float maxX, maxY;

    private void Start()
    {
        Camera mainCarame = Camera.main;

        Vector2 bottomLeft = mainCarame.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCarame.ViewportToWorldPoint(new Vector3(1f, 1f));

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;

    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }
}

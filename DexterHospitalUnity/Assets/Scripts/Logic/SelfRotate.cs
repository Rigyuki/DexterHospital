using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{

    public static SelfRotate Instance { get; private set; }
    public int RotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        RotateSpeed = 50;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime, Space.Self);
    }
}
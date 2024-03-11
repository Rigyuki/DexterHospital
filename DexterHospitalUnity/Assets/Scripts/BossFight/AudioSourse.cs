using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourse : MonoBehaviour
{
    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bgm.Play();
    }
}

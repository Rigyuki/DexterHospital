using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoMgr : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public string videoName ;

     
    // Start is called before the first frame update
    private void Awake()
    {
        videoName = "2video.mp4";
    }
    void Start()
    {
        //Handheld.PlayFullScreenMovie("StarWars.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        videoPlayer.url = Application.dataPath + "/Resources/Video/" + videoName;
        Debug.Log(Application.dataPath + "/Resources/Video/" + videoName);
    }

    // Update is called once per frame
    void Update()
    {
         
        if (videoPlayer.isPrepared)
        {
            if (videoPlayer.frame >= (long)videoPlayer.frameCount-1)
            {
                SceneManager.LoadScene(0);
            }
        }
       
    }

 
}

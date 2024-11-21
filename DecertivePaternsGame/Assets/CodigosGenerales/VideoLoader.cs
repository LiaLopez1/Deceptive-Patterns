using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Arrastra aquí el VideoPlayer desde el Inspector
    public string videoFileName = "VideoOutro.mp4"; // Cambia por el nombre de tu video

    void Start()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        // Configura el VideoPlayer
        videoPlayer.url = videoPath;
        videoPlayer.Play(); // Reproduce el video automáticamente al iniciar
    }
}

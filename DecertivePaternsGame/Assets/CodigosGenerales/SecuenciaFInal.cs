using UnityEngine;
using UnityEngine.Video;

public class FinalSequence : MonoBehaviour
{
    public GameObject videoCanvas; // Canvas que contiene el VideoPlayer
    public VideoPlayer videoPlayer; // Componente VideoPlayer
    public GameObject finalCanvas;  // Canvas final que aparecer� luego del video
    public MonoBehaviour pauseMenuScript; // Script que controla el men� de pausa, en un objeto vac�o

    void Start()
    {
        videoCanvas.SetActive(false); // Asegurarse que el canvas del video est� desactivado al inicio
        finalCanvas.SetActive(false); // El canvas final tambi�n debe estar desactivado

        // A�adir el evento para ejecutar cuando el video finalice
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayPrologueVideo()
    {
        videoCanvas.SetActive(true); // Activar el canvas del video
        videoPlayer.Play(); // Reproducir el video

        // Bloquear el cursor durante la cinem�tica
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desactivar el script del men� de pausa durante la cinem�tica
        if (pauseMenuScript != null)
        {
            pauseMenuScript.enabled = false;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoCanvas.SetActive(false); // Desactivar el canvas del video cuando termine
        finalCanvas.SetActive(true); // Activar el canvas final

        // Desbloquear el cursor cuando el video termine
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // No reactivar el script del men� de pausa, mantenerlo desactivado hasta que termine la interacci�n con el canvas final
    }

    public void OnFinalCanvasClose()
    {
        // Este m�todo se debe llamar cuando el jugador termina la interacci�n con el canvas final
        finalCanvas.SetActive(false); // Desactivar el canvas final

        // Reactivar el script del men� de pausa al finalizar la interacci�n con el canvas final
        if (pauseMenuScript != null)
        {
            pauseMenuScript.enabled = true;
        }
    }
}

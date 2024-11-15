using UnityEngine;
using UnityEngine.Video;

public class FinalSequence : MonoBehaviour
{
    public GameObject videoCanvas; // Canvas que contiene el VideoPlayer
    public VideoPlayer videoPlayer; // Componente VideoPlayer
    public GameObject finalCanvas;  // Canvas final que aparecerá luego del video
    public MonoBehaviour pauseMenuScript; // Script que controla el menú de pausa, en un objeto vacío

    void Start()
    {
        videoCanvas.SetActive(false); // Asegurarse que el canvas del video esté desactivado al inicio
        finalCanvas.SetActive(false); // El canvas final también debe estar desactivado

        // Añadir el evento para ejecutar cuando el video finalice
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayPrologueVideo()
    {
        videoCanvas.SetActive(true); // Activar el canvas del video
        videoPlayer.Play(); // Reproducir el video

        // Bloquear el cursor durante la cinemática
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desactivar el script del menú de pausa durante la cinemática
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

        // No reactivar el script del menú de pausa, mantenerlo desactivado hasta que termine la interacción con el canvas final
    }

    public void OnFinalCanvasClose()
    {
        // Este método se debe llamar cuando el jugador termina la interacción con el canvas final
        finalCanvas.SetActive(false); // Desactivar el canvas final

        // Reactivar el script del menú de pausa al finalizar la interacción con el canvas final
        if (pauseMenuScript != null)
        {
            pauseMenuScript.enabled = true;
        }
    }
}

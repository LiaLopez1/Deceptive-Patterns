using UnityEngine;
using UnityEngine.Video;

public class DoorInteraction : MonoBehaviour
{
    public ContadorLlaves contadorLlaves; // Referencia al script ContadorLlaves
    public FinalSequence finalSequence;   // Referencia al script FinalSequence
    public GameObject interactionCanvas;  // Canvas para mostrar el texto de interacción
    public GameObject player;             // Referencia al jugador (capsula)
    public GameObject backgroundMusic;    // Objeto que controla la música de fondo
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador (como MonoBehaviour)

    private bool hasActivatedCinematic = false; // Booleano para controlar si la cinemática ya se ha activado

    void Start()
    {
        interactionCanvas.SetActive(false); // Desactivar el canvas de interacción al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && contadorLlaves.llavesActuales >= 4)
        {
            interactionCanvas.SetActive(true); // Activar el canvas de interacción cuando el jugador esté cerca
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && contadorLlaves.llavesActuales >= 4 && !hasActivatedCinematic)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Activar la cinemática solo una vez
                hasActivatedCinematic = true;

                // Cuando el jugador tiene 4 llaves y presiona "E" al interactuar con la puerta
                finalSequence.PlayPrologueVideo(); // Llama al método para reproducir el video de prólogo
                gameObject.SetActive(false); // Desactiva la puerta si es necesario
                interactionCanvas.SetActive(false); // Ocultar el canvas de interacción

                // Detener la música de fondo
                if (backgroundMusic != null)
                {
                    backgroundMusic.SetActive(false);
                }

                // Desactivar el movimiento del jugador
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionCanvas.SetActive(false); // Ocultar el canvas de interacción cuando el jugador se aleje
        }
    }
}

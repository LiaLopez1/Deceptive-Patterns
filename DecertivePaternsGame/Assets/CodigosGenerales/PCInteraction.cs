using UnityEngine;
using UnityEngine.UI;

public class PCInteraction : MonoBehaviour
{
    public Camera playerCamera;     // Cámara del jugador
    public Camera computerCamera;   // Cámara del PC
    public GameObject interactionText;  // Texto "Presiona E para interactuar"
    public GameObject panelToDisable;  // El otro panel que deseas desactivar al cambiar de cámara
    public GameObject computerCanvas;  // Canvas de la pantalla del ordenador

    private bool isPlayerInTrigger = false;  // Verificar si el jugador está en el trigger

    void Start()
    {
        playerCamera.enabled = true;
        computerCamera.enabled = false;
        interactionText.SetActive(false);  // Ocultar el mensaje al inicio
        panelToDisable.SetActive(true);  // Asegurarse de que el otro panel esté activo al inicio
        computerCanvas.SetActive(false);  // Asegurar que el Canvas del ordenador esté desactivado al inicio
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))  // Solo si el jugador está en el trigger
        {
            if (playerCamera.enabled)
            {
                // Cambiar a la cámara del PC
                playerCamera.enabled = false;
                computerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.None;  // Desbloquear el cursor
                Cursor.visible = true;  // Mostrar el cursor

                // Desactivar el otro panel y activar el Canvas del PC
                panelToDisable.SetActive(false);  // Deshabilitar el panel que mencionas
                computerCanvas.SetActive(true);   // Habilitar la UI del ordenador
            }
            else
            {
                // Volver a la cámara del jugador
                computerCamera.enabled = false;
                playerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor
                Cursor.visible = false;  // Ocultar el cursor

                // Volver a habilitar el otro panel y desactivar el Canvas del PC
                panelToDisable.SetActive(true);  // Volver a habilitar el panel
                computerCanvas.SetActive(false);  // Desactivar el Canvas del ordenador
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Verificar que sea el jugador quien entra
        {
            isPlayerInTrigger = true;
            interactionText.SetActive(true);  // Mostrar el mensaje "Presiona E para interactuar"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactionText.SetActive(false);  // Ocultar el mensaje al salir del trigger
        }
    }
}

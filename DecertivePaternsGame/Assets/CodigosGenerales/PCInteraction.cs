using UnityEngine;
using UnityEngine.UI;

public class PCInteraction : MonoBehaviour
{
    public Camera playerCamera;     // Cámara del jugador
    public Camera computerCamera;   // Cámara del PC
    public GameObject interactionText;  // Texto "Presiona E para interactuar"
    public GameObject panelToDisable;  // El otro panel que deseas desactivar al cambiar de cámara
    public GameObject computerCanvas;  // Canvas de la pantalla del ordenador
    public MonoBehaviour playerMovementScript; // Referencia al script de movimiento del jugador

    private bool isPlayerInTrigger = false;  // Verificar si el jugador está en el trigger
    private RectTransform canvasRectTransform;  // Para manejar los límites del Canvas

    void Start()
    {
        playerCamera.enabled = true;
        computerCamera.enabled = false;
        interactionText.SetActive(false);  // Ocultar el mensaje al inicio
        panelToDisable.SetActive(true);  // Asegurarse de que el otro panel esté activo al inicio
        computerCanvas.SetActive(false);  // Asegurar que el Canvas del ordenador esté desactivado al inicio

        // Obtener el RectTransform del Canvas para definir los límites
        canvasRectTransform = computerCanvas.GetComponent<RectTransform>();
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

                // Asignar la cámara del PC como la cámara del Canvas
                computerCanvas.GetComponent<Canvas>().worldCamera = computerCamera;

                // Ocultar el mensaje de interacción
                interactionText.SetActive(false);

                // Desactivar el script de movimiento del jugador
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }
            }
            else
            {
                // Volver a la cámara del jugador
                computerCamera.enabled = false;
                playerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor en el centro de la pantalla
                Cursor.visible = false;  // Ocultar el cursor

                // Volver a habilitar el otro panel y desactivar el Canvas del PC
                panelToDisable.SetActive(true);  // Volver a habilitar el panel
                computerCanvas.SetActive(false);  // Desactivar el Canvas del ordenador

                // Mostrar el mensaje de interacción nuevamente (opcional)
                interactionText.SetActive(true);

                // Activar el script de movimiento del jugador
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = true;
                }
            }
        }

        // Mostrar u ocultar el cursor dependiendo de si está dentro del área del Canvas
        if (computerCamera.enabled)
        {
            UpdateCursorVisibility();
        }
    }

    void UpdateCursorVisibility()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // Convertir la posición del mouse a un punto local dentro del RectTransform del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, computerCamera, out localPoint);

        // Verificar si el cursor está dentro del área del Canvas
        if (canvasRectTransform.rect.Contains(localPoint))
        {
            Cursor.visible = true;  // Mostrar el cursor si está dentro del Canvas
        }
        else
        {
            Cursor.visible = false;  // Ocultar el cursor si está fuera del Canvas
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

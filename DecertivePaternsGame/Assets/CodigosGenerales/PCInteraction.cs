using UnityEngine;
using UnityEngine.UI;

public class PCInteraction : MonoBehaviour
{
    public Camera playerCamera;     // C�mara del jugador
    public Camera computerCamera;   // C�mara del PC
    public GameObject interactionText;  // Texto "Presiona E para interactuar"
    public GameObject panelToDisable;  // El otro panel que deseas desactivar al cambiar de c�mara
    public GameObject computerCanvas;  // Canvas de la pantalla del ordenador
    public MonoBehaviour playerMovementScript; // Referencia al script de movimiento del jugador

    private bool isPlayerInTrigger = false;  // Verificar si el jugador est� en el trigger
    private RectTransform canvasRectTransform;  // Para manejar los l�mites del Canvas

    void Start()
    {
        playerCamera.enabled = true;
        computerCamera.enabled = false;
        interactionText.SetActive(false);  // Ocultar el mensaje al inicio
        panelToDisable.SetActive(true);  // Asegurarse de que el otro panel est� activo al inicio
        computerCanvas.SetActive(false);  // Asegurar que el Canvas del ordenador est� desactivado al inicio

        // Obtener el RectTransform del Canvas para definir los l�mites
        canvasRectTransform = computerCanvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))  // Solo si el jugador est� en el trigger
        {
            if (playerCamera.enabled)
            {
                // Cambiar a la c�mara del PC
                playerCamera.enabled = false;
                computerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.None;  // Desbloquear el cursor
                Cursor.visible = true;  // Mostrar el cursor

                // Desactivar el otro panel y activar el Canvas del PC
                panelToDisable.SetActive(false);  // Deshabilitar el panel que mencionas
                computerCanvas.SetActive(true);   // Habilitar la UI del ordenador

                // Asignar la c�mara del PC como la c�mara del Canvas
                computerCanvas.GetComponent<Canvas>().worldCamera = computerCamera;

                // Ocultar el mensaje de interacci�n
                interactionText.SetActive(false);

                // Desactivar el script de movimiento del jugador
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }
            }
            else
            {
                // Volver a la c�mara del jugador
                computerCamera.enabled = false;
                playerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor en el centro de la pantalla
                Cursor.visible = false;  // Ocultar el cursor

                // Volver a habilitar el otro panel y desactivar el Canvas del PC
                panelToDisable.SetActive(true);  // Volver a habilitar el panel
                computerCanvas.SetActive(false);  // Desactivar el Canvas del ordenador

                // Mostrar el mensaje de interacci�n nuevamente (opcional)
                interactionText.SetActive(true);

                // Activar el script de movimiento del jugador
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = true;
                }
            }
        }

        // Mostrar u ocultar el cursor dependiendo de si est� dentro del �rea del Canvas
        if (computerCamera.enabled)
        {
            UpdateCursorVisibility();
        }
    }

    void UpdateCursorVisibility()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // Convertir la posici�n del mouse a un punto local dentro del RectTransform del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, computerCamera, out localPoint);

        // Verificar si el cursor est� dentro del �rea del Canvas
        if (canvasRectTransform.rect.Contains(localPoint))
        {
            Cursor.visible = true;  // Mostrar el cursor si est� dentro del Canvas
        }
        else
        {
            Cursor.visible = false;  // Ocultar el cursor si est� fuera del Canvas
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

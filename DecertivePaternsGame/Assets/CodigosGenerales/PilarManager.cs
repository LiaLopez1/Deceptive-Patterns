using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerInteraction : MonoBehaviour
{
    public Canvas interactionHintCanvas;   // Canvas que muestra "Presiona E para interactuar"
    public Canvas descriptionCanvas;       // Canvas que muestra la descripción con botón para salir
    [TextArea]
    public string description;             // Descripción específica para el pilar
    public TextMeshProUGUI descriptionText; // TextMeshPro que muestra la descripción
    public Button closeButton;             // Botón para cerrar la descripción
    public Button takeKeyButton;           // Botón para tomar la llave
    public GameObject keyModel;            // Modelo de la llave visible sobre el pilar (individual, debe ser único)
    public GameObject player;              // Referencia al jugador
    public int keyID;                      // ID de la llave específica

    private MonoBehaviour playerMovementScript; // Referencia al script de movimiento del jugador
    private bool playerInRange = false;          // Para verificar si el jugador está dentro del rango de interacción
    private bool keyTaken = false;               // Para verificar si la llave ya fue tomada

    private void Start()
    {
        // Obtener el script de movimiento del jugador
        playerMovementScript = player.GetComponent<MonoBehaviour>();

        // Desactivar los canvas al inicio
        interactionHintCanvas.gameObject.SetActive(false);
        descriptionCanvas.gameObject.SetActive(false);

        // Configurar los botones
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDescription);
        }

        if (takeKeyButton != null)
        {
            takeKeyButton.onClick.AddListener(TakeKey);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(false);
            descriptionCanvas.gameObject.SetActive(false);
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowDescription();
        }
    }

    private void ShowDescription()
    {
        // Ocultar el mensaje de interacción
        interactionHintCanvas.gameObject.SetActive(false);

        // Mostrar el canvas de descripción y actualizar el texto
        descriptionCanvas.gameObject.SetActive(true);
        descriptionText.text = description;

        // Desactivar el script de movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // Mostrar el cursor del mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseDescription()
    {
        // Ocultar la descripción
        descriptionCanvas.gameObject.SetActive(false);

        // Reactivar el script de movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Ocultar el cursor del mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Volver a mostrar el mensaje de interacción si el jugador sigue en el área
        if (playerInRange)
        {
            interactionHintCanvas.gameObject.SetActive(true);
        }
    }

    private void TakeKey()
    {
        // Verificar si la llave ya ha sido tomada
        if (keyTaken)
        {
            Debug.Log($"La llave con ID {keyID} ya fue tomada.");
            return;
        }

        Debug.Log($"Intentando tomar la llave con ID: {keyID}");

        // Obtener el inventario del jugador
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
        if (playerInventory == null) return;

        // Si ya tiene una llave, devolver la llave anterior y desactivar el estado de tener una llave
        if (playerInventory.HasKey())
        {
            GameObject previousKeyModel = playerInventory.GetKeyModel();
            if (previousKeyModel != null)
            {
                previousKeyModel.SetActive(true);
            }
        }

        // Desactivar el modelo de la llave actual y actualizar el inventario del jugador
        if (keyModel != null)
        {
            keyModel.SetActive(false);
            playerInventory.SetKey(keyID, keyModel);
            keyTaken = true;  // Marcar la llave como tomada
        }

        // Desactivar el botón de tomar llave para evitar que se tome más de una vez
        takeKeyButton.interactable = false;
    }
}

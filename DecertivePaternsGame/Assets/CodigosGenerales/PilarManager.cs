using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerInteraction : MonoBehaviour
{
    public Canvas interactionHintCanvas;   // Canvas que muestra "Presiona E para interactuar"
    public Canvas descriptionCanvas;       // Canvas que muestra la descripci�n con bot�n para salir
    [TextArea]
    public string description;             // Descripci�n espec�fica para el pilar
    public TextMeshProUGUI descriptionText; // TextMeshPro que muestra la descripci�n
    public Button closeButton;             // Bot�n para cerrar la descripci�n
    public Button takeKeyButton;           // Bot�n para tomar la llave
    public GameObject keyModel;            // Modelo de la llave visible sobre el pilar (individual, debe ser �nico)
    public GameObject player;              // Referencia al jugador
    public int keyID;                      // ID de la llave espec�fica

    private MonoBehaviour playerMovementScript; // Referencia al script de movimiento del jugador
    private bool playerInRange = false;          // Para verificar si el jugador est� dentro del rango de interacci�n
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
        // Ocultar el mensaje de interacci�n
        interactionHintCanvas.gameObject.SetActive(false);

        // Mostrar el canvas de descripci�n y actualizar el texto
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
        // Ocultar la descripci�n
        descriptionCanvas.gameObject.SetActive(false);

        // Reactivar el script de movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Ocultar el cursor del mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Volver a mostrar el mensaje de interacci�n si el jugador sigue en el �rea
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

        // Desactivar el bot�n de tomar llave para evitar que se tome m�s de una vez
        takeKeyButton.interactable = false;
    }
}

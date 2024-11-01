using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Importar la librer�a de TextMeshPro
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour
{
    [System.Serializable]
    public class TriggerData
    {
        public Collider interactionTrigger;    // Trigger para la interacci�n
        public Canvas interactionHintCanvas;   // Canvas que muestra "Presiona E para interactuar"
        [TextArea]
        public string description;             // Descripci�n espec�fica para el pilar
    }

    public List<TriggerData> triggers = new List<TriggerData>();

    // Referencias al Canvas principal en Screen Space
    public Canvas descriptionCanvas;          // Canvas que muestra la descripci�n completa
    public TextMeshProUGUI descriptionText;   // TextMeshPro que muestra la descripci�n
    public Button closeButton;                // Bot�n para cerrar la descripci�n

    public GameObject player;                 // Referencia al jugador

    private TriggerData currentTrigger;       // Trigger actual con el que se est� interactuando
    private MonoBehaviour[] playerMovementScripts; // Referencia a todos los scripts del jugador para control de movimiento

    private void Start()
    {
        // Obtener todos los scripts de movimiento del jugador
        playerMovementScripts = player.GetComponents<MonoBehaviour>();

        // Desactivar todos los hints y el canvas de descripci�n al inicio
        foreach (var trigger in triggers)
        {
            trigger.interactionHintCanvas.gameObject.SetActive(false);
        }
        descriptionCanvas.gameObject.SetActive(false);

        // Configurar el bot�n de cierre
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDescription);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var trigger in triggers)
            {
                if (other == trigger.interactionTrigger)
                {
                    trigger.interactionHintCanvas.gameObject.SetActive(true);
                    currentTrigger = trigger;  // Guardar el trigger actual
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var trigger in triggers)
            {
                if (other == trigger.interactionTrigger)
                {
                    trigger.interactionHintCanvas.gameObject.SetActive(false);
                    currentTrigger = null;  // Limpiar el trigger actual
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (currentTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            ShowDescription(currentTrigger);
        }
    }

    private void ShowDescription(TriggerData trigger)
    {
        // Ocultar el mensaje de interacci�n
        trigger.interactionHintCanvas.gameObject.SetActive(false);

        // Mostrar el canvas de descripci�n y actualizar el texto
        descriptionCanvas.gameObject.SetActive(true);
        descriptionText.text = trigger.description;

        // Desactivar los scripts de movimiento del jugador
        foreach (var script in playerMovementScripts)
        {
            script.enabled = false;
        }
    }

    private void CloseDescription()
    {
        // Ocultar la descripci�n
        descriptionCanvas.gameObject.SetActive(false);

        // Reactivar los scripts de movimiento del jugador
        foreach (var script in playerMovementScripts)
        {
            script.enabled = true;
        }

        // Volver a mostrar el mensaje de interacci�n, si todav�a est� en el �rea del trigger
        if (currentTrigger != null)
        {
            currentTrigger.interactionHintCanvas.gameObject.SetActive(true);
        }
    }
}

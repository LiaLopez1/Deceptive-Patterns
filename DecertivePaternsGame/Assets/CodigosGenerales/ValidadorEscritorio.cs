using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;              // Referencia al Animator del cofre
    public Canvas interactionHintCanvas;        // Canvas que muestra "Presiona E para interactuar"
    public GameObject keyObject;                // La llave dentro del cofre
    public GameObject firstDialogPanel;         // Panel del diálogo para el primer intento fallido y mensaje de éxito
    public Text firstDialogText;                // Texto para el primer intento fallido y mensaje de éxito
    public GameObject multiAttemptDialogPanel;  // Panel del diálogo para intentos múltiples (5º, 8º, etc.)
    public Text multiAttemptDialogText;         // Texto para los intentos múltiples

    // Variables para los mensajes de diálogo, configurables en el Inspector
    [TextArea] public string firstAttemptMessage = "La llave es incorrecta. Prueba con otra llave.";
    [TextArea] public string successMessage = "¡Cofre abierto exitosamente!";
    [TextArea] public string multiAttemptMessage = "Aún no tienes la llave correcta. Sigue intentando.";

    public float dialogDisplayDuration = 2f;    // Duración del diálogo en pantalla, ajustable desde el Inspector
    public float openAnimationDuration = 2f;    // Duración de la animación de apertura del cofre
    private bool playerInRange = false;         // Verifica si el jugador está en rango
    private int failedAttempts = 0;             // Contador de intentos fallidos
    private Collider triggerCollider;           // Referencia al Collider del propio Trigger

    private void Start()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        keyObject.SetActive(false); // Desactiva la llave al inicio del juego
        firstDialogPanel.SetActive(false); // Desactiva el primer panel de diálogo al inicio
        multiAttemptDialogPanel.SetActive(false); // Desactiva el panel de intentos múltiples al inicio

        // Obtiene el Collider del propio objeto en el que está el script
        triggerCollider = GetComponent<Collider>();
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
            interactionHintCanvas.gameObject.SetActive(false); // Oculta el mensaje al salir del trigger
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactionHintCanvas.gameObject.SetActive(false); // Oculta el mensaje al presionar E
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        if (PilarKeyInteraction.hasCorrectKey) // El jugador tiene la llave correcta
        {
            Debug.Log("Cofre abierto correctamente.");
            chestAnimator.SetTrigger("OpenCajon"); // Activa la animación de apertura del cofre
            triggerCollider.enabled = false; // Desactiva el propio trigger
            firstDialogText.text = successMessage; // Muestra el mensaje de éxito
            firstDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(firstDialogPanel)); // Oculta el panel después de un tiempo
            StartCoroutine(ActivateKeyAfterAnimation()); // Activa la llave después de la animación
        }
        else
        {
            HandleIncorrectKeyAttempt();
        }
    }

    private void HandleIncorrectKeyAttempt()
    {
        failedAttempts++;

        // Mostrar el primer diálogo solo en el primer intento fallido
        if (failedAttempts == 1)
        {
            firstDialogText.text = firstAttemptMessage;
            firstDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(firstDialogPanel)); // Oculta el primer diálogo después de un tiempo
        }
        // Mostrar el diálogo para intentos múltiples en el 5º, 8º, 11º intento, etc.
        else if (failedAttempts == 5 || failedAttempts == 8 || (failedAttempts >= 11 && (failedAttempts - 5) % 3 == 0))
        {
            multiAttemptDialogText.text = multiAttemptMessage;
            multiAttemptDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(multiAttemptDialogPanel)); // Oculta el panel de intentos múltiples después de un tiempo
        }
        else
        {
            Debug.Log("Llave incorrecta, pero sin mostrar diálogo.");
        }
    }

    private IEnumerator HideDialogAfterDelay(GameObject dialogPanel)
    {
        yield return new WaitForSeconds(dialogDisplayDuration); // Espera el tiempo especificado en el Inspector
        dialogPanel.SetActive(false); // Oculta el panel de diálogo
    }

    private IEnumerator ActivateKeyAfterAnimation()
    {
        yield return new WaitForSeconds(openAnimationDuration); // Espera hasta que la animación termine
        keyObject.SetActive(true); // Activa la llave cuando termina la animación de apertura
        Debug.Log("Llave activada dentro del cofre.");
    }
}

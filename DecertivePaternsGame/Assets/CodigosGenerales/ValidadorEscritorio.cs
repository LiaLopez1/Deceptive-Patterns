using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;              // Referencia al Animator del cofre
    public Canvas interactionHintCanvas;        // Canvas que muestra "Presiona E para interactuar"
    public GameObject keyObject;                // La llave dentro del cofre
    public GameObject firstDialogPanel;         // Panel del di�logo para el primer intento fallido y mensaje de �xito
    public Text firstDialogText;                // Texto para el primer intento fallido y mensaje de �xito
    public GameObject multiAttemptDialogPanel;  // Panel del di�logo para intentos m�ltiples (5�, 8�, etc.)
    public Text multiAttemptDialogText;         // Texto para los intentos m�ltiples

    // Variables para los mensajes de di�logo, configurables en el Inspector
    [TextArea] public string firstAttemptMessage = "La llave es incorrecta. Prueba con otra llave.";
    [TextArea] public string successMessage = "�Cofre abierto exitosamente!";
    [TextArea] public string multiAttemptMessage = "A�n no tienes la llave correcta. Sigue intentando.";

    public float dialogDisplayDuration = 2f;    // Duraci�n del di�logo en pantalla, ajustable desde el Inspector
    public float openAnimationDuration = 2f;    // Duraci�n de la animaci�n de apertura del cofre
    private bool playerInRange = false;         // Verifica si el jugador est� en rango
    private int failedAttempts = 0;             // Contador de intentos fallidos
    private Collider triggerCollider;           // Referencia al Collider del propio Trigger

    private void Start()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        keyObject.SetActive(false); // Desactiva la llave al inicio del juego
        firstDialogPanel.SetActive(false); // Desactiva el primer panel de di�logo al inicio
        multiAttemptDialogPanel.SetActive(false); // Desactiva el panel de intentos m�ltiples al inicio

        // Obtiene el Collider del propio objeto en el que est� el script
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
            chestAnimator.SetTrigger("OpenCajon"); // Activa la animaci�n de apertura del cofre
            triggerCollider.enabled = false; // Desactiva el propio trigger
            firstDialogText.text = successMessage; // Muestra el mensaje de �xito
            firstDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(firstDialogPanel)); // Oculta el panel despu�s de un tiempo
            StartCoroutine(ActivateKeyAfterAnimation()); // Activa la llave despu�s de la animaci�n
        }
        else
        {
            HandleIncorrectKeyAttempt();
        }
    }

    private void HandleIncorrectKeyAttempt()
    {
        failedAttempts++;

        // Mostrar el primer di�logo solo en el primer intento fallido
        if (failedAttempts == 1)
        {
            firstDialogText.text = firstAttemptMessage;
            firstDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(firstDialogPanel)); // Oculta el primer di�logo despu�s de un tiempo
        }
        // Mostrar el di�logo para intentos m�ltiples en el 5�, 8�, 11� intento, etc.
        else if (failedAttempts == 5 || failedAttempts == 8 || (failedAttempts >= 11 && (failedAttempts - 5) % 3 == 0))
        {
            multiAttemptDialogText.text = multiAttemptMessage;
            multiAttemptDialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay(multiAttemptDialogPanel)); // Oculta el panel de intentos m�ltiples despu�s de un tiempo
        }
        else
        {
            Debug.Log("Llave incorrecta, pero sin mostrar di�logo.");
        }
    }

    private IEnumerator HideDialogAfterDelay(GameObject dialogPanel)
    {
        yield return new WaitForSeconds(dialogDisplayDuration); // Espera el tiempo especificado en el Inspector
        dialogPanel.SetActive(false); // Oculta el panel de di�logo
    }

    private IEnumerator ActivateKeyAfterAnimation()
    {
        yield return new WaitForSeconds(openAnimationDuration); // Espera hasta que la animaci�n termine
        keyObject.SetActive(true); // Activa la llave cuando termina la animaci�n de apertura
        Debug.Log("Llave activada dentro del cofre.");
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Safe : MonoBehaviour
{
    public GameObject interactionText;  // Texto que muestra "Presiona E para interactuar"
    public string correctCode = "1234";  // El código correcto para este objeto

    public GameObject objectToAnimate;  // El objeto que contiene el Animator
    public AnimationClip animationClip;  // La animación que se ejecutará al verificar el código correcto
    public GameObject objectToShow;  // El objeto que aparecerá cuando el código sea correcto
    public GameObject dialogPanel;  // Panel de diálogo que se mostrará al ingresar el código correcto
    public Text dialogText;  // Texto del panel de diálogo que se podrá editar
    public string dialogMessage = "Código correcto, caja desbloqueada.";  // Mensaje del diálogo a mostrar
    public float dialogDelay = 1.0f;  // Tiempo de retraso antes de mostrar el diálogo
    public float dialogDuration = 3.0f;  // Duración del diálogo

    private bool isPlayerInTrigger = false;
    private Animator objectAnimator;  // Animator del objeto
    private Animation objectAnimation;  // Si prefieres usar el componente Animation en vez de Animator
    private bool hasBeenUnlocked = false;  // Verifica si el código ya ha sido ingresado correctamente

    private Collider safeTrigger;  // Referencia al Collider (trigger) del objeto

    void Start()
    {
        interactionText.SetActive(false);  // Ocultar el texto de interacción al inicio

        // Obtener el componente Collider (trigger) del objeto
        safeTrigger = GetComponent<Collider>();

        if (objectToAnimate != null)
        {
            objectAnimator = objectToAnimate.GetComponent<Animator>();  // Obtener el componente Animator del objeto a animar
            objectAnimation = objectToAnimate.GetComponent<Animation>();  // Para usar Animation si lo prefieres
        }
        else
        {
            Debug.LogWarning("No se asignó un objeto para animar en " + gameObject.name);
        }

        // Asegurarse de que el objeto a mostrar esté desactivado al inicio
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);  // El objeto permanece oculto hasta que se introduce el código correcto
        }

        // Asegurarse de que el panel de diálogo esté desactivado al inicio
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Mostrar el mensaje solo si el jugador está en el trigger y la caja no ha sido desbloqueada aún
        if (isPlayerInTrigger && !hasBeenUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            KeypadManager.instance.SetCurrentCode(correctCode);  // Pasar el código correcto al KeypadManager
            KeypadManager.instance.SetCurrentObject(this);  // Pasar el objeto actual al KeypadManager
            KeypadManager.instance.ToggleKeypadPanel();  // Mostrar el panel del Keypad
        }
    }

    // Detectar cuando el jugador entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenUnlocked)
        {
            isPlayerInTrigger = true;
            interactionText.SetActive(true);  // Mostrar el texto de interacción
        }
    }

    // Detectar cuando el jugador sale del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactionText.SetActive(false);  // Ocultar el texto de interacción
        }
    }

    // Función para ejecutar la animación del objeto cuando el código es correcto
    public void PlayCorrectCodeAnimation()
    {
        if (objectAnimation != null && animationClip != null)
        {
            objectAnimation.clip = animationClip;
            objectAnimation.Play();  // Ejecutar la animación usando AnimationClip
        }
        else if (objectAnimator != null && animationClip != null)
        {
            objectAnimator.Play(animationClip.name);  // Si prefieres usar Animator con el nombre del AnimationClip
        }
        else
        {
            Debug.LogWarning("No se encontró Animator o Animation en el objeto " + objectToAnimate.name);
        }

        // Marcar que la caja ha sido desbloqueada y desactivar el trigger
        hasBeenUnlocked = true;

        // Mostrar el objeto oculto al introducir el código correcto
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);  // El objeto aparecerá al introducir el código correcto
        }

        // Iniciar la rutina para mostrar el panel de diálogo con un retraso
        StartCoroutine(ShowDialogWithDelay());

        // Desactivar el trigger
        DisableTrigger();

        // Desactivar el panel del Keypad
        KeypadManager.instance.ToggleKeypadPanel();  // Cerrar el panel del Keypad
    }

    // Corrutina para mostrar el diálogo con un retraso y duración específica
    private IEnumerator ShowDialogWithDelay()
    {
        yield return new WaitForSeconds(dialogDelay);  // Esperar el retraso antes de mostrar el diálogo

        if (dialogPanel != null && dialogText != null)
        {
            dialogText.text = dialogMessage;  // Establecer el mensaje del diálogo
            dialogPanel.SetActive(true);
            yield return new WaitForSeconds(dialogDuration);  // Mantener el diálogo visible durante un tiempo
            dialogPanel.SetActive(false);  // Ocultar el panel de diálogo
        }
    }

    // Función para desactivar el trigger del objeto
    private void DisableTrigger()
    {
        if (safeTrigger != null)
        {
            safeTrigger.enabled = false;  // Desactivar el trigger para que no se pueda interactuar más
        }
        else
        {
            Debug.LogWarning("No se encontró un Collider en el objeto " + gameObject.name);
        }

        // Ocultar el texto de interacción, si estaba visible
        interactionText.SetActive(false);
    }
}

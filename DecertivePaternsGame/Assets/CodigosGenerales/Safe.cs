using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Safe : MonoBehaviour
{
    public GameObject interactionText;  // Texto que muestra "Presiona E para interactuar"
    public string correctCode = "1234";  // El c�digo correcto para este objeto

    public GameObject objectToAnimate;  // El objeto que contiene el Animator
    public AnimationClip animationClip;  // La animaci�n que se ejecutar� al verificar el c�digo correcto
    public GameObject objectToShow;  // El objeto que aparecer� cuando el c�digo sea correcto
    public GameObject dialogPanel;  // Panel de di�logo que se mostrar� al ingresar el c�digo correcto
    public Text dialogText;  // Texto del panel de di�logo que se podr� editar
    public string dialogMessage = "C�digo correcto, caja desbloqueada.";  // Mensaje del di�logo a mostrar
    public float dialogDelay = 1.0f;  // Tiempo de retraso antes de mostrar el di�logo
    public float dialogDuration = 3.0f;  // Duraci�n del di�logo

    private bool isPlayerInTrigger = false;
    private Animator objectAnimator;  // Animator del objeto
    private Animation objectAnimation;  // Si prefieres usar el componente Animation en vez de Animator
    private bool hasBeenUnlocked = false;  // Verifica si el c�digo ya ha sido ingresado correctamente

    private Collider safeTrigger;  // Referencia al Collider (trigger) del objeto

    void Start()
    {
        interactionText.SetActive(false);  // Ocultar el texto de interacci�n al inicio

        // Obtener el componente Collider (trigger) del objeto
        safeTrigger = GetComponent<Collider>();

        if (objectToAnimate != null)
        {
            objectAnimator = objectToAnimate.GetComponent<Animator>();  // Obtener el componente Animator del objeto a animar
            objectAnimation = objectToAnimate.GetComponent<Animation>();  // Para usar Animation si lo prefieres
        }
        else
        {
            Debug.LogWarning("No se asign� un objeto para animar en " + gameObject.name);
        }

        // Asegurarse de que el objeto a mostrar est� desactivado al inicio
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);  // El objeto permanece oculto hasta que se introduce el c�digo correcto
        }

        // Asegurarse de que el panel de di�logo est� desactivado al inicio
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Mostrar el mensaje solo si el jugador est� en el trigger y la caja no ha sido desbloqueada a�n
        if (isPlayerInTrigger && !hasBeenUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            KeypadManager.instance.SetCurrentCode(correctCode);  // Pasar el c�digo correcto al KeypadManager
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
            interactionText.SetActive(true);  // Mostrar el texto de interacci�n
        }
    }

    // Detectar cuando el jugador sale del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactionText.SetActive(false);  // Ocultar el texto de interacci�n
        }
    }

    // Funci�n para ejecutar la animaci�n del objeto cuando el c�digo es correcto
    public void PlayCorrectCodeAnimation()
    {
        if (objectAnimation != null && animationClip != null)
        {
            objectAnimation.clip = animationClip;
            objectAnimation.Play();  // Ejecutar la animaci�n usando AnimationClip
        }
        else if (objectAnimator != null && animationClip != null)
        {
            objectAnimator.Play(animationClip.name);  // Si prefieres usar Animator con el nombre del AnimationClip
        }
        else
        {
            Debug.LogWarning("No se encontr� Animator o Animation en el objeto " + objectToAnimate.name);
        }

        // Marcar que la caja ha sido desbloqueada y desactivar el trigger
        hasBeenUnlocked = true;

        // Mostrar el objeto oculto al introducir el c�digo correcto
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);  // El objeto aparecer� al introducir el c�digo correcto
        }

        // Iniciar la rutina para mostrar el panel de di�logo con un retraso
        StartCoroutine(ShowDialogWithDelay());

        // Desactivar el trigger
        DisableTrigger();

        // Desactivar el panel del Keypad
        KeypadManager.instance.ToggleKeypadPanel();  // Cerrar el panel del Keypad
    }

    // Corrutina para mostrar el di�logo con un retraso y duraci�n espec�fica
    private IEnumerator ShowDialogWithDelay()
    {
        yield return new WaitForSeconds(dialogDelay);  // Esperar el retraso antes de mostrar el di�logo

        if (dialogPanel != null && dialogText != null)
        {
            dialogText.text = dialogMessage;  // Establecer el mensaje del di�logo
            dialogPanel.SetActive(true);
            yield return new WaitForSeconds(dialogDuration);  // Mantener el di�logo visible durante un tiempo
            dialogPanel.SetActive(false);  // Ocultar el panel de di�logo
        }
    }

    // Funci�n para desactivar el trigger del objeto
    private void DisableTrigger()
    {
        if (safeTrigger != null)
        {
            safeTrigger.enabled = false;  // Desactivar el trigger para que no se pueda interactuar m�s
        }
        else
        {
            Debug.LogWarning("No se encontr� un Collider en el objeto " + gameObject.name);
        }

        // Ocultar el texto de interacci�n, si estaba visible
        interactionText.SetActive(false);
    }
}

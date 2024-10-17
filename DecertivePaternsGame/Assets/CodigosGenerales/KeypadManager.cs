using UnityEngine;
using UnityEngine.UI;

public class KeypadManager : MonoBehaviour
{
    public static KeypadManager instance;  // Singleton para acceder desde cualquier objeto
    public GameObject keypadPanel;  // Panel del teclado
    public GameObject otherPanel;
    public Text codeDisplay;  // Donde se muestra el código ingresado
    private string enteredCode = "";  // Código que el usuario ingresa
    private string currentCorrectCode;  // El código correcto actual para verificar
    private Safe currentObject;  // Referencia al objeto actual que estamos interactuando

    public PruebaControlador playerController;  // Referencia al controlador del jugador

    private void Awake()
    {
        // Implementación del patrón Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        keypadPanel.SetActive(false);  // Ocultar el panel inicialmente
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        otherPanel.SetActive(true);
    }

    public void ToggleKeypadPanel()
    {
        bool isActive = keypadPanel.activeSelf;
        keypadPanel.SetActive(!isActive);

        if (isActive)
        {
            // Cuando se cierra el Keypad Panel
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ClearCode();
            otherPanel.SetActive(true);
            playerController.enabled = true;  // Desbloquear la cámara cuando se cierra el panel
        }
        else
        {
            // Cuando se abre el Keypad Panel
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            otherPanel.SetActive(false);
            playerController.enabled = false;  // Bloquear la cámara cuando el panel esté activo
        }
    }

    public void SetCurrentCode(string code)
    {
        currentCorrectCode = code;  // Guardar el código correcto del objeto con el que estamos interactuando
    }

    public void SetCurrentObject(Safe obj)
    {
        currentObject = obj;  // Guardar referencia del objeto con el que estamos interactuando
    }

    public void AddDigit(string digit)
    {
        if (enteredCode.Length < 6)  // Limitar el código a 6 dígitos
        {
            enteredCode += digit;
            codeDisplay.text = enteredCode;
        }
    }

    public void CheckCode()
    {
        if (enteredCode == currentCorrectCode)
        {
            Debug.Log("Código correcto: Acción ejecutada");
            if (currentObject != null)
            {
                currentObject.PlayCorrectCodeAnimation();  // Ejecutar la animación del objeto cuando el código es correcto
                playerController.enabled = true;  // Reactivar la cámara al terminar la animación
            }
        }
        else
        {
            Debug.Log("Código incorrecto");
        }
    }

    public void ClearCode()
    {
        enteredCode = "";
        codeDisplay.text = enteredCode;
    }
}

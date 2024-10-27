using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class A : MonoBehaviour
{
    public GameObject[] panels; // Los 4 paneles de anuncios
    public GameObject[] normalPanels; // Los 4 paneles normales
    public GameObject panelAjustes; // El panel de ajustes que se muestra después del anuncio 4
    public GameObject panelCorreo; // Panel para pedir el correo después de desactivar anuncios
    public TMP_InputField correoInputField; // Campo para ingresar el correo en el panelCorreo
    public Button[] BotonesInicio; // Los 4 botones principales
    public Button BotónSí; // Botón para desactivar los paneles de anuncios
    public Button BotónNo; // Botón para reiniciar la secuencia de anuncios
    public Button enviarCorreoButton; // Botón para enviar el correo y activar el Botón 4
    public CanvasGroup boton4CanvasGroup; // CanvasGroup para controlar opacidad y estado de Botón 4

    private int panelIndex = 0;
    private bool panelsEnabled = true;
    private string storedEmail = ""; // Variable para almacenar el correo ingresado

    void Start()
    {
        // Oculta todos los paneles al inicio
        foreach (var panel in panels)
            panel.SetActive(false);
        foreach (var panel in normalPanels)
            panel.SetActive(false);

        panelAjustes.SetActive(false); // Oculta el panel de ajustes al inicio
        panelCorreo.SetActive(false); // Oculta el panel de correo al inicio

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);

        // Configura los botones principales
        for (int i = 0; i < BotonesInicio.Length; i++)
        {
            int index = i; // Captura el índice para el botón correspondiente
            BotonesInicio[i].onClick.AddListener(() => OnMainButtonClick(index));
        }

        // Desactiva el Botón 4 al inicio
        SetButton4Interactable(false);

        // Configura botones de desactivación y reinicio
        BotónSí.onClick.AddListener(DeactivatePanels);
        BotónNo.onClick.AddListener(ResetPanels);
        enviarCorreoButton.onClick.AddListener(ValidarYGuardarCorreo);
    }

    void OnMainButtonClick(int buttonIndex)
    {
        if (panelsEnabled)
        {
            // Muestra el siguiente panel de anuncios en la secuencia
            if (panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                panelIndex++;
            }

            // Si llegamos al último panel de anuncios, el panel de ajustes se mostrará automáticamente debido al `OnClick` configurado
            if (panelIndex == panels.Length)
            {
                BotónSí.gameObject.SetActive(true);
                BotónNo.gameObject.SetActive(true);
            }
        }
        else
        {
            // Si los paneles de anuncios están desactivados, muestra el panel normal correspondiente
            if (buttonIndex < normalPanels.Length)
            {
                normalPanels[buttonIndex].SetActive(true);
            }
        }
    }

    void DeactivatePanels()
    {
        // Oculta todos los paneles de anuncios y muestra el panel de correo
        foreach (var panel in panels)
            panel.SetActive(false);

        panelsEnabled = false;
        panelIndex = 0;

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);
        panelCorreo.SetActive(true); // Muestra el panel de correo para solicitar el correo
    }

    void ResetPanels()
    {
        // Reinicia la secuencia de paneles de anuncios
        foreach (var panel in panels)
            panel.SetActive(false);

        panelIndex = 0;

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);
    }

    void ValidarYGuardarCorreo()
    {
        string email = correoInputField.text;

        // Expresión regular para validar el formato de correo
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (Regex.IsMatch(email, emailPattern))
        {
            storedEmail = email; // Almacena el correo
            SetButton4Interactable(true); // Activa el Botón 4
            panelCorreo.SetActive(false); // Oculta el panel de correo
            Debug.Log("Correo válido y guardado: " + storedEmail);
        }
        else
        {
            Debug.Log("Por favor ingrese un correo válido.");
        }
    }

    void SetButton4Interactable(bool interactable)
    {
        // Usa CanvasGroup para controlar la opacidad y la interacción del Botón 4
        boton4CanvasGroup.interactable = interactable;
        boton4CanvasGroup.blocksRaycasts = interactable;
        boton4CanvasGroup.alpha = interactable ? 1f : 0.5f;

        if (interactable)
        {
            BotonesInicio[3].onClick.AddListener(MostrarPanelAjustes); // Agrega el evento para abrir el panel de ajustes
        }
        else
        {
            BotonesInicio[3].onClick.RemoveAllListeners(); // Remueve cualquier evento si está desactivado
        }
    }

    void MostrarPanelAjustes()
    {
        panelAjustes.SetActive(true); // Muestra el panel de ajustes

        // Activa los botones Sí y No al mostrar el panel de ajustes
        BotónSí.gameObject.SetActive(true);
        BotónNo.gameObject.SetActive(true);
    }
}

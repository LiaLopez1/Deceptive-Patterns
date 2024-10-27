using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class A : MonoBehaviour
{
    public GameObject[] panels; // Los 4 paneles de anuncios
    public GameObject[] normalPanels; // Los 4 paneles normales
    public GameObject panelAjustes; // El panel de ajustes que se muestra despu�s del anuncio 4
    public GameObject panelCorreo; // Panel para pedir el correo despu�s de desactivar anuncios
    public TMP_InputField correoInputField; // Campo para ingresar el correo en el panelCorreo
    public Button[] BotonesInicio; // Los 4 botones principales
    public Button Bot�nS�; // Bot�n para desactivar los paneles de anuncios
    public Button Bot�nNo; // Bot�n para reiniciar la secuencia de anuncios
    public Button enviarCorreoButton; // Bot�n para enviar el correo y activar el Bot�n 4
    public CanvasGroup boton4CanvasGroup; // CanvasGroup para controlar opacidad y estado de Bot�n 4

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

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);

        // Configura los botones principales
        for (int i = 0; i < BotonesInicio.Length; i++)
        {
            int index = i; // Captura el �ndice para el bot�n correspondiente
            BotonesInicio[i].onClick.AddListener(() => OnMainButtonClick(index));
        }

        // Desactiva el Bot�n 4 al inicio
        SetButton4Interactable(false);

        // Configura botones de desactivaci�n y reinicio
        Bot�nS�.onClick.AddListener(DeactivatePanels);
        Bot�nNo.onClick.AddListener(ResetPanels);
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

            // Si llegamos al �ltimo panel de anuncios, el panel de ajustes se mostrar� autom�ticamente debido al `OnClick` configurado
            if (panelIndex == panels.Length)
            {
                Bot�nS�.gameObject.SetActive(true);
                Bot�nNo.gameObject.SetActive(true);
            }
        }
        else
        {
            // Si los paneles de anuncios est�n desactivados, muestra el panel normal correspondiente
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

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);
        panelCorreo.SetActive(true); // Muestra el panel de correo para solicitar el correo
    }

    void ResetPanels()
    {
        // Reinicia la secuencia de paneles de anuncios
        foreach (var panel in panels)
            panel.SetActive(false);

        panelIndex = 0;

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);
    }

    void ValidarYGuardarCorreo()
    {
        string email = correoInputField.text;

        // Expresi�n regular para validar el formato de correo
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (Regex.IsMatch(email, emailPattern))
        {
            storedEmail = email; // Almacena el correo
            SetButton4Interactable(true); // Activa el Bot�n 4
            panelCorreo.SetActive(false); // Oculta el panel de correo
            Debug.Log("Correo v�lido y guardado: " + storedEmail);
        }
        else
        {
            Debug.Log("Por favor ingrese un correo v�lido.");
        }
    }

    void SetButton4Interactable(bool interactable)
    {
        // Usa CanvasGroup para controlar la opacidad y la interacci�n del Bot�n 4
        boton4CanvasGroup.interactable = interactable;
        boton4CanvasGroup.blocksRaycasts = interactable;
        boton4CanvasGroup.alpha = interactable ? 1f : 0.5f;

        if (interactable)
        {
            BotonesInicio[3].onClick.AddListener(MostrarPanelAjustes); // Agrega el evento para abrir el panel de ajustes
        }
        else
        {
            BotonesInicio[3].onClick.RemoveAllListeners(); // Remueve cualquier evento si est� desactivado
        }
    }

    void MostrarPanelAjustes()
    {
        panelAjustes.SetActive(true); // Muestra el panel de ajustes

        // Activa los botones S� y No al mostrar el panel de ajustes
        Bot�nS�.gameObject.SetActive(true);
        Bot�nNo.gameObject.SetActive(true);
    }
}

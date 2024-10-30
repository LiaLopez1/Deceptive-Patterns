using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class A : MonoBehaviour
{
    public GameObject[] panels;           // Paneles de anuncios en secuencia
    public GameObject[] normalPanels;     // Paneles normales
    public GameObject panelAjustes;       // Panel de ajustes
    public GameObject panelCorreo;        // Panel de correo
    public TMP_InputField correoInputField;
    public Button[] BotonesInicio;
    public Button Bot�nS�;
    public Button Bot�nNo;
    public Button enviarCorreoButton;
    public CanvasGroup boton4CanvasGroup;
    public GameObject pcTrigger;

    private int panelIndex = 0;
    private bool panelsEnabled = true;
    private string storedEmail = "";
    private bool reactivarTrigger = false;
    private bool isCorreoIngresado = false;

    void Start()
    {
        // Inicializaci�n: desactiva todos los paneles
        foreach (var panel in panels)
            panel.SetActive(false);
        foreach (var normalPanel in normalPanels)
            normalPanel.SetActive(false);

        panelAjustes.SetActive(false);
        panelCorreo.SetActive(false);

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);

        // Asigna listeners a los botones principales para abrir paneles
        for (int i = 0; i < BotonesInicio.Length; i++)
        {
            int index = i;
            BotonesInicio[i].onClick.AddListener(() => OnMainButtonClick(index));
        }

        SetButton4Interactable(false);
        Bot�nS�.onClick.AddListener(DeactivatePanels);
        Bot�nNo.onClick.AddListener(ResetPanels);
        enviarCorreoButton.onClick.AddListener(ValidarYGuardarCorreo);
    }

    void OnMainButtonClick(int buttonIndex)
    {
        Debug.Log("OnMainButtonClick called with buttonIndex: " + buttonIndex);
        // Si ya hay un panel abierto, no permite abrir otro
        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot open panel. Another panel is already open.");
            return;
        }

        if (panelsEnabled)
        {
            // Abre el siguiente panel de anuncios en secuencia
            if (panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                Debug.Log("Panel " + panelIndex + " opened.");
                panelIndex++;
            }

            // Si llega al �ltimo panel, muestra opciones para desactivar anuncios
            if (panelIndex == panels.Length)
            {
                Bot�nS�.gameObject.SetActive(true);
                Bot�nNo.gameObject.SetActive(true);
                Debug.Log("All panels shown. Displaying options to deactivate panels.");
            }
        }
        else
        {
            // Abre un panel normal si los anuncios est�n desactivados
            if (buttonIndex < normalPanels.Length)
            {
                normalPanels[buttonIndex].SetActive(true);
                Debug.Log("Normal panel " + buttonIndex + " opened.");
            }
        }
    }

    void DeactivatePanels()
    {
        Debug.Log("DeactivatePanels called.");
        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot deactivate panels. Another panel is already open.");
            return;
        }

        panelsEnabled = false;
        panelIndex = 0;

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);
        Debug.Log("Panels deactivated.");

        // Muestra el panel de correo si a�n no se ha ingresado un correo v�lido
        if (!isCorreoIngresado)
        {
            panelCorreo.SetActive(true);
            pcTrigger.SetActive(false);
            reactivarTrigger = true;
            Debug.Log("Correo panel opened.");
        }
    }

    void ResetPanels()
    {
        Debug.Log("ResetPanels called.");
        CerrarPaneles(); // Cierra los paneles activos y restablece el estado

        // Reiniciar el ciclo de los paneles de anuncios
        panelsEnabled = true;
        panelIndex = 0;

        Debug.Log("Panels reset. Starting over the ad sequence.");
    }

    void ValidarYGuardarCorreo()
    {
        Debug.Log("ValidarYGuardarCorreo called.");
        string email = correoInputField.text;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (Regex.IsMatch(email, emailPattern))
        {
            storedEmail = email;
            isCorreoIngresado = true;
            SetButton4Interactable(true);
            panelCorreo.SetActive(false);
            Debug.Log("Correo v�lido y guardado: " + storedEmail);

            // Reactiva el trigger solo si se marc� como necesario
            if (reactivarTrigger)
            {
                pcTrigger.SetActive(true);
                reactivarTrigger = false;
                Debug.Log("pcTrigger reactivated.");
            }
        }
        else
        {
            Debug.Log("Por favor ingrese un correo v�lido.");
        }
    }

    void SetButton4Interactable(bool interactable)
    {
        Debug.Log("SetButton4Interactable called. Interactable: " + interactable);
        boton4CanvasGroup.interactable = interactable;
        boton4CanvasGroup.blocksRaycasts = interactable;
        boton4CanvasGroup.alpha = interactable ? 1f : 0.5f;

        if (interactable)
        {
            BotonesInicio[3].onClick.AddListener(MostrarPanelAjustes);
        }
        else
        {
            BotonesInicio[3].onClick.RemoveAllListeners();
        }
    }

    void MostrarPanelAjustes()
    {
        Debug.Log("MostrarPanelAjustes called.");
        // Si ya hay un panel abierto, no permite abrir otro
        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot open ajustes panel. Another panel is already open.");
            return;
        }

        panelAjustes.SetActive(true);
        Bot�nS�.gameObject.SetActive(true);
        Bot�nNo.gameObject.SetActive(true);
        Debug.Log("Ajustes panel opened.");
    }

    public void CerrarPaneles()
    {
        Debug.Log("CerrarPaneles called.");
        // Cierra todos los paneles activos y permite abrir otro
        foreach (var panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Debug.Log("Panel closed.");
            }
        }

        foreach (var normalPanel in normalPanels)
        {
            if (normalPanel.activeSelf)
            {
                normalPanel.SetActive(false);
                Debug.Log("Normal panel closed.");
            }
        }

        if (panelAjustes.activeSelf)
        {
            panelAjustes.SetActive(false);
            Debug.Log("Ajustes panel closed.");
        }

        if (panelCorreo.activeSelf)
        {
            panelCorreo.SetActive(false);
            Debug.Log("Correo panel closed.");
        }

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);
    }

    bool IsAnyPanelOpen()
    {
        // Revisa si alg�n panel est� activo usando activeSelf
        foreach (var panel in panels)
        {
            if (panel.activeSelf)
            {
                Debug.Log("A panel is currently open.");
                return true;
            }
        }
        foreach (var normalPanel in normalPanels)
        {
            if (normalPanel.activeSelf)
            {
                Debug.Log("A normal panel is currently open.");
                return true;
            }
        }
        if (panelAjustes.activeSelf)
        {
            Debug.Log("Ajustes panel is currently open.");
            return true;
        }
        if (panelCorreo.activeSelf)
        {
            Debug.Log("Correo panel is currently open.");
            return true;
        }

        Debug.Log("No panels are open.");
        return false;
    }
}

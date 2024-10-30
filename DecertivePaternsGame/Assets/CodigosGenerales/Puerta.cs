using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta est� inicialmente bloqueada
    private bool isNear = false; // Si el jugador est� cerca
    public GameObject interactMessagePanel; // Panel de interacci�n
    public GameObject lockedMessagePanel; // Panel de puerta bloqueada
    public bool unlockAfterDelay = false; // Desbloquear despu�s de un tiempo
    public float unlockDelay = 5f; // Tiempo de espera para desbloquear la puerta

    void Start()
    {
        // Aseg�rate de desactivar ambos paneles al inicio
        if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
        if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);

        // Desbloquea la puerta despu�s de unos segundos si est� habilitado
        if (unlockAfterDelay)
        {
            Invoke("UnlockDoor", unlockDelay);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = true; // El jugador est� cerca de la puerta
            UpdateMessagePanel();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = false; // El jugador se aleja de la puerta
            UpdateMessagePanel();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        UpdateMessagePanel();
    }

    void UpdateMessagePanel()
    {
        if (isNear)
        {
            if (isLocked)
            {
                if (lockedMessagePanel != null) lockedMessagePanel.SetActive(true);
                if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
            }
            else
            {
                if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);
                if (interactMessagePanel != null) interactMessagePanel.SetActive(true);
            }
        }
        else
        {
            if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);
            if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocked && Input.GetKeyDown(KeyCode.E) && isNear)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Realiza el movimiento de la puerta aqu�
        transform.Rotate(0, 90f, 0); // Ajusta seg�n necesidad

        // Si tienes una animaci�n de puerta, podr�as activar esa animaci�n en vez de rotarla manualmente.
        Debug.Log("Puerta abierta");
        UpdateMessagePanel(); // Actualiza el panel despu�s de abrir la puerta
    }
}

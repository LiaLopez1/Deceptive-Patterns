using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta está inicialmente bloqueada
    private bool isNear = false; // Si el jugador está cerca
    public GameObject interactMessagePanel; // Panel de interacción
    public GameObject lockedMessagePanel; // Panel de puerta bloqueada
    public bool unlockAfterDelay = false; // Desbloquear después de un tiempo
    public float unlockDelay = 5f; // Tiempo de espera para desbloquear la puerta

    void Start()
    {
        // Asegúrate de desactivar ambos paneles al inicio
        if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
        if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);

        // Desbloquea la puerta después de unos segundos si está habilitado
        if (unlockAfterDelay)
        {
            Invoke("UnlockDoor", unlockDelay);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = true; // El jugador está cerca de la puerta
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
        // Realiza el movimiento de la puerta aquí
        transform.Rotate(0, 90f, 0); // Ajusta según necesidad

        // Si tienes una animación de puerta, podrías activar esa animación en vez de rotarla manualmente.
        Debug.Log("Puerta abierta");
        UpdateMessagePanel(); // Actualiza el panel después de abrir la puerta
    }
}

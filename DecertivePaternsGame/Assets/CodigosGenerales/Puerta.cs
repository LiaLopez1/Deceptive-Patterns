using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta está inicialmente bloqueada
    private bool isNear = false; // Si el jugador está cerca
    private bool isOpen = false; // Estado de la puerta (cerrada inicialmente)
    public GameObject interactMessagePanel; // Panel de interacción
    public GameObject lockedMessagePanel; // Panel de puerta bloqueada
    public bool unlockAfterDelay = false; // Desbloquear después de un tiempo
    public float unlockDelay = 5f; // Tiempo de espera para desbloquear la puerta

    // Referencias de audio
    public AudioClip openSound; // Sonido para abrir la puerta
    public AudioClip closeSound; // Sonido para cerrar la puerta
    private AudioSource audioSource; // Componente AudioSource para reproducir sonidos

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

        // Inicializa el componente AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
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
        // Solo permite la interacción si la puerta no está bloqueada, el jugador está cerca y presiona la tecla E
        if (!isLocked && Input.GetKeyDown(KeyCode.E) && isNear)
        {
            ToggleDoor(); // Alterna entre abrir y cerrar la puerta
        }
    }

    private void ToggleDoor()
    {
        if (isOpen)
        {
            // Si la puerta está abierta, la cerramos (rotamos -90 grados)
            transform.Rotate(0, 90f, 0);
            Debug.Log("Puerta cerrada");
            PlaySound(closeSound); // Reproducimos el sonido de cierre
        }
        else
        {
            // Si la puerta está cerrada, la abrimos (rotamos 90 grados)
            transform.Rotate(0, -90f, 0);
            Debug.Log("Puerta abierta");
            PlaySound(openSound); // Reproducimos el sonido de apertura
        }

        isOpen = !isOpen; // Cambiamos el estado de la puerta
        UpdateMessagePanel(); // Actualizamos el panel después de cambiar el estado de la puerta
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

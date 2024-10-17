using System.Collections;
using UnityEngine;

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock;
    public GameManager gameManager;
    public Servidor servidor;

    public AudioSource audioSource;  // AudioSource de la llave

    private Volumen volumenManager;  // Referencia al script de Volumen para obtener el volumen de efectos
    private bool jugadorEnRango = false;  // Para verificar si el jugador está en rango para recoger la llave

    void Start()
    {
        // Asegurarse de que el AudioSource esté asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Obtener el AudioSource si no está asignado manualmente
        }

        // Obtener una referencia al Volumen Manager para acceder al volumen de efectos
        volumenManager = GameObject.FindObjectOfType<Volumen>();
        if (volumenManager != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  // Asignar el volumen de efectos de sonido inicial
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            // Aquí podrías mostrar un UI para decirle al jugador que presione 'E' para recoger la llave
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            // Aquí podrías ocultar el UI anteriormente mostrado
        }
    }

    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            RecolectarLlave();
        }
    }

    public void RecolectarLlave()
    {
        doorToUnlock.UnlockDoor();
        gameManager.LlaveRecolectada(gameObject.name);

        // Actualiza el contador de llaves
        GameObject.FindObjectOfType<ContadorLlaves>().RecolectarLlave();

        // Reproducir sonido de recolección de la llave con el volumen correcto de efectos
        PlaySound();

        // Actualiza las llaves en la base de datos
        StartCoroutine(ActualizarLlavesEnBD());

        // Destruir la llave después de un breve retraso para permitir que el sonido se reproduzca completamente
        Destroy(gameObject, audioSource.clip.length);
    }

    // Método para reproducir el sonido de recolección
    void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  // Ajustar el volumen según el slider de efectos
            audioSource.Play();  // Reproducir el sonido
        }
    }

    IEnumerator ActualizarLlavesEnBD()
    {
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual;  // Utilizar el nombre del usuario logueado

        // Consumir el servicio "actualizar_llaves"
        StartCoroutine(servidor.ConsumirServicio("actualizar_llaves", datos, null));
        yield return new WaitUntil(() => !servidor.ocupado);
    }
}

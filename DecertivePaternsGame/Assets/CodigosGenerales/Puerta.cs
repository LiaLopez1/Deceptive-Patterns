using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta est� inicialmente bloqueada
    private bool isNear = false; // Si el jugador est� cerca
    private string message = "La puerta est� bloqueada"; // Mensaje inicial

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true; // El jugador est� cerca de la puerta
        }
    }

    void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            isNear = false; // El jugador se aleja de la puerta
        }
    }
    void OnGUI()
    {
        if (isNear)
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 30; // Cambia el tama�o de la fuente
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), message, guiStyle);
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        message = "Presiona 'E' para abrir la puerta"; // Actualiza el mensaje
        Debug.Log("Puerta desbloqueada!");
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

        // igual la puerta tendra una animaci�n, esta debe onerse aqui en vez de eso, o revisarlo.

        Debug.Log("Puerta abierta");
        message = ""; // Limpia el mensaje una vez abierta la puerta
    }
}

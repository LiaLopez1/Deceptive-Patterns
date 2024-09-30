using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta está inicialmente bloqueada
    private bool isNear = false; // Si el jugador está cerca
    private string message = "La puerta está bloqueada"; // Mensaje inicial

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = true; // El jugador está cerca de la puerta
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = false; // El jugador se aleja de la puerta
        }
    }

    void OnGUI()
    {
        if (isNear)
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 30;
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), message, guiStyle);
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        message = "Presiona 'E' para abrir la puerta";
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

        // igual la puerta tendra una animación, esta debe onerse aqui en vez de eso, o revisarlo.

        Debug.Log("Puerta abierta");
        message = ""; // Limpia el mensaje una vez abierta la puerta
    }
}

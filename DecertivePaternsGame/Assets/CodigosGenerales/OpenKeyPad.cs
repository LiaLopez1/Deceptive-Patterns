using UnityEngine;

public class OpenKeyPad : MonoBehaviour
{
    public GameObject keypadOB; // Referencia al objeto del teclado numérico.
    public GameObject keypadText; // Referencia al texto que se muestra para interactuar.
    public bool inReach; // Booleano para saber si el jugador está dentro del rango para interactuar.

    void Start()
    {
        inReach = false;
        keypadOB.SetActive(false);
        SetCursorState(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inReach = true;
            keypadText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inReach = false;
            keypadText.SetActive(false);
            keypadOB.SetActive(false); // Desactiva el teclado al salir del rango.
            SetCursorState(false); // Esconde y bloquea el cursor cuando el jugador sale del rango.
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach)
        {
            keypadOB.SetActive(!keypadOB.activeSelf); // Activa o desactiva el teclado.
            SetCursorState(keypadOB.activeSelf); // Configura el estado del cursor basado en si el teclado está activo.
        }
    }

    private void SetCursorState(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}

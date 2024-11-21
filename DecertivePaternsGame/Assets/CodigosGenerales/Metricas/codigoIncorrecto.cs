using UnityEngine;
using System;

public class codigoIncorrecto : MonoBehaviour
{
    public Safe safeToMonitor;  // Referencia al objeto Safe que se va a monitorear
    public string correctCode = "290";  // C�digo correcto que debe introducir el jugador
    public string incorrectCode = "330";  // C�digo incorrecto para detectar el patr�n enga�oso

    private void OnEnable()
    {
        KeypadManager.OnCodeChecked += OnCodeEntered;
    }

    private void OnDisable()
    {
        KeypadManager.OnCodeChecked -= OnCodeEntered;
    }

    private void OnCodeEntered(string enteredCode)
    {
        // Verificar si el c�digo ingresado es incorrecto espec�fico
        if (enteredCode == incorrectCode)
        {
            Debug.LogWarning("El jugador ha ingresado el c�digo incorrecto ");
        }
    }
}

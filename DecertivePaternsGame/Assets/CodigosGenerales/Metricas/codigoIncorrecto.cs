using UnityEngine;
using System;

public class codigoIncorrecto : MonoBehaviour
{
    public Safe safeToMonitor;  // Referencia al objeto Safe que se va a monitorear
    public string correctCode = "290";  // Código correcto que debe introducir el jugador
    public string incorrectCode = "330";  // Código incorrecto para detectar el patrón engañoso

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
        // Verificar si el código ingresado es incorrecto específico
        if (enteredCode == incorrectCode)
        {
            Debug.LogWarning("El jugador ha ingresado el código incorrecto ");
        }
    }
}

// este codigo es para saber si el jugador activo todas las casillas, entonces si activa el ultimo trigguer que es el que va a tener este
//script entonces significa que cayo en el patron de la primera habitacion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrica1: MonoBehaviour
{
    public int triggerToWatch; // El ID del trigger que queremos monitorear

    private void OnEnable()
    {
        DialogueTrigger.OnTriggerActivated += OnTriggerActivated;
    }

    private void OnDisable()
    {
        DialogueTrigger.OnTriggerActivated -= OnTriggerActivated;
    }

    private void OnTriggerActivated(DialogueTrigger activatedTrigger)
    {
        if (activatedTrigger.triggerSequence == triggerToWatch)
        {
            Debug.Log($"El jugador ha activado el trigger con la secuencia {triggerToWatch}.");
        }
    }
}

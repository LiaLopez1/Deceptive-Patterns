using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeerNotas : MonoBehaviour
{
    public GameObject indicadorInteraccion;  // Objeto que contiene el indicador de interacción (texto de "Presiona 'E' para interactuar")
    public RawImage rawImagenNota;  // Imagen que se mostrará al interactuar
    public Transform cameraTransform;  // Transform de la cámara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast

    private bool isNear = false;  // Si el jugador está cerca del objeto
    private bool isViewingNote = false;  // Si el jugador está viendo la nota

    void Start()
    {
        // Asegurarse de que el indicador y la imagen estén desactivados al inicio
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        RaycastParaMostrarIndicador();

        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!isViewingNote)
            {
                MostrarNota();
            }
            else
            {
                OcultarNota();
            }
        }
    }

    void RaycastParaMostrarIndicador()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "Interactuable"
            if (hit.collider.gameObject == gameObject)
            {
                if (!isNear)
                {
                    isNear = true;
                    if (indicadorInteraccion != null && !isViewingNote)
                    {
                        indicadorInteraccion.SetActive(true);  // Mostrar el texto de interacción
                    }
                }
            }
            else if (isNear)
            {
                isNear = false;
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(false);  // Ocultar el texto de interacción
                }
            }
        }
        else if (isNear)
        {
            isNear = false;
            if (indicadorInteraccion != null)
            {
                indicadorInteraccion.SetActive(false);  // Ocultar el texto de interacción
            }
        }
    }

    void MostrarNota()
    {
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(true);
            isViewingNote = true;
        }
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }
    }

    void OcultarNota()
    {
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(false);
            isViewingNote = false;
        }
        if (isNear && indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(true);
        }
    }
}

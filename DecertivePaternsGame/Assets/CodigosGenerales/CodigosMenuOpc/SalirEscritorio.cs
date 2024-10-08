using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaCompletaManager : MonoBehaviour
{
    public Button salirPantallaCompletaButton;

    void Start()
    {
        salirPantallaCompletaButton.onClick.AddListener(SalirDePantallaCompleta);
    }

    public void SalirDePantallaCompleta()
    {
        Screen.fullScreen = false;
    }
}

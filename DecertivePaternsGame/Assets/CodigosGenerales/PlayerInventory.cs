using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int currentKeyID = -1; // ID de la llave actualmente en posesión del jugador (-1 significa ninguna llave)
    private GameObject currentKeyModel = null; // Referencia al modelo de la llave actualmente en posesión del jugador

    public void SetKey(int keyID, GameObject keyModel)
    {
        currentKeyID = keyID;
        currentKeyModel = keyModel;
    }

    public int GetKey()
    {
        return currentKeyID;
    }

    public GameObject GetKeyModel()
    {
        return currentKeyModel;
    }

    public bool HasKey()
    {
        return currentKeyID != -1;
    }

    public void RemoveKey()
    {
        currentKeyID = -1;
        currentKeyModel = null;
    }
}

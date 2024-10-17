using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad2 : MonoBehaviour
{
    [SerializeField] private Text Ans;

    public void Number(int number)
    {
        Ans.text += number.ToString();
    }


}

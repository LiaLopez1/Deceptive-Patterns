using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Keypad1 : MonoBehaviour
{
    public GameObject player;
    public GameObject keypadOB;
    //public GameObject hud;
    //public GameObject inv;


    public GameObject animateOB;
    public Animator ANI;


    public Text textOB;
    public string answer = "";

    //public AudioSource button;
    //public AudioSource correct;
    //public AudioSource wrong;

    public bool animate;


    void Start()
    {
        keypadOB.SetActive(false);

    }

    public void Number(int number)
    {
        textOB.text += number.ToString();
        //button.Play();   
    }
    public void Execute()
    {
        if (textOB.text == answer)
        {
            //correct.Play();
            textOB.text = "Right";
        }
        else
        {
            //wrong.Play();
            textOB.text = "Wrong";
        }
    }

    public void Clear()
    {
        {
            textOB.text = "";
            //button.Play();
        }
    }

    public void Exit()
    {
        keypadOB.SetActive(false);
        //inv.SetActive(true); //esto es por si hay inventario o objetos a la mano
        //hud.SetActive(true);     
    }

    public void Update()
    {
        if (textOB.text == "Right" && animate)
        {
            ANI.SetBool("animate", true);
            Debug.Log("its open");
        }
    }
}
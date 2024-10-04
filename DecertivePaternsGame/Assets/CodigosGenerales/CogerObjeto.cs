using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CojerObjeto : MonoBehaviour
{
   
    public GameObject HandPoint;
    private GameObject pickedObject = null;


    // Update is called once per frame
    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;

                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("objeto"))
        {
            if (Input.GetKey("e")&& pickedObject ==null) 
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent <Rigidbody>().isKinematic = true;

                other.transform.position = HandPoint.transform.position;
                other.gameObject.transform.SetParent(HandPoint.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }
    }
}

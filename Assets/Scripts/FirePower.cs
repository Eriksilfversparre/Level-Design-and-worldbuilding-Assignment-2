using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePower : MonoBehaviour
{
    /* public GameObject other;
     Rigidbody rb;

     private void Start()
     {
         rb = GetComponent<Rigidbody>();


     }

     public void OnCollisionEnter(Collision collision)
     {
         Debug.LogError("hghfgh");
         SetFireOn();
     }

     public void OnTriggerEnter(Collider other)
     {

         GameObject otherObject = other.gameObject;

         otherObject.GetComponent<Shoot>().firePowerActive = true;
         Debug.LogError("dfdfdff");

     }

     private void SetFireOn()
     {
         GameObject otherObject = other.gameObject;
         otherObject.GetComponent<Shoot>().firePowerActive = true;
         Debug.LogError("worky");

     }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        player.GetComponent<Shoot>().firePowerActive = true;
        Debug.Log("workds");

    }

}

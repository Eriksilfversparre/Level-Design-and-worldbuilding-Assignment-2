using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;
using System;

public class Projectile : MonoBehaviour
{
    /*
    public String whatIHit;

    //public PlayerStats playerStats;

    public NavMeshAgent Enemy;

    //public EnemyErik enemyScript;


    

    public bool iHaveNotHit;
  

    

    public Material playerBulletMat;

    public Material enemyBulletMat;

    private void Start()
    {

       
        if (whatIHit == "Enemy")
        {
            



        }
        else
        {

           

        }


       
        iHaveNotHit = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(whatIHit) && iHaveNotHit)
        {
            if (other.gameObject.name == "Player")
            {
               
                {

                   
                    //playerStats = other.gameObject.GetComponent<PlayerStats>();
                    //playerStats.DecreaseMeter();
                   

                }

                iHaveNotHit = false;
                Destroy(this.gameObject, 1f);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

              
               
                iHaveNotHit = false;
                Destroy(this.gameObject, 1f);
            }

        }

    }*/

    [SerializeField] private Rigidbody projectileBody;
    private bool isActive;
    public float speed;

    public void Initialize()
    {
        isActive = true;
    }

    private void Update()
    {
        if(isActive)
        {
            projectileBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject collisionobject = collision.gameObject;
            Destroy(collisionobject);

        }
        
    }

}

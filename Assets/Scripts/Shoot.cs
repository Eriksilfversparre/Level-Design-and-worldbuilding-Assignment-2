using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;
    public GameObject fireProjectile;
    public float projectileSpeed;
    
    public Vector2 rightStickPosition;
    public GameObject chest;
    public bool firePowerActive;

    public float cooldownTimer = 0;
    public bool canShoot = true;
    public float shootCooldown;

    private void Start()
    {
        firePowerActive = false;
    }
    
    private void Update()
    {
        CanShootWeapon();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            OnFire();
            canShoot = false;
            cooldownTimer = 0;
        }

        if(Input.GetKeyDown(KeyCode.E) && (firePowerActive == true))
        {
            ShootFire();

        }
    }

    private void CanShootWeapon()
    {
        if (canShoot == false)
        {
            cooldownTimer = cooldownTimer + Time.deltaTime;

            if (cooldownTimer >= shootCooldown)
            {
               
                canShoot = true;
                cooldownTimer = 0;
            }
        }
    }


    public void ShootFire()
    {
        GameObject projectileClone2 = Instantiate(fireProjectile, firePoint.transform.position, firePoint.transform.rotation);
        //ojectileClone2.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        //projectileClone.GetComponent<Projectile>().whatIHit = "Enemy";
        //ojectileClone2.GetComponent<SphereCollider>().radius = 10;


        Destroy(projectileClone2, 5f);
    }

    public void OnFire()
    {
        
        
            GameObject projectileClone = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            //ojectileClone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
            //projectileClone.GetComponent<Projectile>().whatIHit = "Enemy";
            //ojectileClone.GetComponent<SphereCollider>().radius = 4;


            Destroy(projectileClone, 2f);

        

    }

    /*public void OnLook(InputValue lookValue)
    {
        rightStickPosition = lookValue.Get<Vector2>();

        if (rightStickPosition != Vector2.zero)
        {
            float angle = Mathf.Atan2(rightStickPosition.x, rightStickPosition.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }*/


}

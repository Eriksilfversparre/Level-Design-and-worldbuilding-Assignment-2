using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{
    [SerializeField] private Rigidbody projectileBody;
    private bool isActive;
    public float speed;

    public void Initialize()
    {
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            projectileBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            GameObject collisionobject = collision.gameObject;
            Destroy(collisionobject);

        }

    }
}

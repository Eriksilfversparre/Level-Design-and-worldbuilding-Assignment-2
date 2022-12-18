using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{

    public Vector3 cameraOffset;
    public Transform Player;


    private void LateUpdate()
    {
        transform.position = Player.transform.position + cameraOffset;
    }
}

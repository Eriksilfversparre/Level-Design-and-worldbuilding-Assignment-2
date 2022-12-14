using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera123 : MonoBehaviour
{

    public Vector3 cameraOffset;
    public Transform Player;


    private void LateUpdate()
    {
        transform.position = Player.transform.position + cameraOffset;
    }
}

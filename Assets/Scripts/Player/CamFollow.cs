using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float smoothspeed = 0.125f;
    public Vector3 offset;


    void LateUpdate()
    {
        if (!GameObject.Find("Main Camera").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CamAnim")) {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothspeed);
            transform.position = smoothedPosition;
           
        }
    }
}

﻿using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -10f;
    

    public void Attract(Transform body)
    {
        Vector3 gravityUp = body.position - transform.position;
        gravityUp.Normalize();
        Vector3 bodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(gravityUp*gravity, ForceMode.Acceleration);
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 100*Time.deltaTime);
    }
}

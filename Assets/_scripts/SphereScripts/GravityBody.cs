using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour {

    public GravityAttractor attractor;
    private Transform MyTransform;
    void Start()
    {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        myRigidbody.useGravity = false;
        MyTransform = transform;
    }

    void Update()
    {
        attractor.Attract(MyTransform);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceField : MonoBehaviour {

    // Make sure to add the line 'using System.Collections.Generic'
    // on the first line of the file
    public float forceRadius = 20;
    public float gravPower = 9.81f;
    void FixedUpdate()
    {
        // Populate a list of nearby bodies
        List<Rigidbody> bodies = new List<Rigidbody>();
        foreach (Collider col in Physics.OverlapSphere
            (transform.position, forceRadius))
        {
            if (col.attachedRigidbody != null && !bodies.Contains(col.attachedRigidbody))
            {
                bodies.Add(col.attachedRigidbody);
            }
        }
        // Now you have your rigidbodies, time to add the force!
        foreach (Rigidbody body in bodies)
        {
            float bodyDist = (body.position - transform.position).sqrMagnitude;
            float gravStrengthFactor = forceRadius / bodyDist;
            body.AddForce(gravStrengthFactor * gravPower *
               (transform.position - body.position) * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}

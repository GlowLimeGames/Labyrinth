@script AddComponentMenu ("Forces/Local/Mouse Directional Force")
@script RequireComponent(Rigidbody)

var Force: Transform;
var ForceSpeed: float = 1;



function ActiveForce(){

        
var MyObjectsForce : Vector3  = Force.transform.TransformDirection (0, 1, 0);      

//GameObjects
gameObject.GetComponent.<Rigidbody>().AddForce (MyObjectsForce * ForceSpeed);
}


function OnDrawGizmosSelected () {
		// Draws a 5 meter long red line in front of the object
		Gizmos.color = Color.red;
		var direction : Vector3 = transform.TransformDirection (Vector3(0,1,0)) * 5;
		Gizmos.DrawRay (transform.position, direction);
	}
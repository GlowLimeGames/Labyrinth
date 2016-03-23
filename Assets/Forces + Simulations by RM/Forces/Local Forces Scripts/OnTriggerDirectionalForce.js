@script AddComponentMenu ("Forces/Local/OnTrigger Directional Force")

var ForceSpeed: float = 1;
var TriggerDrag: float = 0;
var SpaceDrag: float = 0;

function OnTriggerStay (other : Collider) { 
        
var MyObjectsForce : Vector3  = gameObject.transform.TransformDirection (0, 1, 0);      

//GameObjects
other.GetComponent.<Rigidbody>().AddForce (MyObjectsForce * ForceSpeed);

other.GetComponent.<Rigidbody>().drag = TriggerDrag;


}

function OnTriggerExit (other : Collider) { 

//GameObjects
other.GetComponent.<Rigidbody>().drag = SpaceDrag;

}


function OnDrawGizmosSelected () {
		// Draws a 5 meter long red line in front of the object
		Gizmos.color = Color.red;
		var direction : Vector3 = transform.TransformDirection (Vector3(0,1,0)) * 5;
		Gizmos.DrawRay (transform.position, direction);
	}
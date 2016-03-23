@script AddComponentMenu ("Forces/Local/OnTrigger Spherical Force")

var Force: Transform;
var ForceSpeed: float = 1;

var TriggerDrag: float = 0;
var SpaceDrag: float = 0;

function OnTriggerStay (other : Collider) { 
        
var GameObjectsForce = Force.transform.position - other.transform.position;

GameObjectsForce = GameObjectsForce.normalized;

other.GetComponent.<Rigidbody>().AddForce (GameObjectsForce *ForceSpeed);

other.GetComponent.<Rigidbody>().drag = TriggerDrag;

}

function OnTriggerExit (other : Collider) { 

other.GetComponent.<Rigidbody>().drag = SpaceDrag;
}

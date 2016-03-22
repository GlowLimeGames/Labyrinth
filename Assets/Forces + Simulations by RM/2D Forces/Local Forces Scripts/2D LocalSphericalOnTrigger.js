@script AddComponentMenu ("Forces 2D/Local/OnTrigger Spherical Force")

var Force: Transform;
var ForcePower: float = 1;
var TriggerDrag: float = 0;
var SpaceDrag: float = 0;


function OnTriggerStay2D(other: Collider2D) {

var GameObjectsForce = Force.transform.position - other.transform.position;

GameObjectsForce = GameObjectsForce.normalized;

other.GetComponent.<Rigidbody2D>().AddForce (GameObjectsForce *ForcePower);

other.GetComponent.<Rigidbody2D>().drag = TriggerDrag;
}


function OnTriggerExit2D(other: Collider2D) {

//GameObjects
other.GetComponent.<Rigidbody2D>().drag = SpaceDrag;

}
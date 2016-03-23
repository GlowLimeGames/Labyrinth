@script AddComponentMenu ("Forces/Local/Mouse Directional Force")
@script RequireComponent(Rigidbody)

var Force: Transform;
var ForceSpeed: float = 1;



function ActiveForce(){

        
var MyObjectsForce : Vector3  = Force.transform.TransformDirection (0, 1, 0);      

//GameObjects
gameObject.GetComponent.<Rigidbody>().AddForce (MyObjectsForce * ForceSpeed);
}
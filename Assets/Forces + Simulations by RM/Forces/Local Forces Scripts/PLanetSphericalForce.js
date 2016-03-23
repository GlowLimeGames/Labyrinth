﻿@script AddComponentMenu ("Forces/Local/Planet Spherical Force")
@script RequireComponent(Rigidbody)


var OnCollision: boolean = true;
var OnTrigger: boolean = true;

var Force: Transform;
var ForceSpeed: float = 1;
var CollisonDrag:float = 20;
var TriggerDrag:float = 20;
var SpaceDrag: float = 0;





function Update () { 
        
var GameObjects1WindForce = Force.transform.position - gameObject.transform.position;

GameObjects1WindForce = GameObjects1WindForce.normalized;

gameObject.GetComponent.<Rigidbody>().AddForce (GameObjects1WindForce *ForceSpeed);

}


function OnCollisionStay (other : Collision) { 
if (OnCollision == true){
gameObject.GetComponent.<Rigidbody>().drag = CollisonDrag;
}
}

function OnCollisionExit (other : Collision) { 
if (OnCollision == true){
gameObject.GetComponent.<Rigidbody>().drag = SpaceDrag;
}
}

function OnTriggerStay (other : Collider) { 
if (OnTrigger == true){
gameObject.GetComponent.<Rigidbody>().drag = TriggerDrag;
}
}

function OnTriggerExit (other : Collider) { 
if (OnTrigger == true){
gameObject.GetComponent.<Rigidbody>().drag = SpaceDrag;
}
}
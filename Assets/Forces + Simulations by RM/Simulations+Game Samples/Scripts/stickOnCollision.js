

var CollisonDrag:float = 20;
var SpaceDrag: float = 0;

function OnCollisionStay (other : Collision) { 

other.rigidbody.drag = CollisonDrag;
}


function OnCollisionExit (other : Collision) { 

other.rigidbody.drag = SpaceDrag;
}

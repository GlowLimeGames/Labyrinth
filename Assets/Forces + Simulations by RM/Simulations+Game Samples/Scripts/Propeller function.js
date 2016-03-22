var Propeler: Transform;
var Force : Transform;
var FollowForceDirection: boolean = false; 
var Active: boolean = false;
var OnMouse: boolean = false;

function Update() {
if (Active == true){
    var Speed = Force.GetComponent(DirectionalForce).ForceSpeed;
Propeler.transform.Rotate(Time.deltaTime*Speed*40, 0, 0);
		}
	if (FollowForceDirection == true){
transform.rotation =  Force.transform.rotation;

}


if(Input.GetMouseButton(1)){

if (OnMouse == true){

    var Speed2 = Force.GetComponent(DirectionalForce).ForceSpeed;
Propeler.transform.Rotate(Time.deltaTime*Speed2*40, 0, 0);

	if (FollowForceDirection == true){
transform.rotation =  Force.transform.rotation;
}
}
}
	}
	


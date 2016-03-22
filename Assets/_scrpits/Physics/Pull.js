#pragma strict
//**************************************************
//This script goes on the object with the most mass.
//**************************************************
//Set Gravational Constant. use this for time scaling.
static var Gcons : float = 0.000000000067;
function Start () {
 
}  
 
function Update () {
    var obj1 : GameObject;
    var obj2 : GameObject;
    var SolarBody : GameObject[] = GameObject.FindGameObjectsWithTag ("Solar");
    var dist : float;
    var gforce : Vector3;
    var i : int;
    var j : int;
    for (j=0;j<SolarBody.length;j++){
        for (i=j+1;i<SolarBody.length;i++) {
            //Assign objects to be gravitized.
            obj1=SolarBody[j];
            obj2=SolarBody[i];
            //Get distance between objects
            dist = Vector3.Distance(obj1.transform.position,obj2.transform.position);
            //Calculate force of gravity
            gforce = Vector3.Normalize(obj2.transform.position - obj1.transform.position)*Gcons*obj1.GetComponent.<Rigidbody>().mass*1000000000000000*obj2.GetComponent.<Rigidbody>().mass*1000000000000000/(dist*dist)*Time.deltaTime;
            //Apply Gravity
            obj1.GetComponent.<Rigidbody>().AddForce(gforce);
            obj2.GetComponent.<Rigidbody>().AddForce(-gforce);
        }
    }
}
 
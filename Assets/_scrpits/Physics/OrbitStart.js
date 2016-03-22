#pragma strict
 
//**********************************************
//This script goes on all 'Solar' tagged objects
//except for the object with the highest mass.
//**********************************************
 
function Start () {
    var OrbitTarget : GameObject;
    var obj1 : GameObject;
    var obj2 : GameObject;
    var CentralObject : GameObject;
    var SolarBody : GameObject[] = GameObject.FindGameObjectsWithTag ("Solar");
    var dist : float;
    var gforce : Vector3;
    var gforce1 : Vector3;
    var gforce2 : Vector3;
    var i : int;
    var NeedVel : float;
    var NeedForce : float;
    //Find the object with the highest gforce with this object
    for (i=0;i<SolarBody.length;i++) {
        if (OrbitTarget!=null){
            if(SolarBody[i]!=gameObject){
                if (OrbitTarget!=SolarBody[i]){
                    obj1=gameObject;
                    obj2=SolarBody[i];
                    dist = Vector3.Distance(obj1.transform.position,obj2.transform.position);
                    gforce1 = Vector3.Normalize(obj2.transform.position - obj1.transform.position)*Pull.Gcons*obj1.GetComponent.<Rigidbody>().mass*1000000000000000*obj2.GetComponent.<Rigidbody>().mass*1000000000000000/(dist*dist)/2*Time.deltaTime;
                    dist = Vector3.Distance(obj1.transform.position,OrbitTarget.transform.position);
                    gforce2 = Vector3.Normalize(OrbitTarget.transform.position - obj1.transform.position)*Pull.Gcons*obj1.GetComponent.<Rigidbody>().mass*1000000000000000*OrbitTarget.GetComponent.<Rigidbody>().mass*1000000000000000/(dist*dist)/2*Time.deltaTime;                    
                    if (gforce1.magnitude>gforce2.magnitude){
                        OrbitTarget=SolarBody[i];
                    }
                }
            }
        }else{
            OrbitTarget=SolarBody[i];
            if(SolarBody[i]!=gameObject){
                if (OrbitTarget!=SolarBody[i]){
                    obj1=gameObject;
                    obj2=SolarBody[i];
                    dist = Vector3.Distance(obj1.transform.position,obj2.transform.position);
                    gforce1 = Vector3.Normalize(obj2.transform.position - obj1.transform.position)*Pull.Gcons*obj1.GetComponent.<Rigidbody>().mass*1000000000000000*obj2.GetComponent.<Rigidbody>().mass*1000000000000000/(dist*dist)/2*Time.deltaTime;
                    dist = Vector3.Distance(obj1.transform.position,OrbitTarget.transform.position);
                    gforce2 = Vector3.Normalize(OrbitTarget.transform.position - obj1.transform.position)*Pull.Gcons*obj1.GetComponent.<Rigidbody>().mass*1000000000000000*OrbitTarget.GetComponent.<Rigidbody>().mass*1000000000000000/(dist*dist)/2*Time.deltaTime;                    
                    if (gforce1.magnitude>gforce2.magnitude){
                        OrbitTarget=SolarBody[i];
                    }
                }
            }
        }
    }
    //Assign objects to be started.
    obj1=gameObject;
    obj2=OrbitTarget;
    //Get distance between objects
    dist = Vector3.Distance(obj1.transform.position,obj2.transform.position);
    //Determine the velocity needed for orbit.
    Debug.Log(obj1.name);
    NeedVel = Mathf.Sqrt(obj2.GetComponent.<Rigidbody>().mass*1000000000000000*Pull.Gcons/dist);
    Debug.Log(NeedVel);
    //Determine Force needed to reach that velocity
    NeedForce = obj1.GetComponent.<Rigidbody>().mass*1000000000000000*(NeedVel/Time.fixedDeltaTime);
    Debug.Log(NeedForce);
    //Get the direction of gravity's force
    gforce=Vector3.Normalize(obj2.transform.position - obj1.transform.position);
    //Apply the Needed force perpendicular to gravity's force
    GetComponent.<Rigidbody>().AddForce(Vector3(gforce.z,gforce.y,gforce.x)*NeedForce);
}
 
function Update () {
 
}
 
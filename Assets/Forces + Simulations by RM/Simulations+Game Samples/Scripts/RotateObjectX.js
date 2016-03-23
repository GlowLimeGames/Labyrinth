var speed: float;
var MyObject: GameObject;




function Update () {
  MyObject.transform.Rotate(Time.deltaTime*speed ,0,0); 
    // rotate 90 degrees around the object's local Y axis:
}
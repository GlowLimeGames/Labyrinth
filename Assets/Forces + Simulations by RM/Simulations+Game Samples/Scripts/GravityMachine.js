

import System.Linq;

var Active: boolean = true; 
var OnMouseEvent: boolean = false;
var OnCollision: boolean = false;
var OnTrigger: boolean = false;
var ObjectsTagName: String;
var Cloths: Cloth[] = null;
var ParticleSystems: ParticleEmitter[] = null;
var ForceRange: float = 1;
var ForceSpeed: float = 1;
var ObjDrag: float = 1;
var ObjAnglularDrag: float = 1;
var Glass : Transform;
var LightB : Light;
var LightA : Light;


private var key_pressed: int =0;

function Update () {

if (Active == true){

ActiveForce();
}
else
{

if (OnMouseEvent == true){

FunctionOnMouse();
}
}
}


//ACTIVE

function ActiveForce(){
     
var GameObjects = GameObject.FindGameObjectsWithTag(ObjectsTagName).Select(function (go) {return go.transform;}).ToArray();

     
for (var i = 0; i < GameObjects.length; i++)
        {
        
          
 var range = Vector3.Distance( GameObjects[i].transform.position,this.transform.position );



       if ( range <= ForceRange ){   
        
        
var GameObjects1WindForce = this.transform.position - GameObjects[i].position;

GameObjects1WindForce = GameObjects1WindForce.normalized;


GameObjects[i].GetComponent.<Rigidbody>().AddForce (GameObjects1WindForce *ForceSpeed);

}
}

//Cloths
if (Cloths != null) {

for (var i2 = 0; i2 < Cloths.length; i2++)
        {
        
range = Vector3.Distance(Cloths[i2].gameObject.transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ClothWindForce = this.transform.position - Cloths[i2].gameObject.transform.position;

ClothWindForce = ClothWindForce.normalized; 

Cloths[i2].externalAcceleration = (ClothWindForce *ForceSpeed) -Physics.gravity/10;

}
else{
for (var i3 = 0; i3 < Cloths.length; i3++)
        {
if ( range >= ForceRange ){  
Cloths[i3].externalAcceleration = Vector3(0, 0, 0);
}
}
}
}
}

//Particle Systems
if (ParticleSystems != null){

for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {

range = Vector3.Distance(ParticleSystems[i5].transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ParticlesWindForce = this.transform.position - ParticleSystems[i5].transform.localPosition;

ParticlesWindForce = ParticlesWindForce.normalized; 


ParticleSystems[i5].localVelocity = (ParticlesWindForce *ForceSpeed);

}
else{

if ( range >= ForceRange ){ 
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
}
}
}
}

//ON MOUSE

function FunctionOnMouse(){



if(Input.GetKeyDown(KeyCode.E)) {
  if(key_pressed%2 ==0) {

ForceSpeed = -ForceSpeed;

LightA.intensity = 0;
LightB.intensity = 8;
 key_pressed++;
}
else if  (key_pressed%2 ==1) {
ForceSpeed = -ForceSpeed;

LightB.intensity = 0;
LightA.intensity = 8;
 key_pressed++;
}
}



 if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            ForceSpeed += 2;
            Glass.GetComponent.<Renderer>().material.color += Color32(2, 2, 2, 0);

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            ForceSpeed -=  2;
            Glass.GetComponent.<Renderer>().material.color -= Color32(2, 2, 2, 0);
        
        }

var GameObjects = GameObject.FindGameObjectsWithTag(ObjectsTagName).Select(function (go) {return go.transform;}).ToArray();
for (var i = 0; i < GameObjects.length; i++)
        {

if(Input.GetMouseButton(1)){


        
          
 var range = Vector3.Distance( GameObjects[i].transform.position,this.transform.position );



       if ( range <= ForceRange ){   
        
        
var GameObjects1WindForce = this.transform.position - GameObjects[i].position;

GameObjects1WindForce = GameObjects1WindForce.normalized;


GameObjects[i].GetComponent.<Rigidbody>().AddForce (GameObjects1WindForce *ForceSpeed);

GameObjects[i].GetComponent.<Rigidbody>().useGravity = false;

GameObjects[i].GetComponent.<Rigidbody>().drag = ObjDrag;

GameObjects[i].GetComponent.<Rigidbody>().angularDrag = ObjAnglularDrag;

}
}
else {
//Debug.Log("Not held");
GameObjects[i].GetComponent.<Rigidbody>().useGravity = true;
GameObjects[i].GetComponent.<Rigidbody>().drag = 0;
}

}


//Cloths

if(Input.GetMouseButton(1)){

if (Cloths != null) {

for (var i2 = 0; i2 < Cloths.length; i2++)
        {
        
range = Vector3.Distance(Cloths[i2].gameObject.transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ClothWindForce = this.transform.position - Cloths[i2].gameObject.transform.position;

ClothWindForce = ClothWindForce.normalized; 

Cloths[i2].externalAcceleration = (ClothWindForce *ForceSpeed) -Physics.gravity/10;


}
}
}

}
else { 
for (var i3 = 0; i3 < Cloths.length; i3++)
        {
Cloths[i3].externalAcceleration = Vector3(0, 0, 0);

}
}
//Particle Systems

if (ParticleSystems != null){

if(Input.GetMouseButton(1)){

for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {

range = Vector3.Distance(ParticleSystems[i5].transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ParticlesWindForce = this.transform.position - ParticleSystems[i5].transform.localPosition;

ParticlesWindForce = ParticlesWindForce.normalized; 


ParticleSystems[i5].localVelocity = (ParticlesWindForce *ForceSpeed);

}
else{

if ( range >= ForceRange ){ 
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
}
}
}
else{
for (var i6 = 0; i6 < ParticleSystems.length; i6++)
        {
ParticleSystems[i6].localVelocity = Vector3(0, 0, 0);
}
}
}
}


//ON COLLISION


function OnCollisionEnter(collision : Collision) {	
		
if (OnCollision == true){

var GameObjects = GameObject.FindGameObjectsWithTag(ObjectsTagName).Select(function (go) {return go.transform;}).ToArray();
for (var i = 0; i < GameObjects.length; i++)
        {
        
          
 var range = Vector3.Distance( GameObjects[i].transform.position,this.transform.position );



       if ( range <= ForceRange ){   
        
        
var GameObjects1WindForce = this.transform.position - GameObjects[i].position;

GameObjects1WindForce = GameObjects1WindForce.normalized;


GameObjects[i].GetComponent.<Rigidbody>().AddForce (GameObjects1WindForce *ForceSpeed);



}
}
//Cloths
if (Cloths != null) {

for (var i2 = 0; i2 < Cloths.length; i2++)
        {
        
range = Vector3.Distance(Cloths[i2].gameObject.transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ClothWindForce = this.transform.position - Cloths[i2].gameObject.transform.position;

ClothWindForce = ClothWindForce.normalized; 

Cloths[i2].externalAcceleration = (ClothWindForce *ForceSpeed) -Physics.gravity/10;

}
else{
 if ( range >= ForceRange ){ 
for (var i3 = 0; i3 < Cloths.length; i3++)
        {
        if ( range >= ForceRange ){ 
Cloths[i3].externalAcceleration = Vector3(0, 0, 0);

}
}
}
}
}
}

//Particle Systems
if (ParticleSystems != null){

for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {

range = Vector3.Distance(ParticleSystems[i5].transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ParticlesWindForce = this.transform.position - ParticleSystems[i5].transform.localPosition;

ParticlesWindForce = ParticlesWindForce.normalized; 


ParticleSystems[i5].localVelocity = (ParticlesWindForce *ForceSpeed);

}
else{

if ( range >= ForceRange ){ 
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
}
}
}
}
}

function OnCollisionExit(collision : Collision) {

if (OnCollision == true){
//ParticleSystems
for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
        
//Cloths
for (var i2 = 0; i2 < Cloths.length; i2++)
        {
Cloths[i2].externalAcceleration = Vector3(0, 0, 0);
}
}
}



//ON TRIGGER
function OnTriggerStay (other : Collider) {
	
if (OnTrigger == true){

var GameObjects = GameObject.FindGameObjectsWithTag(ObjectsTagName).Select(function (go) {return go.transform;}).ToArray();
for (var i = 0; i < GameObjects.length; i++)
        {
        
          
 var range = Vector3.Distance( GameObjects[i].transform.position,this.transform.position );



       if ( range <= ForceRange ){   
        
        
var GameObjects1WindForce = this.transform.position - GameObjects[i].position;

GameObjects1WindForce = GameObjects1WindForce.normalized;


GameObjects[i].GetComponent.<Rigidbody>().AddForce (GameObjects1WindForce *ForceSpeed);



}
}
//Cloths
if (Cloths != null) {

for (var i2 = 0; i2 < Cloths.length; i2++)
        {
        
range = Vector3.Distance(Cloths[i2].gameObject.transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ClothWindForce = this.transform.position - Cloths[i2].gameObject.transform.position;

ClothWindForce = ClothWindForce.normalized; 

Cloths[i2].externalAcceleration = (ClothWindForce *ForceSpeed) -Physics.gravity/10;

}
else{
 
for (var i3 = 0; i3 < Cloths.length; i3++)
        {
        if ( range >= ForceRange ){ 
Cloths[i3].externalAcceleration = Vector3(0, 0, 0);

}
}
}
}
}
//Particle Systems
if (ParticleSystems != null){

for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {

range = Vector3.Distance(ParticleSystems[i5].transform.position,this.transform.position );

 if ( range <= ForceRange ){   
        
        
var ParticlesWindForce = this.transform.position - ParticleSystems[i5].transform.localPosition;

ParticlesWindForce = ParticlesWindForce.normalized; 


ParticleSystems[i5].localVelocity = (ParticlesWindForce *ForceSpeed);

}
else{

if ( range >= ForceRange ){ 
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
}
}
}
}
}
function OnTriggerExit (other : Collider) {

if (OnTrigger == true){

//ParticleSystems
for (var i5 = 0; i5 < ParticleSystems.length; i5++)
        {
ParticleSystems[i5].localVelocity = Vector3(0, 0, 0);
}
//Cloths
for (var i4 = 0; i4 < Cloths.length; i4++)
        {
        
Cloths[i4].externalAcceleration = Vector3(0, 0, 0);
}
}
}

//DRAW GIZMOS

	function OnDrawGizmosSelected () {
		
		var Radius = ForceRange;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, Radius);
	}
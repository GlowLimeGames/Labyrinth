
var DestroyTag: String;

function OnTriggerEnter(other : Collider)
{
  if (other.gameObject.tag == DestroyTag)
     Destroy(gameObject);
     Application.LoadLevel(Application.loadedLevel);
}
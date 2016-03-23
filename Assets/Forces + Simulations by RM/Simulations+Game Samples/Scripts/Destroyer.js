#pragma strict

function OnTriggerStay (other : Collider) {
		Destroy(other.gameObject);
	}
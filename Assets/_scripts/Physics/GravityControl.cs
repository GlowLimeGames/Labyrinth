using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour {

    // Use this for initialization
    private float gravity = 9.81f;
    private bool isOn = true;
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        changeGravity();
        Physics.gravity = new Vector3(0, -gravity, 0);
	
	}
    void Update()
    {

       
    }

    
    void changeGravity()
    {
        //if (Input.GetButton("Fire1") && isOn == true) {
        //    gravity = 0;
        //    isOn = false;
        //    Debug.Log("Should be off");
        //}
        //if (Input.GetButton("Fire1") && isOn == false) {
        //    gravity = 9.81f;
        //    isOn = true;
        //    Debug.Log("Should be on!");
        //        }


    }
}

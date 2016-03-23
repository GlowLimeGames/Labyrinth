var GameOverGui : GameObject;
	
	
	function Start (){
	Time.timeScale = 1.0;
	}
	
	function Update () {
		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (0);  
			
	}
	
	function OnCollisionEnter2D (collision : Collision2D) {
		GameOverGui.SetActive (true);
		if (Time.timeScale == 1.0)
				Time.timeScale = 0.0;
	}

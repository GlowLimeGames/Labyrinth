# UPFT Eye-Gaze Input version 1.0.0

## How to Use

### Basic

1. Adding script to the object to set "eye gaze".  
Please add the "Eye Gaze Input" to the object contains MainCamera.

2. Adding script to the object to set "eye gaze".  
Please add the "Eye Gaze Receiver" to the object you wish to do some action by being stare.

3. Define the action
When the object that has "Eye Gaze Receiver" is being stared by the object that has "Eye Gaze Receiver", "OnGazeBegin" message will send. And if hte eye gaze　left, "OnGazeEnd" message will send. You can set unique actions to adding script to the object, and using "OnGazeBegin" method and "OnGazeEnd" method.

### Simple Button

1. Adding script to the object to set "eye gaze".  
Please add the "Eye Gaze Input" to the object contains MainCamera.

2. Place the button  
Drag and drop "/EyeGazeInput/SimpleEyeGazeButton/Prefabs/SimpleEyeGazeButton.prefab" to Hierarchy.

3. Define the action  
When the object that has "Eye Gaze Receiver" is being stared by the object that has "Eye Gaze Receiver", "OnGazeBegin" message will send. And if hte eye gaze　left, "OnGazeEnd" message will send. You can set unique actions to adding script to the object, and using "OnGazeBegin" method and "OnGazeEnd" method.

## Customize  

Following parameters are variable at Inspector.

### Simple Eye Gaze Button

* Duration
	* Time of the call to "OnButtonGaze".  
	
* Normal Bg Texture
	* the button background image when it has not been staring.
	
* Gazing Bg Texture
	* the button background image when it is staring.

## Release History

### 1.0.0

Initial Release


# UPFT Eye-Gaze Input version 1.0.0

## How to Use

### Basic

1. 凝視するオブジェクトにスクリプトを追加する  
MainCameraを持つオブジェクトに "Eye Gaze Input" を追加してください。

2. 凝視されるオブジェクトにスクリプトを追加する  
凝視されることで何らかのアクションを行いたいオブジェクトに "Eye Gaze Receiver" を追加してください。

3. アクションを記述する  
"Eye Gaze Receiver" を追加されたオブジェクトが "Eye Gaze Input" を追加されたオブジェクトに凝視されると、凝視されたオブジェクトに OnGazeBegin メッセージが発行されます。また、視線が離れると OnGazeEnd メッセージが発行されます。  
このオブジェクトに任意のスクリプトを追加し、OnGazeBegin メソッドと OnGazeEnd メソッドを記述することで任意のアクションを行います。

### Simple Button

1. 凝視するオブジェクトにスクリプトを追加する  
MainCameraを持つオブジェクトに "Eye Gaze Input" を追加してください。

2. ボタンを配置する  
/EyeGazeInput/SimpleEyeGazeButton/Prefabs/SimpleEyeGazeButton.prefabをHierarchyにドラッグ＆ドロップしてください。

3. アクションを記述する  
"Eye Gaze Receiver" を追加されたオブジェクトが "Eye Gaze Input" を追加されたオブジェクトに一定時間凝視され続けると、凝視されたオブジェクトに OnButtonGaze メッセージが発行されます。このオブジェクトに任意のスクリプトを追加し、OnGazeButton メソッドを記述することで任意のアクションを行います。

## Customize  

Inspector上では以下の設定が可能です。

### Simple Eye Gaze Button

* Duration
	* OnButtonGaze を呼び出すまでの時間です。  
	
* Normal Bg Texture
	* 凝視されていない時のボタンの背景画像です。
	
* Gazing Bg Texture
	* 凝視されている時のボタンの背景画像です。

## Release History

### 1.0.0

Initial Release


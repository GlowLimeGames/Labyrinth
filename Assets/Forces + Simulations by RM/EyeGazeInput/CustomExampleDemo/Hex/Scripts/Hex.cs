using UnityEngine;
using System.Collections;

namespace Upft
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(EyeGazeButton))]
	public class Hex : MonoBehaviour
	{
		private Animator animator;
		private EyeGazeButton button;
		private EyeGazeReceiver receiver;

		private int focusedId = Animator.StringToHash ("Focused");
		private int shaderMainColorId;
		private int shaderProgressId;
		private HexParams hexParams;

		public Renderer baseHexRenderer;
		public Renderer hexBackRenderer;

		private float oldProgress = 0f;

		void Awake ()
		{
			shaderMainColorId = Shader.PropertyToID ("_Color");
			shaderProgressId = Shader.PropertyToID ("_Progress");
			animator = GetComponent<Animator> ();
			receiver = GetComponent<EyeGazeReceiver> ();
			button = GetComponent<EyeGazeButton> ();
		}

		void OnGazeBegin ()
		{
			animator.SetBool (focusedId, true);
		}

		void OnGazeEnd ()
		{
			animator.SetBool (focusedId, false);
		}

		void OnButtonGaze ()
		{
			GameObject.FindObjectOfType<Background> ().stableColor = hexParams.hexColor;
		}

		public void SetHexParams (HexParams hexParams)
		{
			this.hexParams = hexParams;
		
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock ();
			baseHexRenderer.GetPropertyBlock (propertyBlock);
			propertyBlock.AddColor (shaderMainColorId, hexParams.hexColor);
			baseHexRenderer.SetPropertyBlock (propertyBlock);

			Util.SpheralPosition (transform, hexParams.x, hexParams.y, 1f);

			receiver.receive = true;
		}

		void Update ()
		{
			float normalizedGazeTime = button.normalizedGazeTime;
			if (oldProgress != normalizedGazeTime) {
				MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock ();
				hexBackRenderer.GetPropertyBlock (propertyBlock);
				propertyBlock.AddFloat (shaderProgressId, normalizedGazeTime);
				hexBackRenderer.SetPropertyBlock (propertyBlock);

				oldProgress = normalizedGazeTime;
			}
		}

		[System.Serializable]
		public class HexParams
		{
			public Color hexColor;
			[HideInInspector]
			public float
				x;
			[HideInInspector]
			public float
				y;
		}
	}
}
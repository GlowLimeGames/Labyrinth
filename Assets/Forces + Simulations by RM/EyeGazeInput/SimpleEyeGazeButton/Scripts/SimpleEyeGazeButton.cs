using UnityEngine;
using System.Collections;

namespace Upft
{
	public class SimpleEyeGazeButton : EyeGazeButton
	{	
		public Renderer buttonRenderer;
		public Texture2D normalBgTexture;
		public Texture2D gazingBgTexture;
		
		private int shaderFillId;
		private int shaderBgId;

		private float oldGazeTime = 0f;

		void Awake ()
		{
			shaderFillId = Shader.PropertyToID ("_Fill");
			shaderBgId = Shader.PropertyToID ("_MainTex");

			SetNormalTexture ();
		}

		protected void Update ()
		{
			base.Update ();

			if (normalizedGazeTime != oldGazeTime) {
				MaterialPropertyBlock block = new MaterialPropertyBlock ();
				buttonRenderer.GetPropertyBlock (block);
				block.AddFloat (shaderFillId, normalizedGazeTime);
				buttonRenderer.SetPropertyBlock (block);

				oldGazeTime = normalizedGazeTime;
			}
		}

		void OnButtonGaze ()
		{
			Debug.Log ("<color=red><size=20>Button was gazing!!</size></color>");

			SetNormalTexture ();
		}

		void OnGazeBegin ()
		{
			base.OnGazeBegin ();

			SetGazingTexture ();
		}
		
		void OnGazeEnd ()
		{
			base.OnGazeEnd ();

			SetNormalTexture ();
		}

		void SetNormalTexture ()
		{
			MaterialPropertyBlock block = new MaterialPropertyBlock ();
			buttonRenderer.GetPropertyBlock (block);
			block.AddTexture (shaderBgId, normalBgTexture);
			buttonRenderer.SetPropertyBlock (block);
		}

		void SetGazingTexture ()
		{
			MaterialPropertyBlock block = new MaterialPropertyBlock ();
			buttonRenderer.GetPropertyBlock (block);
			block.AddTexture (shaderBgId, gazingBgTexture);
			buttonRenderer.SetPropertyBlock (block);
		}
	}
}
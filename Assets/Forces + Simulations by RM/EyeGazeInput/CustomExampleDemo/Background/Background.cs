using UnityEngine;
using System.Collections;

namespace Upft
{
	public class Background : MonoBehaviour
	{
		public Color stableColor;
		private Color color;

		void Start ()
		{
			color = stableColor;
		}

		void Update ()
		{
			color = Color.Lerp (color, stableColor, 0.1f);
		
			MaterialPropertyBlock block = new MaterialPropertyBlock ();
			GetComponent<Renderer>().GetPropertyBlock (block);
			block.AddColor ("_MainColor", Color.Lerp (color, Color.white, 0.3f));
			block.AddColor ("_WaveColor", Color.Lerp (color, Color.white, 0.5f));
			GetComponent<Renderer>().SetPropertyBlock (block);
		}
	}
}
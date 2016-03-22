using UnityEngine;
using System.Collections;

namespace Upft
{
	public static class Util
	{
		public static bool SpheralPosition (Transform transform, float x, float y, float distance)
		{
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			if (player == null) {
				return false;
			}
			Transform target = player.transform;
			Quaternion q = Quaternion.Euler (-y, x, 0f);
			Vector3 dis = q * target.forward;
			transform.position = target.position + dis * distance;
			transform.LookAt (target.position);
			
			return true;
		}

		public static Color HSVToRGB (float h, float s, float v)
		{
			float r = 0f;
			float g = 0f; 
			float b = 0f;
			float f, p, q, t;
			int i = (int)Mathf.Floor (h * 6);
			f = h * 6 - i;
			p = v * (1 - s);
			q = v * (1 - f * s);
			t = v * (1 - (1 - f) * s);
			switch (i % 6) {
			case 0:
				r = v;
				g = t;
				b = p;
				break;
			case 1:
				r = q;
				g = v;
				b = p;
				break;
			case 2:
				r = p;
				g = v;
				b = t;
				break;
			case 3:
				r = p;
				g = q;
				b = v;
				break;
			case 4:
				r = t;
				g = p;
				b = v;
				break;
			case 5:
				r = v;
				g = p;
				b = q;
				break;
			}
			return new Color (r, g, b); 
		}
	}
}
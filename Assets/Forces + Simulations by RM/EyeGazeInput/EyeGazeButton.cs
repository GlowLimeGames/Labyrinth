using UnityEngine;
using System.Collections;

namespace Upft
{
	[RequireComponent(typeof(EyeGazeReceiver))]
	public class EyeGazeButton : MonoBehaviour
	{
		public float duration = 1.8f;
		private float gazeBeginTime = float.NaN;

		public float gazeTime {
			get {
				if (float.IsNaN (gazeBeginTime)) {
					return 0f;
				} else {
					return Time.time - gazeBeginTime;
				}
			}
		}

		public float normalizedGazeTime {
			get {
				return gazeTime / duration;
			}
		}
	
		protected void Update ()
		{
			if (normalizedGazeTime > 1f) {
				gazeBeginTime = float.NaN;
				gameObject.SendMessage ("OnButtonGaze", SendMessageOptions.DontRequireReceiver);	
			}
		}

		protected void OnGazeBegin ()
		{
            Debug.Log("Works!");
            Physics.gravity = new Vector3(0, 0, 0);
			gazeBeginTime = Time.time;
		}

		protected void OnGazeEnd ()
		{
			gazeBeginTime = float.NaN;
		}
	}
}
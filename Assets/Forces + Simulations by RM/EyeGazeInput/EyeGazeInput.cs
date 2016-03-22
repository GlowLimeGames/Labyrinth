using UnityEngine;
using System.Collections;

namespace Upft
{
	public sealed class EyeGazeInput : MonoBehaviour
	{
		public bool gaze = true;
		public bool parentRecursive = true;

		void Update ()
		{
			if (gaze) { 
				RaycastHit[] hits = Physics.RaycastAll (transform.position, transform.forward);
			
				foreach (var hit in hits) {
					EyeGazeReceiver receiver = hit.collider.gameObject.GetComponent<EyeGazeReceiver> ();
					if (parentRecursive) {
						receiver = hit.collider.gameObject.GetComponentInParent<EyeGazeReceiver> ();
					} else {
						receiver = hit.collider.gameObject.GetComponent<EyeGazeReceiver> ();
					}
					if (receiver) {
						if (receiver.receive) {
							receiver.OnGazeFrame ();
						}
					}
				}
			}
		}

		void OnDrawGizmos ()
		{
			if (gaze) {
				Color c = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawRay (transform.position, transform.forward * 100);
				Gizmos.color = c;
			}
		}
	}
}
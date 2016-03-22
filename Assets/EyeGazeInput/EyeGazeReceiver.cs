using UnityEngine;
using System.Collections;

namespace Upft
{
	public sealed class EyeGazeReceiver : MonoBehaviour
	{
		private bool isGazeFrame = false;
		private bool isGazeFrameOld = false;
        private Physics cube;
		public bool receive = true;
        public GameObject camera;


        void Awake() {
        
    

        }

		void Update ()
		{
			if (isGazeFrameOld ^ isGazeFrame) {
				if (isGazeFrame) {
					gameObject.SendMessage ("OnGazeBegin", SendMessageOptions.DontRequireReceiver);
				} else {
					gameObject.SendMessage ("OnGazeEnd", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (isGazeFrame) {
				gameObject.SendMessage ("OnGazing", SendMessageOptions.DontRequireReceiver);
			}

			isGazeFrameOld = isGazeFrame;
			isGazeFrame = false;
		}

		public void OnGazeFrame ()
		{
			isGazeFrame = true;
		}

        protected void OnGazeBegin()
        {

            GetComponent<Rigidbody>().AddForce(camera.transform.forward* 100);
            Debug.Log("Works1111111");
        }

        protected void OnGazeEnd()
        {
        }

    }
}
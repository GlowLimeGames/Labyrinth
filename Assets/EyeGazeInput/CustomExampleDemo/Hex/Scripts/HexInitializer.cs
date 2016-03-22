using UnityEngine;
using System.Collections;
using Upft;

namespace Upft
{
	public class HexInitializer : MonoBehaviour
	{
		public GameObject
			hexPrefab;
		public int horizontalNum = 3;
		public int verticalNum = 3;

		public int horizontalMaxNum = 9;
		public int verticalMaxNum = 9;

		private GameObject[] instances = new GameObject[0];

		void Awake ()
		{
			Hex.HexParams[] hexParams = new Hex.HexParams[horizontalNum * verticalNum];

			for (int i = 0; i < hexParams.Length; i++) {
				hexParams [i] = new Hex.HexParams (){
				hexColor = Util.HSVToRGB(Random.value, 1f, 1f)
			};
			}

			instances = new GameObject[hexParams.Length];
			for (int i = 0; i < instances.Length; i++) {
				GameObject hex = GameObject.Instantiate (hexPrefab) as GameObject;
				hex.transform.parent = this.transform;
				instances [i] = hex;

				int x = (i % horizontalNum);
				int y = (i / horizontalNum);
				float xFactor = 360f / (float)horizontalMaxNum;
				float yFactor = 180f / (float)verticalMaxNum;
			
				hexParams [i].x = (xFactor * (x - (horizontalNum - 1) + y % 2 / 2f));
				hexParams [i].y = (yFactor * (y - (verticalNum - 1) / 2f));
				hex.GetComponentInChildren<Hex> ().SetHexParams (hexParams [i]);
			}
		}
	}
}
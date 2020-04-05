using UnityEngine;

namespace Multiplayer
{
	public class Billboard : MonoBehaviour {

		private GameObject targetCamera;

		public void Start()
		{
			targetCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		private void Update () {
			transform.LookAt(targetCamera.transform);
		}
	}
}

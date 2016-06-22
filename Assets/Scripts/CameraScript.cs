using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float xBound = 10f;
	public GameObject followTarget;
	public Vector3 offsetPos;
	public float scrollSpeed = 3f; // bigger number = camera faster scroll

	void Update () {

		// The Lerp here is used very differently. Normally, we lerp with a fixed
		// start/end position... but here, our start/end position is updated every
		// frame. The "t" value is not [0.0 ~ 1.0] anymore, instead we use deltaTime.

		// This produces that you will see when you test the game -- when the camera
		// is far from the player, it will scroll faster; when the camera is nearer,
		// it will scroll slower.

		Vector3 oldPos = transform.position;
		Vector3 targetPos = followTarget.transform.position - offsetPos;
		Vector3 newPos = Vector3.Lerp(oldPos, targetPos, Time.deltaTime * scrollSpeed);

		if (newPos.x < -xBound)
			newPos.x = -xBound;
		if (newPos.x > xBound)
			newPos.x = xBound;

		newPos.z = offsetPos.z; // don't update the camera up or down, just move it left or right

		transform.position = newPos;
	
	}
}

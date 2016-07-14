using UnityEngine;
using System.Collections;

public class FlotatingCameraController : MonoBehaviour {

	private Vector3 originalPos;

	public float moveSpeed;
	public float maxRange;

	// Use this for initialization
	void Start () 
	{
		originalPos = transform.localPosition;

		StartCoroutine ("CameraTranslation");
	}


	IEnumerator CameraTranslation() 
	{

		while (true) {
			float elapsedTime = 0;
			float newOffset = Random.Range (-maxRange, maxRange);
			Vector3 newPos = originalPos + Vector3.forward * newOffset;
			while (Vector3.Distance (transform.localPosition, newPos) > 0.5f) {
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp (transform.localPosition.z, newPos.z, (elapsedTime / moveSpeed)));
					elapsedTime += Time.deltaTime;
					yield return null;
			}
			yield return null;
		}
	}
}

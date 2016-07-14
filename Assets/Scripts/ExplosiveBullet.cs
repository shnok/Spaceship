using UnityEngine;
using System.Collections;

public class ExplosiveBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("AutoDestroy", 2f);
	}
	
	IEnumerator AutoDestroy(float time) {
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}
}

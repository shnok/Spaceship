/* A coller sur les particles effects à détruire */

using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float duration;

	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyOb ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DestroyOb(){
		yield return new WaitForSeconds (duration);
		Destroy (gameObject);
	}
}

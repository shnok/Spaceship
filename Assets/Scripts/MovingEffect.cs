using UnityEngine;
using System.Collections;

public class MovingEffect : MonoBehaviour {

	public float turnSpeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.eulerAngles = transform.eulerAngles + (Vector3.up * turnSpeed * Time.deltaTime);
	}
}

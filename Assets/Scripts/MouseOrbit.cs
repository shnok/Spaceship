/* Ce script gère la rotation de la caméra
 * et des tourelles en fonction des axes
 * de la souris */

using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {

	/* Position/Rotation */
	private float x = 0.0F;
	private float y = 0.0F;
	private float yMinLimit = -89F;
	private float yMaxLimit = 15F;
	private Quaternion rotation;
	private Vector3 smoothPosition;

	/* Smooth zoom */
	private float startTime = 0; 
	private float startFov = 0;
	public float zoomDuration = 0.10f;

	public Transform cameraTransform;

	public Transform pivot; /* Tourelle à laquelle est attachée notre caméra */
	public float cameraSpeed; /* Vitesse de la caméra */
	public float sensitivity; /* Sensibilité de la caméra */
	public Vector3 camOffset; /* Offset de la caméra */

	void Update () 
	{
		CameraControl ();

		ZoomFunction ();

	}

	/* Gère les déplacements de la caméra */
	private void CameraControl()
	{

		x += Input.GetAxis ("Mouse X") * sensitivity * Time.deltaTime;
		y -= Input.GetAxis ("Mouse Y") * sensitivity * Time.deltaTime;
		y = ClampAngle (y, yMinLimit, yMaxLimit);

		rotation = Quaternion.Euler(y, x, 0);

		/* Déplacement lisse de la caméra autour de la tourelle */
		smoothPosition = Vector3.Lerp (smoothPosition, pivot.right * camOffset.x + pivot.up * camOffset.y + pivot.forward * camOffset.z + pivot.position ,Time.deltaTime*cameraSpeed);

		/* Effectue une rotation de la tourelle à laquelle est attachée notre caméra */
		pivot.eulerAngles = transform.eulerAngles + Vector3.right * 90;

		transform.localRotation = rotation;	
		transform.position = smoothPosition;
	}

	/* Gère le zoom */
	private void ZoomFunction()
	{
		if (Input.GetMouseButtonDown (1) || Input.GetMouseButtonUp (1)) {
			startTime = Time.time;
			startFov = cameraTransform.GetComponent<Camera> ().fieldOfView;
		}

		float elapsedTime = (Time.time - startTime);
		float ratio = elapsedTime / zoomDuration;

		if (GameManager.activeTurret != TurretType.Turret2) {
			if (Input.GetMouseButton (1)) {
				cameraTransform.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (startFov, 40, ratio);
			} else {
				if (cameraTransform.GetComponent<Camera> ().fieldOfView < 60) {
					cameraTransform.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (startFov, 60, ratio);
				}
			}
		} else {
			if (Input.GetMouseButton (1)) {
				GameManager.zooming = true;
				cameraTransform.GetComponent<Camera> ().fieldOfView = 20;
			} else {
				GameManager.zooming = false;
				cameraTransform.GetComponent<Camera> ().fieldOfView = 60;
			}
		}
			
	}

	/* Permet de gérer les limites d'angles */
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}

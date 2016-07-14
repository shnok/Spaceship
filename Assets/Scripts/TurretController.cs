/* Ce script gère l'animation des
 * tourelles et le système de tirs */

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TurretController : MonoBehaviour {

	private Animator animator;
	public GameObject bullet;
	public Camera turretCamera;
	public TurretType turretID;
	public List<Transform> turretElements;
	private Vector3 direction;
	private float distance;
	private bool hasATarget = false;
	public RaycastHit[] hit;

	private float lastFire;

	public float LoadValue;
	public bool startLoad = false;

	void Start()
	{
		animator = GetComponent<Animator> ();
		//turretCamera = GameManager.turretCamera.GetComponent<Camera> ();
		//turretCamera = GameObject.Find ("TurretCamera").GetComponent<Camera> ();
	}

	void Update ()
	{
		HandleMouse ();

		hasATarget = DetectTargets ();
	}

	/* Change la couleur de la tourelle sélectionnée dans le menu, 
	 * active l'animation de tir lors de l'appuie sur le clic gauche de la souris */ 

	public void HandleMouse()
	{
		if (!GameManager.menu) {

			if (GameManager.activeTurret == turretID) {

				/* Turret 1 */
				if (turretID == TurretType.Turret1) {
					if (Input.GetMouseButton (0)) {
						animator.SetBool ("Fire", true);

					} else {
						animator.SetBool ("Fire", false);
					}
				}

				/* Turret 2 */
				if (turretID == TurretType.Turret2) {
					if (Input.GetMouseButtonDown (0)) {
						if (Time.time - lastFire >= 1f) {
							lastFire = Time.time;
							animator.SetBool ("Fire", true);
						}
					} else {
						animator.SetBool ("Fire", false);
					}
				}

				/* Turret 3 */
				if (turretID == TurretType.Turret3) {
					GameManager.loadValue = LoadValue;

					if (Input.GetMouseButtonDown (0)) {
						startLoad = true;
					}

					if (Input.GetMouseButton (0) && startLoad) {
						if (LoadValue < 1)
							LoadValue += Time.deltaTime * 1.25f;
						
						if (LoadValue > 1)
							LoadValue = 1;
						if (LoadValue == 1) {
							startLoad = false;
							animator.SetBool ("Fire", true);
							LoadValue = 0;
						}

					} else {
						if (LoadValue > 0)
							LoadValue -= Time.deltaTime * 0.25f;
						if (LoadValue < 0)
							LoadValue = 0;
						
						animator.SetBool ("Fire", false);
					}

				} 

				SwitchColor (Color.white);

			} else {
				animator.SetBool ("Fire", false);
			} 

			if (GameManager.activeTurret != TurretType.Turret3) {
					LoadValue = 0;
					GameManager.loadValue = 0;
			}
		} else {

			if (GameManager.mouseOverButton != 0 && GameManager.mouseOverButton == turretID) {
				SwitchColor (Color.yellow);
			} else {
				SwitchColor (Color.white);
			}
		}
	}

	/* Vérifie si l'on vise un ennemi et recalcule la direction de notre projectile */
	public bool DetectTargets()
	{
		hit = Physics.RaycastAll (turretCamera.transform.position, turretCamera.transform.forward,  200f, 512);
		if(Physics.Raycast(turretCamera.transform.position, turretCamera.transform.forward, 200f, 512)){
			var heading = hit[0].point - transform.position;
			distance = heading.magnitude;
			direction = heading / distance;
			return true;
		} else {
			direction = turretCamera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0)).direction;
			return false;
		}
	}

	/* Fonction de tir: instantie un projectile et lui attribue 
	 * tous les paramètres utiles en fonction du type de tourelle */
	public void Fire()
	{
		GameObject prop;
		GameObject prop2;

		prop =  (GameObject)Instantiate (bullet, transform.position + transform.up*0.65f, Quaternion.identity);
		prop.transform.SetParent(GameObject.Find("World").transform);

		if (turretID == TurretType.Turret1) {


			prop2 = (GameObject)Instantiate (bullet, transform.position + transform.up * 0.65f + transform.right * 0.15f, Quaternion.identity);
			prop2.transform.SetParent (GameObject.Find ("World").transform);

			if (hasATarget) {
				prop.GetComponent<BulletScript> ().targets.Add(hit[0].transform);
				prop.GetComponent<BulletScript> ().hitDistance = distance;
				prop2.GetComponent<BulletScript> ().targets.Add(hit[0].transform);
				prop2.GetComponent<BulletScript> ().hitDistance = distance;
			}

			prop.GetComponent<Rigidbody> ().AddForce (direction * 75000);
			prop.GetComponent<BulletScript> ().sender = transform;
			prop2.GetComponent<Rigidbody> ().AddForce (direction * 75000);
			prop2.GetComponent<BulletScript> ().sender = transform;
			prop.GetComponent<BulletScript> ().speed = 75000;
			prop2.GetComponent<BulletScript> ().speed = 75000;

			prop.GetComponent<BulletScript> ().myType = BulletType.Normal;
			prop2.GetComponent<BulletScript> ().myType = BulletType.Normal;
		}


		if (turretID == TurretType.Turret2) {

			if (hasATarget) {
				prop.GetComponent<BulletScript> ().hitDistance = distance;
				for (int i = 0; i < hit.Length; i++) {
					prop.GetComponent<BulletScript> ().targets.Add (hit[i].transform);
				}
			}

			prop.GetComponent<Rigidbody> ().AddForce (direction * 75000);
			prop.GetComponent<BulletScript> ().sender = transform;
			prop.GetComponent<BulletScript> ().speed = 75000;

			prop.GetComponent<BulletScript> ().myType = BulletType.Normal;

		}

		if (turretID == TurretType.Turret3) {

			if (hasATarget) {
				prop.GetComponent<BulletScript> ().hitDistance = distance;
				for (int i = 0; i < hit.Length; i++) {
					prop.GetComponent<BulletScript> ().targets.Add (hit[i].transform);
				}
			}

			prop.GetComponent<Rigidbody> ().AddForce (direction * 3000);
			prop.GetComponent<BulletScript> ().sender = transform;
			prop.GetComponent<BulletScript> ().speed = 3000;
			prop.GetComponent<BulletScript> ().myType = BulletType.Explosive;


		}


		if (GameManager.menu) {
			direction = transform.up;
		} 


	}

	/* Change la couleur de la tourelle */
	private void SwitchColor(Color c)
	{
		foreach (Transform t in turretElements) {
			t.GetComponent<MeshRenderer> ().material.color = c;
		}
	}
}

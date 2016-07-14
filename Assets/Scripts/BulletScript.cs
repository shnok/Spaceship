/* Script attaché aux projectiles */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BulletType{
	Normal,
	Explosive
}

public class BulletScript : MonoBehaviour {

	public Vector3 moveDirection = Vector3.zero; 
	public float hitDistance;
	public Transform sender;
	public float speed;
	public List<Transform> targets = new List<Transform>();
	public BulletType myType;
	public GameObject explosionPrefab;
	private bool messageSent = false;

	void Awake()
	{
		StartCoroutine (DestroyAfter(10));
	}

	void Update()
	{
		if (messageSent)
			return;
		/* Si on avait une cible dans le viseur lors du tir ,
			* détruit notre projectile ainsi que la cible
			* lorsque sa distance est égale à celle mésurée lors
			* de son envoi */
		if (myType == BulletType.Normal) {
			
			if (targets.Count == 0)
				return;

			if (Vector3.Distance (transform.position, sender.position) >= hitDistance) {
				GetComponentInChildren<Light> ().enabled = false;
				GetComponent<Rigidbody> ().velocity = Vector3.zero;

				for (int i = 0; i < targets.Count; i++) {
					if (targets [i] != null)
						targets [i].SendMessageUpwards ("GetDamage");
				}

				messageSent = true;

				StartCoroutine (DestroyAfter (1f));
			} else {
				if (GameManager.menu == true) {
					GetComponent<Rigidbody> ().velocity = Vector3.zero;
				} 
			}
		}

		/* Disparait lorsque l'on touche une cible puis instancie une explosion */
		GameObject explosion;
		if (myType == BulletType.Explosive) {
			Collider[] col = Physics.OverlapSphere (transform.position, 3f, 512);
			Collider[] explosionRange = Physics.OverlapSphere (transform.position, 15f, 512);
			if (col.Length != 0) {
				explosion = (GameObject)Instantiate (explosionPrefab, transform.position, Quaternion.identity);
				foreach (Collider c in explosionRange) {
					c.SendMessageUpwards ("GetDamage");
				}

				messageSent = true;
				StartCoroutine (DestroyAfter (1f));
			}
		}
	}
		
	/* Détruit notre projectile après une durée */	
	IEnumerator DestroyAfter(float duration)
	{
		yield return new WaitForSeconds (duration);
		Destroy (gameObject);
	}
}

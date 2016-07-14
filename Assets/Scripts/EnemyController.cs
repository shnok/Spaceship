/* Script attaché aux ennemis
 * gère les déplacements de l'objet */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ShipType{
	Boss,
	Normal
}

public class EnemyController : MonoBehaviour {

	public Vector3 moveDirection;
	public Vector3 rotateSpeed;
	public float speed;
	public bool autoDestroy = false;
	public GameObject particleExplosion;
	public ShipType myType = ShipType.Normal;
	public int HP = 150;

	void Start()
	{
		if(autoDestroy)
			StartCoroutine ("AutoDestroy");
	}

	void Update () {
		if (GameManager.menu == false) {
			transform.localEulerAngles = transform.localEulerAngles + Vector3.forward * Time.deltaTime * rotateSpeed.x + Vector3.right * Time.deltaTime * rotateSpeed.y + Vector3.up * Time.deltaTime * rotateSpeed.z;
			transform.position = transform.position + ((transform.right * moveDirection.x) + (transform.up * moveDirection.y) + (transform.forward * moveDirection.z)) * Time.deltaTime * 1f;
		}

		if (myType == ShipType.Boss) {
			GlobalUI.UI_Elements ["BossBar"].SetActive (true);
			GlobalUI.UI_Elements ["BossBar"].GetComponent<Image> ().fillAmount = (float)((float)HP / (150f));
			GlobalUI.UI_Elements ["BossBar"].transform.FindChild ("BossHp").GetComponent<Text> ().text = HP.ToString () + "/150";
		}
	}

	/* Fonction appelée lors que l'objet est touché par un projectile */
	public void GetDamage()
	{
		if (myType == ShipType.Boss) {
			Debug.Log ("hit");
			HP--;
			if (HP <= 0) {
				GameManager.kills += 100;
				GameObject particle = (GameObject)Instantiate (particleExplosion, transform.position, Quaternion.identity);
				GlobalUI.UI_Elements ["BossBar"].SetActive (false);
				Destroy (gameObject);
			}
		} else {
			GameManager.kills++;
			GameObject particle = (GameObject)Instantiate (particleExplosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}

	/* Détruit le vaisseau après un certain délai */
	IEnumerator AutoDestroy()
	{
		if (myType == ShipType.Normal) {
			yield return new WaitForSeconds (15f);
		} else {
			yield return new WaitForSeconds (30f);
		}

		if(myType == ShipType.Boss)
			GameManager.misses+=100;
		else 
			GameManager.misses++;
		
		GlobalUI.UI_Elements ["BossBar"].SetActive (false);
		Destroy (gameObject);

	}

}

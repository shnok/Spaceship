/* Ce script gère l'apparition de vagues d'ennemis 
 * il est à mettre sur un gameobject qui va être
 * un point de spawn */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Types de vagues d'ennemis */
public enum WaveType{
	Triangle,
	Basic,
	Boss
}

/* Structure d'une vague */
[System.Serializable]
public struct WaveData{
	public WaveType type;
	public int number;
	public Vector3 direction;
	public Vector3 rotation;
}

public class Spawner : MonoBehaviour {

	public GameObject shipPrefab;
	public GameObject bossPrefab;

	/* Liste dans laquelle va appararaître toutes les vagues */
	public List<WaveData> waves = new List<WaveData>();

	IEnumerator SpawnWave(int waveNumber)
	{

		if (waveNumber > waves.Count)
			yield break;
		
		switch (waves [waveNumber].type) {

		/* Triangle*/
		case WaveType.Triangle:
			for (int i = 0; i < waves [waveNumber].number; i++) {
						
				for (int j = 0; j < i * 2 + 1; j++) {

					GameObject go = (GameObject)Instantiate (shipPrefab, transform.position + transform.right * (3 - i) * 5f + transform.right * j * 5f, Quaternion.identity);
					EnemyController goScript = go.GetComponent<EnemyController> ();
					go.transform.SetParent (GameObject.Find ("World").transform);
					go.transform.eulerAngles = transform.eulerAngles;

					goScript.moveDirection = waves [waveNumber].direction;
					goScript.rotateSpeed = waves [waveNumber].rotation; 

				}

				yield return new WaitForSeconds (0.5f);
			}
			break;

		case WaveType.Basic:
			for (int i = 0; i < waves [waveNumber].number; i++) {

				GameObject go = (GameObject)Instantiate (shipPrefab, transform.position, Quaternion.identity);
				EnemyController goScript = go.GetComponent<EnemyController> ();
				go.transform.SetParent (GameObject.Find ("World").transform);
				go.transform.eulerAngles = transform.eulerAngles;

				goScript.moveDirection = waves [waveNumber].direction;
				goScript.rotateSpeed = waves [waveNumber].rotation; 


				yield return new WaitForSeconds (0.5f);
			}
			break;

		case WaveType.Boss:
			for (int i = 0; i < waves [waveNumber].number; i++) {

				GameObject go = (GameObject)Instantiate (bossPrefab, transform.position, Quaternion.identity);
				EnemyController goScript = go.GetComponent<EnemyController> ();
				go.transform.SetParent (GameObject.Find ("World").transform);
				go.transform.eulerAngles = transform.eulerAngles;

				goScript.moveDirection = waves [waveNumber].direction;
				goScript.rotateSpeed = waves [waveNumber].rotation; 


				yield return new WaitForSeconds (0.5f);
			}
			break;

		default:
			break;
		}
	}
}

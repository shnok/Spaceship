/* Script qui gère le fonctionnement général
 * de notre jeu. Que ce soit UI, état de la 
 * partie, choix des tourelles... */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum TurretType{
	None,
	Turret1,
	Turret2,
	Turret3
}

public class GameManager : MonoBehaviour {

	public List<Transform> spawners = new List<Transform> ();

	private Transform turret1Pivot;
	private Transform turret2Pivot;
	private Transform turret3Pivot;

	public static Transform turretCamera;
	public static Transform globalCamera;
	public static TurretType activeTurret = TurretType.None;

	public static bool firstStart = true;

	/* Stats*/
	public static int kills = 0;
	public static int misses = 0;
	public static int wave = 0;
	public static float loadValue = 0; 

	/* UI*/
	public static bool menu = true;
	public static bool zooming = false;
	public static TurretType mouseOverButton;


	public int numberOfWaves = 5; /* Nombre de vagues */
	public float wavesDelay = 10; /* Délai entre les vagues */

	void Start () 
	{
		turret1Pivot = GameObject.Find ("Turret1Pivot").transform;
		turret2Pivot = GameObject.Find ("Turret2Pivot").transform;
		turret3Pivot = GameObject.Find ("Turret3Pivot").transform;
		globalCamera = GameObject.Find ("OutCamera").transform;
		turretCamera = GameObject.Find ("TurretCamera").transform;

		SetVars ("Menu");
	}

	void Update()
	{
		ManageInputs ();

		if (menu) {
			if (!firstStart)
				Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}
		


	/* Modifie des variables en fonction de l'état du menu */
	private void SetVars(string type)
	{
		if (type == "Menu") {

			zooming = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			globalCamera.GetComponent<Camera> ().pixelRect = new Rect (0, 0, Screen.width, Screen.height);
			turretCamera.GetComponent<MouseOrbit> ().enabled = false;
			turretCamera.gameObject.SetActive (false);

		} else if (type == "FirstPerson") {

			mouseOverButton = TurretType.None;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			turretCamera.gameObject.SetActive (true);
			globalCamera.GetComponent<Camera> ().pixelRect = new Rect (0.75f * Screen.width, 0.7f * Screen.height, 0.25f * Screen.width, 0.3f * Screen.height);
			turretCamera.GetComponent<MouseOrbit> ().enabled = true;
		}
	}

	private void ManageInputs()
	{
		/* Gère l'ouverture/fermeture du menu */
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!menu) {
				menu = true;
				SetVars ("Menu");
			} else {
				if (!firstStart) {
					menu = false;
					SetVars ("FirstPerson");
				}
			}
		} 

		if (!zooming) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				ButtonPressed (1);
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				ButtonPressed (2);
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				ButtonPressed (3);
			}
		}
	}

	/* Choix des tourelles */
	public void ButtonPressed(int id)
	{
		menu = false;

		SetVars ("FirstPerson");

		if (firstStart) {
			StartCoroutine ("StartGame");
			firstStart = false;
		}

		if (id == 1) {
			activeTurret = TurretType.Turret1;
			turretCamera.GetComponent<MouseOrbit> ().pivot = turret1Pivot;
		} else if (id == 2) {
			activeTurret = TurretType.Turret2;
			turretCamera.GetComponent<MouseOrbit> ().pivot = turret2Pivot;
		} else if (id == 3) {
			activeTurret = TurretType.Turret3;
			turretCamera.GetComponent<MouseOrbit> ().pivot = turret3Pivot;
		}

	}

	/* Stocke dans une variable la tourelle qui correspond au bouton que l'on est en train de survoler */
	public void MouseOverHandler(int t)
	{
		if (t == 0)
			mouseOverButton = TurretType.None;
		else if (t == 1)
			mouseOverButton = TurretType.Turret1;
		else if (t == 2)
			mouseOverButton = TurretType.Turret2;
		else 
			mouseOverButton = TurretType.Turret3;
	}

	/* Gère l'apparition des vagues du jeu */
	IEnumerator StartGame() {
		wave = 0;
		kills = 0;
		misses = 0;

		GlobalUI.UI_Elements ["CountDown"].SetActive (true);
		int startDelay = 3;
		for (int i = 1; i < startDelay+1; i++){
			GlobalUI.UI_Elements ["CountDown"].GetComponent<Text> ().text = (startDelay + 1 - i).ToString ();
			Debug.Log (startDelay+1-i);
			yield return new WaitForSeconds (1);
		}

		GlobalUI.UI_Elements ["CountDown"].GetComponent<Text> ().text = "Start";
		StartCoroutine (GlobalUI.FadeOut(GlobalUI.UI_Elements ["CountDown"].GetComponent<Text> ()));

		for (int i = 1; i <= numberOfWaves; i++) {
			wave = i;
			GlobalUI.UI_Elements ["WaveValue"].SetActive (true);
			GlobalUI.UI_Elements ["WaveValue"].GetComponent<Text> ().text = "Wave " + i;
			StartCoroutine(GlobalUI.FadeOut(GlobalUI.UI_Elements ["WaveValue"].GetComponent<Text> ()));

			SpawnWave (i);

			yield return new WaitForSeconds (wavesDelay);
		}

		yield return new WaitForSeconds (30f);

		menu = true;
		SetVars ("Menu");
		firstStart = true;
	}

	private void SpawnWave(int nb) {
		foreach (Transform s in spawners) {
			if(s!=null)
				s.SendMessage("SpawnWave", nb-1);
		}
	}




}

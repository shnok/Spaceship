using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GlobalUI : MonoBehaviour {

	public static Dictionary<string,GameObject> UI_Elements = new Dictionary<string,GameObject>();

	void Start () 
	{
		LoadUI ();
	}

	void Update () 
	{
		ManageUI ();
	}

	/* Stocke tous les éléments de l'UI dans un dictionnaire */
	private void LoadUI()
	{
		UI_Elements.Add ("UI", GameObject.Find ("UI"));
		UI_Elements.Add ("Kills", GameObject.Find ("kills"));
		UI_Elements.Add ("Misses", GameObject.Find ("misses"));
		UI_Elements.Add ("Waves", GameObject.Find ("waves"));
		UI_Elements.Add ("TurretInfos_Container", GameObject.Find ("TurretInfos"));
		UI_Elements.Add ("TurretInfos_Turret1", GameObject.Find ("Turret1_Infos"));
		UI_Elements.Add ("TurretInfos_Turret2", GameObject.Find ("Turret2_Infos"));
		UI_Elements.Add ("TurretInfos_Turret3", GameObject.Find ("Turret3_Infos"));
		UI_Elements.Add ("Stats_Container", GameObject.Find ("Stats"));
		UI_Elements.Add ("Advice", GameObject.Find ("Infos"));
		UI_Elements.Add ("Pause", GameObject.Find ("Pause"));
		UI_Elements.Add ("Zoom", GameObject.Find ("Zoom"));
		UI_Elements.Add ("Cross", GameObject.Find ("Cross"));
		UI_Elements.Add ("Choice", GameObject.Find ("Choice"));
		UI_Elements.Add ("LoadBar", GameObject.Find ("LoadBar"));
		UI_Elements.Add ("CountDown", GameObject.Find ("CountDown"));
		UI_Elements.Add ("WaveValue", GameObject.Find ("WaveValue"));
		UI_Elements.Add ("BossBar", GameObject.Find ("BossBar"));
	}

	/* Gère les éléments de l'UI en fonction de l'état du menu ou de certains éléments du jeu */
	private void ManageUI()
	{

		if (GameManager.menu == true) {

			UI_Elements ["Zoom"].SetActive (false);
			UI_Elements ["Stats_Container"].SetActive (false);
			UI_Elements ["Advice"].SetActive (false);
			UI_Elements ["TurretInfos_Container"].SetActive (true);

			UI_Elements ["Cross"].SetActive (false);
			UI_Elements ["Choice"].SetActive (true);
			UI_Elements ["LoadBar"].SetActive (false);
			UI_Elements ["CountDown"].SetActive (false);
			UI_Elements ["WaveValue"].SetActive (false);
			UI_Elements ["BossBar"].SetActive (false);

			if (!GameManager.firstStart) {
				UI_Elements ["Pause"].SetActive (true);
			} else {
				UI_Elements ["Pause"].SetActive (false);
			}

			switch (GameManager.mouseOverButton) {
			case TurretType.Turret1:
				UI_Elements ["TurretInfos_Turret1"].SetActive (true);
				UI_Elements ["TurretInfos_Turret2"].SetActive (false);
				UI_Elements ["TurretInfos_Turret3"].SetActive (false);
				break;
			case TurretType.Turret2:
				UI_Elements ["TurretInfos_Turret1"].SetActive (false);
				UI_Elements ["TurretInfos_Turret2"].SetActive (true);
				UI_Elements ["TurretInfos_Turret3"].SetActive (false);
				break;
			case TurretType.Turret3:
				UI_Elements ["TurretInfos_Turret1"].SetActive (false);
				UI_Elements ["TurretInfos_Turret2"].SetActive (false);
				UI_Elements ["TurretInfos_Turret3"].SetActive (true);
				break;
			default:
				UI_Elements ["TurretInfos_Container"].SetActive (false);
				break;
			}

		} else {

			UI_Elements ["TurretInfos_Container"].SetActive (false);
			UI_Elements ["Stats_Container"].SetActive (true);

			UI_Elements ["Pause"].SetActive (false);
			UI_Elements ["Choice"].SetActive (false);

			if (!GameManager.zooming) {
				UI_Elements ["Cross"].SetActive (true);
				UI_Elements ["Zoom"].SetActive (false);
				UI_Elements ["Advice"].SetActive (true);
			} else { 
				UI_Elements ["Cross"].SetActive (false);
				UI_Elements ["Zoom"].SetActive (true);
				UI_Elements ["Advice"].SetActive (false);
			}

			if (GameManager.loadValue != 0) {
				UI_Elements ["LoadBar"].SetActive (true);
				UI_Elements ["LoadBar"].GetComponent<Image> ().fillAmount = GameManager.loadValue;
			} else {
				UI_Elements ["LoadBar"].SetActive (false);
			}

			UI_Elements ["Kills"].GetComponent<Text>().text = string.Concat ("Destroyed ships: ", GameManager.kills.ToString()); 
			UI_Elements ["Misses"].GetComponent<Text>().text = string.Concat ("Missed ships: ", GameManager.misses.ToString()); 
			UI_Elements ["Waves"].GetComponent<Text>().text = string.Concat ("Waves: ", GameManager.wave.ToString(), "/5"); 

		}

	}

	public static IEnumerator FadeOut(Text t)
	{
		Color tColor = t.color;
		tColor.a = 1;
		t.color = tColor;

		yield return new WaitForSeconds (1);

		while(t.color.a > 0){
			tColor.a -= Time.deltaTime * 0.75f;
			t.color = tColor;
			yield return null;
		}
		t.gameObject.SetActive (false);
	}
}

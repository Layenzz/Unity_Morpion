using UnityEngine;
using System.Collections;

public class RotationMenu : MonoBehaviour {

	// Use this for initialization
	float x;
	float y;
	float z;
	public GUISkin guiskin;
	bool aff;

	void Start () 
	{
		aff = false;
		x = y = z = 0;
	}

	void OnGUI()
	{
		GUI.skin = guiskin;
		if (aff == false)
		{
			if (GUI.Button(new Rect(Screen.width/2.8f, Screen.height/1.27f, Screen.width/9, Screen.height/9), "Jouer"))
			{
				Application.LoadLevel("The Game");
			}

			if (GUI.Button(new Rect(Screen.width/1.8f, Screen.height/1.27f, Screen.width/9, Screen.height/9), "A propos"))
			{
				aff = true;
			}
		}
		if (aff == true)
		{
			string msg = "\n\nConception/Développement : Marie Baudy\n\n\n" +
				"Temps de travail :\n\n" +
				" - une semaine pour les bases du projet (modélisation 3D, configuration des grilles, script de pose d'un symbole\n    et condition de victoire, stockage de plusieurs valeurs dont j'aurais besoin à l'affichage)\n\n" +
				" - une semaine pour les évènements aléatoires (environ un à deux jours de code + tests par évènement)\n\n" +
				" - trois jours pour l'affichage et les finitions (gui, création d'un script pour gérer des chronomètres, debug et\n    optimisation)\n\n\n" +
				"Problèmes rencontrés :\n\n" +
					" - Tentative d'optimisation du code : perte de la configuration des grilles, 2 soirées pour tout régler\n\n" +
					" - Un évènement n'a pas pu être ajouté : le mélange de symboles. Il donnait automatiquement la victoire à un joueur\n    s'il y avait trop de symboles sur la grille, ou alors était inintéressant s'il n'y en avait pas assez.\n    Cet évènement deviendrait amusant sur un morpion 5x5 cases avec 3 couleurs\n\n" +
					" - Un évènement fut supprimé : la \"pitiée pour un joueur\". Cet évènement posait un symbole contraire quand 2\n    symboles identiques étaient alignés afin de contrer une possible victoire" +
					"\n\n\nTravail commencé en juin 2013, terminé en fevrier 2014";
				
					GUI.Label(new Rect(5,0, 800, 600), msg, guiskin.customStyles[1]);
		}

		if (aff == true)
			if (GUI.Button(new Rect(Screen.width/1.8f, Screen.height/1.27f, Screen.width/9, Screen.height/9), "Retour"))
		{
			aff = false;
		}

	}

	// Update is called once per frame
	void Update () 
	{
		Vector3 rotation = new Vector3(x, y, z);
		this.gameObject.transform.eulerAngles = rotation;
		x++;
		y++;
		z++;
	}
}

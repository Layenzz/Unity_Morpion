using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIGame : MonoBehaviour {

	private Cube cube;
	private Core core;
	private Case current_cell;
	private SelectObject selectobject;
	public bool move;
	private float progress;
	private Case[] tmp_case;
	private Vector3 tmp_rotation;
	private Vector3 tmp_rotation_game;
	public GameObject Game;
	public bool turn_game;
	public Rules rules;
	public Event ev;
	public GUISkin guiskin;

	void Start () 
	{
		cube = Cube.cube_instance;
		core = Core.instance;
		selectobject = SelectObject.instance;
		ev = Event.EvInstance;
		move = false;
		turn_game = false;
		progress = 0;
		rules = Rules.rules_instance;
	}

    void OnGUI()
    { 
		GUI.skin = guiskin;
		string your_turn = "A vous de jouer";

		if (core.gui_at_left == true)
			GUI.Label(new Rect(60,200,500,500), your_turn);
		else
			GUI.Label(new Rect(650,200,500,500), your_turn);

		GUI.Label(new Rect(Screen.width / 2.18f,110,500,500), "Tour :   " + core.step.ToString());

		GUI.Label(new Rect(80,240,500,500), core.player_list[0].Pseudo);
		GUI.Label(new Rect(670,240,500,500), core.player_list[1].Pseudo);

		GUI.Label(new Rect(100, 375,500,500), core.player_list[0].Score.ToString(), guiskin.customStyles[0]);
		GUI.Label(new Rect(690,375,500,500), core.player_list[1].Score.ToString(), guiskin.customStyles[0]);

		if (ev.change_player == true)
		{
			string msg = "Inversion des symboles des joueurs";
			GUI.Label(new Rect((Screen.width-msg.Length) / 2.9f,70,500,500), msg);
		}

		if (ev.init == true)
		{
			string msg = "Réinitialisation d'une grille";
			GUI.Label(new Rect((Screen.width-msg.Length) / 2.5f,70,500,500), msg);
		}

		if (ev.turn_game == true)
		{
			string msg = "Changement de grille";
			GUI.Label(new Rect((Screen.width-msg.Length) / 2.4f,70,500,500), msg);
		}

		if (ev.erase == true)
		{
			string msg = "Suppression d'un symbole";
			GUI.Label(new Rect((Screen.width-msg.Length) / 2.4f,70,500,500), msg);
		}


		if (selectobject.selectObject == null)
			GUI.enabled = false;

		if (GUI.Button(new Rect(Screen.width/2.33f, Screen.height/1.27f, Screen.width/7, Screen.height/7), "Confirmer"))
		{
			if (!move && (selectobject.selectObject != null || current_cell == null))
			{
				if (current_cell == null || (core.current_player == 1 && selectobject.selectObject.transform.eulerAngles != current_cell.Position[1]) || (core.current_player == 2 && selectobject.selectObject.transform.eulerAngles != current_cell.Position[2]))
				{
					tmp_case = selectobject.selectObject.GetComponents<Case>();
					foreach (Case cell in tmp_case)
					{
						if (cell.GridId == core.current_grid)
						{
							move = true;
							tmp_rotation = selectobject.selectObject.transform.localEulerAngles;
							current_cell = cell;
							break;
						}
					}
					if (current_cell == null)
					{
				//		Debug.Log("Cell grid " + core.current_grid + " not found on block " + selectobject.selectObject.name);
					}
				}
			}
		}

		if (move)
			GUI.enabled = false;
		else
			GUI.enabled = true;
		
			
/*		if (GUI.Button(new Rect(Screen.width/1.5f, Screen.height/5, Screen.width/7, Screen.height/7), "Next"))
		{
			if (!turn_game) 
			{
				core.index_grid_rotation++;
				if (core.index_grid_rotation > 5)
					core.index_grid_rotation = 0;
				turn_game = true;
				tmp_rotation_game = Game.transform.eulerAngles;
			}
		}
		*/
	}

	void Update () 
	{
		if (!move && turn_game)
		{
			Game.transform.eulerAngles = Vector3.Lerp(tmp_rotation_game, core.grid_rotation[core.index_grid_rotation], progress);
			if (progress >= 1)
			{
				Debug.Log("Grid number : " + cube.grid[core.index_grid_rotation].id);
				progress = 0;
				current_cell = null;
				core.current_grid = cube.grid[core.index_grid_rotation].id;
				turn_game = false;
				Game.transform.eulerAngles = core.grid_rotation[core.index_grid_rotation];
			}
			else
				progress += Time.deltaTime;			
		}
		if (move && !turn_game)
		{
		//	Debug.Log(current_cell.Position[1]);
			selectobject.selectObject.transform.localEulerAngles = Vector3.Lerp(tmp_rotation, core.current_player == 1 ? current_cell.Position[1] : current_cell.Position[2], progress);
			if (progress >= 1)
			{	
				progress = 0;
				move = false;
				selectobject.selectObject.transform.localEulerAngles = core.current_player == 1 ? current_cell.Position[1] : current_cell.Position[2];

				if (core.current_player == 1)
					current_cell.Status = Case.Player.PLAYER1;
				else
					current_cell.Status = Case.Player.PLAYER2;

				current_cell = null;
				cube.grid[core.current_grid].recap[cube.grid[core.current_grid].cube.IndexOf(selectobject.selectObject)] = core.current_player;
				selectobject.selectObject.renderer.materials = selectobject.save_material;
				selectobject.selectObject = null;
				
				if (rules.check_victory() == false)
					ev.RandomEvent();
				if (core.gui_at_left == false)
					core.gui_at_left = true;
				else
					core.gui_at_left = false;

				core.step++;
				core.current_player = core.current_player == 1 ? 2 : 1;
//				ev.RandomEvent();
			}
			else
				progress += Time.deltaTime;
		}
	}
}

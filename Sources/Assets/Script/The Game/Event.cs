using UnityEngine;
using System.Collections.Generic;

public class Event : MonoBehaviour 
{
	Cube cube;
	Core core;
	Case current_cell;
	SelectObject selectobject;
//	private Rules rules;
	float progress;
	Vector3 tmp_rotation;
	public static Event EvInstance;
	public int[] save_recap;
	Vector3 tmp_rotation_game;
	public List<Invalid> invalid;
	public bool init;
	public bool change_player;
	Timer invalid_obj_timer;
	#region Pity

	public bool pity;
	GameObject tmp;
	int ennemy;
	int index;

	#endregion

	#region Shake
	
	int number_cross;
	int number_circle;
	
	#endregion
	
	#region Erase
	public bool erase;
	#endregion
	
	#region turn_board
	public bool turn_game;
	#endregion

	private int grid_id;

	void Awake()
	{
		EvInstance = this;
	}
	
	void Start () 
	{
		cube = Cube.cube_instance;
		core = Core.instance;
		selectobject = SelectObject.instance;
		pity = false;
		init = false;
		change_player = false;
		tmp = null;
		ennemy = 0;
		index = -1;
		grid_id = -1;
		progress = 0;
		invalid = new List<Invalid>();
		invalid_obj_timer = null;
	}
	
	public void RandomEvent()
	{
		int i = Random.Range(0, 7);

		save_recap = cube.grid[core.current_grid].recap;
		int count = 0;

		foreach (int nb in save_recap)
		{
			if (nb == 0)
				count++;
		}
		if (count == 0)
		{
//			reinitialisation(core.current_grid);
			TurnBoard();
		}
		else
		{
/*			if (i == 0)
			{
				Debug.Log("pity");
				ennemy = core.player_list[core.current_player == 1 ? 1 : 0].PlayerNb;
				PityForPlayer();
			}
*/			if (i == 0)
			{
				Debug.Log("Erase");
				foreach(int x in save_recap)
				{
					if (x > 0)
					{
						EraseSymbole();
						return;
					}
				}
				RandomEvent();
			}
			if (i == 1)
			{
				Debug.Log("Turn board");
				TurnBoard();
			}
			if (i == 2)
			{
				Debug.Log("Invalid cube");
				invalid_cube();
			}
			if (i == 3)
			{
				int id = Random.Range(0, 6);
				save_recap = cube.grid[id].recap;
				foreach(int x in save_recap)
				{
					if (x > 0)
					{
						reinitialisation(id);
						Debug.Log("Reinitialisation grid " + id);
						return;
					}
				}
				RandomEvent();
			}
			if (i == 4)
			{
				Debug.Log("Change player");
				ChangePlayer();
			}
			if (i > 4)
				Debug.Log("Do nothing");
			//		if (i == 1)
			//			shake_position();
		}
	}

	public void reinitialisation(int GridId)
	{
		init = true;

		grid_id = GridId;

		foreach (GameObject obj in cube.grid[grid_id].cube)
			change_number_in_recap(obj, 0);
	}

	void change_material(GameObject test_invalid)
	{
		Material[] mat = new Material[test_invalid.renderer.materials.Length + 1];
		
		int x = -1;
		while (++x < test_invalid.renderer.materials.Length)
			mat[x] = test_invalid.renderer.materials[x];
		
		mat[x] = Resources.Load("Invalid", typeof(Material)) as Material;
		test_invalid.renderer.materials = mat;
	}

	void change_number_in_recap(GameObject test_invalid, int number)
	{
		Case[] tmp_case = test_invalid.GetComponents<Case>();
		foreach (Case script_case in tmp_case)
		{
			int tmp = 0;
			foreach (GameObject obj in cube.grid[script_case.GridId].cube)
			{
				if (obj == test_invalid)
				{
					cube.grid[script_case.GridId].recap[tmp] = number;
					break;
				}
				else
					tmp++;
			}
		}
	}

	public void invalid_cube()
	{
		int i = 0;
		int size = 0;
		bool condition = false;
		GameObject test_invalid = null;
		while (condition != true)
		{
			i = Random.Range(0, 9);
			test_invalid = cube.grid[core.current_grid].cube[i];
			size = 0;
			if (invalid.Count == 0)
				condition = true;
			else
			{
				foreach(Invalid obj in invalid)
				{
					if (test_invalid != obj.invalid)
						size++;
				}
				if (size == invalid.Count)
					condition = true;
			}
		}

		string msg = "Disparition d'un cube. Il reviendra dans 6 tours";
		invalid_obj_timer = Chrono.Chrono_instance.Create_new_timer(true, msg, Time.time,3,new Rect((Screen.width-msg.Length) / 3.2f,70,500,500),2);

		change_material(test_invalid);
		change_number_in_recap(test_invalid, 9);
		test_invalid.transform.eulerAngles = new Vector3(0,0,0);
		Invalid objt = new Invalid();
		objt.invalid = test_invalid;
		objt.step = core.step;
		invalid.Add(objt);
	}

	public void ChangePlayer()
	{
		core.current_player = core.current_player == 1 ? 2 : 1;
		foreach(Gamer player in core.player_list)
		{
			player.PlayerNb = player.PlayerNb == 1 ? 2 : 1;
		}
		change_player = true;
		Debug.Log("Players change symbol");
	}

	public void TurnBoard()
	{
		core.index_grid_rotation++;
		if (core.index_grid_rotation > 5)
			core.index_grid_rotation = 0;
		turn_game = true;
		tmp_rotation_game = core.Game.transform.eulerAngles;
	}
	
	#region Erase_function
	public void EraseSymbole()
	{
		save_recap = cube.grid[core.current_grid].recap;
		int i = Random.Range(0, 8);
		
		while (save_recap[i] != 1 && save_recap[i] != 2)
			i = Random.Range(0, 9);

		to_delete();
		save_recap[i] = 0;
		erase = true;
		selectobject.selectObject = cube.grid[core.current_grid].cube[i];
		tmp_rotation = selectobject.selectObject.transform.localEulerAngles;
		to_delete();
	}
	#endregion
				
	#region Shake_function
//	public void shake_position()
//	{
//		number_cross = 0;
//		number_circle = 0;
//		
//		save_recap = cube.grid[core.current_grid].recap;
//		int i = 0;
//		int tmp_player;
//		
//		while (i <= 8)
//		{
//			if (save_recap[i] == 1)
//				number_cross++;
//			if (save_recap[i] == 2)
//				number_circle++;
//			save_recap[i] = 0;
//			i++;
//		}
//		
//		Debug.Log("nombre de croix au nouveau tour : " + number_cross);
//		Debug.Log("nombre de cercle au nouveau tour : " + number_circle);
//
//		i = 0;
//		
//		if (number_cross > 0 || number_circle > 0)
//		{
//			while (i <= 8)
//			{
//				tmp_player = Random.Range(0, 2);
//
//				while (tmp_player == 1 && number_cross <= 0 || tmp_player == 2 && number_circle <= 0)
//				{
//					Debug.Log("Boucle de random car tmp_player = " + tmp_player);
//					tmp_player = Random.Range(0, 2);
//				}
//
//				Debug.Log("tmp_player final = " + tmp_player);
//				
//				if (tmp_player == 1 && number_cross > 0)
//				{
//					if (i == 0)
//					{
//						save_recap[i] = 1;
//						Debug.Log("j'ai pose une croix car i == 0");
//						number_cross--;
//					}
//					else
//					{
//						
//						save_recap[i] = 1;
//						Debug.Log("Croix posee en " + i);
//						number_cross--;
//						ennemy = tmp_player;
//
//						if (check_for_pity() == true)
//						{
//							Debug.Log("Il reste " + number_circle + " cercle a placer");
//
//							if (tmp_player == 1 && save_recap[index] == 0 && number_circle > 0)
//							{
//								save_recap[index] = 2;
//								//Debug.Log("==>" + save_recap[i]);
//								Debug.Log("cercle pose pour contrer en : " + index);
//								number_circle--;
//							}
//						}
//					}
//				}
//				i++;
//
//				while (save_recap[i] != 0)
//				{
//					i++;
//					if (i > 8)
//						return;
//				}
//
//			}
//			to_delete();
//		}
//	}
	#endregion

	void to_delete()
	{
//		Debug.Log("0 = " + save_recap[0] +  ", 1 = " + save_recap[1] + ", 2 = " + save_recap[2] + ", 3 = " + save_recap[3] + ", 4 = " + save_recap[4]+ ", 5 = " + save_recap[5]+ ", 6 = " + save_recap[6]+ ", 7 = " + save_recap[7]+ ", 8 = " + save_recap[8]);
	}
	
	#region Pity_function
	public void take_cell()
	{
		if (tmp != null && save_recap[index] == 0)
		{
			Case[] tmp_case = tmp.GetComponents<Case>();

			foreach (Case cell in tmp_case)
			{
				if (cell.GridId == core.current_grid)
				{
					tmp_rotation = tmp.transform.localEulerAngles;
					pity = true;
					current_cell = cell;
					break;
				}
			}
			if (current_cell == null)
			{
				Debug.Log("Cell grid " + core.current_grid + " not found on block " + selectobject.selectObject.name);
			}
		}
	}
	
	public bool check_for_pity()
	{
		if (save_recap[0] == ennemy && save_recap[1] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[2];
			index = 2;
			Debug.Log("1");
			if (save_recap[2] == 0)
				return true;
		}
		
		if (save_recap[2] == ennemy && save_recap[1] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[0];
			index = 0;
			Debug.Log("2");
			if (save_recap[0] == 0)
			return true;
		}

		if (save_recap[0] == ennemy && save_recap[2] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[1];
			index = 1;
			Debug.Log("3");
			if (save_recap[1] == 0)
				return true;
		}
		
		if (save_recap[3] == ennemy && save_recap[4] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[5];
			index = 5;
			Debug.Log("4");
			if (save_recap[5] == 0)	
				return true;
		}

		if (save_recap[3] == ennemy && save_recap[5] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[4];
			index = 4;
			Debug.Log("5");
			if (save_recap[4] == 0)
			return true;
		}

		if (save_recap[4] == ennemy && save_recap[5] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[3];
			index = 3;
			Debug.Log("6");
			if (save_recap[3] == 0)
				return true;
		}
		
		if (save_recap[6] == ennemy && save_recap[7] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[8];
			index = 8;
			Debug.Log("7");
			if (save_recap[8] == 0)
				return true;
		}

		if (save_recap[6] == ennemy && save_recap[8] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[7];
			index = 7;
			Debug.Log("8");
			if (save_recap[7] == 0)
				return true;
		}
		
		if (save_recap[7] == ennemy && save_recap[8] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[6];
			index = 6;
			Debug.Log("9");
			if (save_recap[6] == 0)
			return true;
		}
		
		if (save_recap[2] == ennemy && save_recap[5] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[8];
			index = 8;
			Debug.Log("10");
			if (save_recap[8] == 0)

			return true;
		}
		
		if (save_recap[2] == ennemy && save_recap[8] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[5];
			index = 5;
			Debug.Log("11");
			if (save_recap[5] == 0)

			return true;
		}
		
		if (save_recap[5] == ennemy && save_recap[8] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[2];
			index = 2;
			Debug.Log("12");
			if (save_recap[2] == 0)

			return true;
		}
		
		if (save_recap[1] == ennemy && save_recap[4] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[7];
			index = 7;
			Debug.Log("13");
			if (save_recap[7] == 0)

			return true;
		}
		
		if (save_recap[1] == ennemy && save_recap[7] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[4];
			index = 4;
			Debug.Log("14");
			if (save_recap[4] == 0)

			return true;
		}
		
		if (save_recap[4] == ennemy && save_recap[7] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[1];
			index = 1;
			Debug.Log("15");
			if (save_recap[1] == 0)

			return true;
		}
		
		if (save_recap[0] == ennemy && save_recap[3] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[6];
			index = 6;
			Debug.Log("16");
			if (save_recap[6] == 0)

			return true;
		}
		
		if (save_recap[0] == ennemy && save_recap[6] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[3];
			index = 3;
			Debug.Log("17");
			if (save_recap[3] == 0)

			return true;
		}
		
		if (save_recap[3] == ennemy && save_recap[6] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[0];
			index = 0;
			Debug.Log("18");
			if (save_recap[0] == 0)

			return true;
		}
		
		if (save_recap[8] == ennemy && save_recap[4] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[0];
			index = 0;
			Debug.Log("19");
			if (save_recap[0] == 0)

			return true;
		}
		
		if (save_recap[0] == ennemy && save_recap[8] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[4];
			index = 4;
			Debug.Log("20");
			if (save_recap[4] == 0)

			return true;
		}
		
		if (save_recap[0] == ennemy && save_recap[4] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[8];
			index = 8;
			Debug.Log("21");
			if (save_recap[8] == 0)

			return true;
		}
		
		if (save_recap[2] == ennemy && save_recap[4] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[6];
			index = 6;
			Debug.Log("22");
			if (save_recap[6] == 0)

			return true;
		}
		
		if (save_recap[2] == ennemy && save_recap[6] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[4];
			index = 4;
			Debug.Log("23");
			if (save_recap[4] == 0)

			return true;
		}
		
		if (save_recap[4] == ennemy && save_recap[6] == ennemy)
		{
			tmp = cube.grid[core.current_grid].cube[2];
			index = 2;
			Debug.Log("24");
			if (save_recap[2] == 0)

			return true;
		}

		return false;
	}
	
	public void PityForPlayer()
	{
		save_recap = cube.grid[core.current_grid].recap;
		ennemy = core.current_player == 1 ? 2 : 1;
		if (check_for_pity ())
			take_cell ();
		else
			RandomEvent();
	}
	#endregion 
	
	void Update () 
	{
		if (invalid_obj_timer != null && invalid_obj_timer.active == false)
		{
			Chrono.Chrono_instance.Remove_timer(invalid_obj_timer);
			invalid_obj_timer = null;
		}
		if (pity) 
		{
			tmp.transform.localEulerAngles = Vector3.Lerp (tmp_rotation, current_cell.Position [core.current_player], progress);
			if (progress >= 1) {	
				progress = 0;
				pity = false;
				tmp.transform.localEulerAngles = current_cell.Position [core.current_player];
				
				if (core.current_player == 1)
					current_cell.Status = Case.Player.PLAYER1;
				else
					current_cell.Status = Case.Player.PLAYER2;
				
				current_cell = null;
				tmp = null;
				cube.grid [core.current_grid].recap [index] = core.current_player;
				index = -1;
				//Debug.Log ("Pitie pour le joueur " + core.current_player);
			} else
				progress += Time.deltaTime;
		}
		if (erase)
		 {
			selectobject.selectObject.transform.localEulerAngles = Vector3.Lerp (tmp_rotation, new Vector3 (0, 0, 0), progress);
			if (progress >= 1)
			{	
				progress = 0;
				erase = false;
				selectobject.selectObject.transform.localEulerAngles = new Vector3 (0, 0, 0);

				Case[] tmp_case = selectobject.selectObject.GetComponents<Case> ();

				for (int i = 0; i < tmp_case.Length; i++) 
				{
					Case cell = tmp_case [i];
					if (cell.GridId == core.current_grid)
					{
						current_cell = cell;
						break;
					}
				}
				if (current_cell == null)
				{
				//	Debug.Log ("Cell grid " + core.current_grid + " not found on block " + selectobject.selectObject.name);
				}

				current_cell.Status = Case.Player.NONE;
				current_cell = null;
				cube.grid [core.current_grid].recap [cube.grid [core.current_grid].cube.IndexOf (selectobject.selectObject)] = 0;
				selectobject.selectObject = null;

			} else
				progress += Time.deltaTime;
		}
		if (turn_game)
		{
			core.Game.transform.eulerAngles = Vector3.Lerp(tmp_rotation_game, core.grid_rotation[core.index_grid_rotation], progress);
			if (progress >= 1)
			{
				Debug.Log("Grid number : " + cube.grid[core.index_grid_rotation].id);
				progress = 0;
				current_cell = null;
				core.current_grid = cube.grid[core.index_grid_rotation].id;
				turn_game = false;
				core.Game.transform.eulerAngles = core.grid_rotation[core.index_grid_rotation];
			}
			else
				progress += Time.deltaTime;		
		}

		if (init)
		{
			foreach (GameObject obj in cube.grid[grid_id].cube)
			{
				obj.transform.localEulerAngles = Vector3.Lerp (obj.transform.localEulerAngles, new Vector3 (0, 0, 0), progress);
			}

			if (progress >= 1)
			{
				foreach (GameObject obj in cube.grid[grid_id].cube)
					obj.transform.localEulerAngles = new Vector3 (0, 0, 0);

				if (invalid.Count > 0)
				{
					Debug.Log("count invalid list = : " + invalid.Count);
					foreach(Invalid obj in invalid)
					{
						Case[] tmp_case = obj.invalid.GetComponents<Case>();
						foreach (Case script_case in tmp_case)
						{
							if (grid_id == script_case.GridId)
							{
								change_number_in_recap(obj.invalid, 0);
								Material[] mat = new Material[obj.invalid.renderer.materials.Length - 1];

								int x = -1;
								while (++x < obj.invalid.renderer.materials.Length - 1)
									mat[x] = obj.invalid.renderer.materials[x];

								obj.invalid.renderer.materials = mat;

								invalid.Remove(obj);
							}
						}
						if (invalid.Count == 0)
							break;
					}
				}
				progress = 0;
				init = false;
			}
			else
				progress += Time.deltaTime;	
		}

		if (change_player)
		{
			foreach(Gamer player in core.player_list)
			{
				player.player_cube.transform.localEulerAngles = Vector3.Lerp (player.player_cube.transform.localEulerAngles, player.PlayerNb == 1 ? player.rotation_two : player.rotation_one, progress);
			}
			progress += Time.deltaTime;
			if (progress >= 1)
			{
				foreach(Gamer player in core.player_list)
				{
					player.player_cube.transform.localEulerAngles = player.PlayerNb == 1 ? player.rotation_two : player.rotation_one;
				}
				progress = 0;
				change_player = false;
			}
		}

		if (invalid.Count > 0)
		{
			foreach(Invalid obj in invalid)
			{
				if (core.step >= obj.step + 6)
				{
					change_number_in_recap(obj.invalid, 0);
					Material[] mat = new Material[obj.invalid.renderer.materials.Length - 1];
					
					int x = -1;
					while (++x < obj.invalid.renderer.materials.Length - 1)
						mat[x] = obj.invalid.renderer.materials[x];
					
					obj.invalid.renderer.materials = mat;
					obj.invalid.transform.localEulerAngles = new Vector3(0,0,0);
					invalid.Remove(obj);
					break;
				}
			}
		}
	}
	
	public static Event Ev
	{
		get{return EvInstance;}
	}
}

public class Invalid
{
	public GameObject invalid;
	public int step;
}
using UnityEngine;
using System.Collections;

public class Rules : MonoBehaviour {

	private Core core;
	private Cube cube;
	public static Rules rules_instance;
	private Event ev;
	private Chrono _chrono;
	private Timer time;
	private Timer player_win_timer;

	void Awake()
	{
		rules_instance = this;
	}	
	
	void Start () 
	{
		cube = Cube.cube_instance;
		core = Core.instance;
		ev = Event.EvInstance;
		chrono = Chrono.Chrono_instance;
		time = null;
		player_win_timer = null;
	}

	public bool check_victory()
	{
		if (rules.check() == true)
		{
//			Debug.Log("PLAYER " + core.player_list[0].PlayerNb == core.current_player ? core.player_list[0].Pseudo : core.player_list[1].Pseudo + " WIN !!!");
			string msg = "";
			foreach(Gamer player in core.player_list)
			{
				if (core.current_player == player.PlayerNb)
				{
					msg = player.Pseudo + " win !";
					player.Score++;
				}
			}
			string msg2 = "New grid in : ";
			player_win_timer = chrono.Create_new_timer(true, msg, Time.time, 15.0f, new Rect((Screen.width-msg.Length) / 2.25f,30,500,500), 2);
			time = chrono.Create_new_timer(true, msg2, Time.time, 15.0f, new Rect((Screen.width-msg2.Length) / 2.25f,70,500,500), 1);
			return true;
		}
		return false;
	}

	public bool check()
	{
		if (check_middle() == true || check_bottom_right() == true || check_top_left() == true || check_diagonal() == true)
			return true;
		return false;
	}
	
	public bool check_middle()
	{
		if (cube.grid[core.current_grid].recap[4] == core.current_player)
		{
			Debug.Log("4 7 1");
			if (cube.grid[core.current_grid].recap[7] == core.current_player)
				if (cube.grid[core.current_grid].recap[1] == core.current_player)
					return true;
			
			Debug.Log("4 5 3");
					
			if (cube.grid[core.current_grid].recap[5] == core.current_player)
				if (cube.grid[core.current_grid].recap[3] == core.current_player)
					return true;
		}
		return false;
	}

	public bool check_diagonal()
	{
		if (cube.grid[core.current_grid].recap[8] == core.current_player)
		{
			Debug.Log("8 4 0");
			
			if (cube.grid[core.current_grid].recap[4] == core.current_player)
				if (cube.grid[core.current_grid].recap[0] == core.current_player)
					return true;
		}
		if (cube.grid[core.current_grid].recap[6] == core.current_player)
		{
			Debug.Log("6 4 2");
			
			if (cube.grid[core.current_grid].recap[4] == core.current_player)
				if (cube.grid[core.current_grid].recap[2] == core.current_player)
					return true;
		}
		return false;
	}
	public bool check_top_left()
	{
		if (cube.grid[core.current_grid].recap[8] == core.current_player)
		{
			Debug.Log("8 7 6");

			if (cube.grid[core.current_grid].recap[7] == core.current_player)
				if (cube.grid[core.current_grid].recap[6] == core.current_player)
					return true;
			
			Debug.Log("8 5 2");

			if (cube.grid[core.current_grid].recap[5] == core.current_player)
				if (cube.grid[core.current_grid].recap[2] == core.current_player)
					return true;
		}
		return false;
	}
	
	public bool check_bottom_right()
	{
		if (cube.grid[core.current_grid].recap[0] == core.current_player)
		{
			Debug.Log("0 4 8");

			if (cube.grid[core.current_grid].recap[4] == core.current_player)
				if (cube.grid[core.current_grid].recap[8] == core.current_player)
					return true;
			
			Debug.Log("0 3 6");

			if (cube.grid[core.current_grid].recap[3] == core.current_player)
				if (cube.grid[core.current_grid].recap[6] == core.current_player)
					return true;
			
			Debug.Log("0 2 1");

			if (cube.grid[core.current_grid].recap[1] == core.current_player)
				if (cube.grid[core.current_grid].recap[2] == core.current_player)
					return true;
		}
		return false;
	}

	public Chrono chrono
	{
		get{return _chrono;}
		set{_chrono = value;}
	}

	void Update()
	{
		if (time != null && time.active == false && player_win_timer != null && player_win_timer.active == false)
		{
			ev.reinitialisation(core.current_grid);
			chrono.Remove_timer(player_win_timer);
			chrono.Remove_timer(time);
			time = null;
			player_win_timer = null;
			core.step = 0;
		}
	}

	public static Rules rules
	{
		get {return rules_instance;}
	}
}
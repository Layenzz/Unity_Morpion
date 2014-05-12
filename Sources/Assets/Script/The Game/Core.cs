using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour {

	private static Core core_instance;
	public int i_current_player;
	private int i_current_grid;
	public GameObject Game;

	public int score_player_1;	
	public int score_player_2;	
	
	public Vector3[] grid_rotation;
	public int index_grid_rotation;
	public int step;

	public List<Gamer> player_list;

	public bool gui_at_left;

	void Awake()
	{
		core_instance = this;
	}
	
	void Start () 
	{
		i_current_player = 1;
		i_current_grid = 0;
		score_player_1 = score_player_2 = index_grid_rotation = step = 0;
		Gamer player1 = new Gamer();
		player1.Score = 0;
		player1.Pseudo = "Player 1";
		player1.PlayerNb = 1;
		player1.rotation_one = new Vector3(0,270,0);
		player1.rotation_two = new Vector3(0,180,90);
		player1.player_cube = GameObject.Find("Player 1 Cube");
		Gamer player2 = new Gamer();
		player2.Pseudo = "Player 2";
		player2.Score = 0;
		player2.PlayerNb = 2;
		player2.rotation_one = new Vector3(0,90,180);
		player2.rotation_two = new Vector3(0,180,90);
		player2.player_cube = GameObject.Find("Player 2 Cube");
		player_list = new List<Gamer>();
		player_list.Add(player1);
		player_list.Add(player2);
		gui_at_left = true;
	}
	
	void Update () 
	{
	}
	
	public static Core instance
	{
		get{return core_instance;}
	}
	
	public int current_player
	{
		get{return i_current_player;}
		set{i_current_player = value;}
	}
	
	public int current_grid
	{
		get {return i_current_grid;}
		set {i_current_grid = value;}
	}
}

public class Gamer 
{	
	// Use this for initialization
	private int score;
	private int player_number;
	private string pseudo;
	public GameObject player_cube;
	private Vector3 _rotation_one;
	private Vector3 _rotation_two;

	public int Score
	{
		get{return score;}
		set{score = value;}
	}
	
	public int PlayerNb
	{
		get{return player_number;}
		set{ player_number = value;}
	}
	
	public string Pseudo
	{
		get{return pseudo;}
		set{pseudo = value;}
	}

	public Vector3 rotation_one
	{
		get{return _rotation_one;}
		set{_rotation_one = value;}
	}

	public Vector3 rotation_two
	{
		get{return _rotation_two;}
		set{_rotation_two = value;}
	}
};
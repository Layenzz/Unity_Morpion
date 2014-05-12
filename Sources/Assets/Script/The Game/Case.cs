using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour {

    public enum Player
    { 
        NONE = 0,
        PLAYER1 = 1,
        PLAYER2 = 2,
    }
	
	public int GridId;
	
	public Player player;
	//public int i_status;

	public List<Vector3> Position;
	public List<Player> CaseStatus = new List<Player>();

	void Start () 
	{
		player = Player.NONE;
		//i_status = 0;
 	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Player Status
	{
		get{return player;}
		set{player = value;}
	}
}
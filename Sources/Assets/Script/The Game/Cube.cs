using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour 
{
	public List<Grid> grid;
	
	public static Cube cube_instance;
	// Use this for initialization
	
	void Awake()
	{
		cube_instance = this;
	}
	
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public static Cube Instance
	{
		get{return cube_instance;}
	}
}

[System.Serializable]
public class Grid
{
	public int id;
	public List<GameObject> cube;
	public int[] recap;
}
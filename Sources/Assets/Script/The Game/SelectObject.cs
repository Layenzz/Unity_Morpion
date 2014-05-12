using UnityEngine;
using System.Collections;

public class SelectObject: MonoBehaviour 
{
	private Material[] save;
	public GameObject selectObject;
	private GameObject current;
	private static SelectObject selectobject_instance;
	private Vector2 vMouse;
	private Rect rect;
	
	public GUIGame guigame;
	
	void Awake()
	{
		selectobject_instance = this;
	}
	
	void Start () 
	{
		selectObject = null;
		rect = new Rect(Screen.width/2.33f, Screen.height/1.27f, Screen.width/7, Screen.height/7);
		current = null;
	}

	// change the texture
	void switch_texture(out Material[] saveT, GameObject selectO)
	{
		// save texture to re-apply it when another object is selected
		saveT = selectO.renderer.materials;
		
		// copy the material array with length + 1 for "grid" texture
		Material[] mat = new Material[selectO.renderer.materials.Length + 1];
		int i = -1;
		while (++i < selectO.renderer.materials.Length)
			mat[i] = selectO.renderer.materials[i];

		mat[i] = Resources.Load("Grille", typeof(Material)) as Material;
		
		// apply the new texture with the grid
		selectO.renderer.materials = mat;		
	}
	
	// touch object, show gizmo
	void ray_touch_another_object(GameObject current)
	{
		// apply the original texture
		if (selectObject != null)
		{
			selectObject.renderer.materials = save;
			selectObject = current;
			switch_texture(out save, selectObject);	
		}
		else
		{
			selectObject = current;
			switch_texture(out save, selectObject);	
		}
	}
	
	// cast a ray to find gizmo or gameobjet
	void raycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (!rect.Contains(vMouse) && guigame.move == false && guigame.ev.init == false && guigame.ev.turn_game == false && guigame.ev.erase == false && guigame.ev.pity == false && guigame.rules.chrono.timer_list.Count == 0 && guigame.ev.change_player == false)
		{
			if (Physics.Raycast (ray, out hit, 100))
			{
				current = hit.collider.gameObject;

				int i = 0;

				foreach(GameObject obj in Cube.cube_instance.grid[Core.instance.current_grid].cube)
				{
					if (obj == current)
					{
						if (Cube.cube_instance.grid[Core.instance.current_grid].recap[i] != 0)
							return;
					}
					i++;
				}

				foreach (Invalid invalid in guigame.ev.invalid)
				{
					if (current == invalid.invalid)
						return;
				}

				if (current != selectObject)
					ray_touch_another_object(current);
			}
			else if (selectObject != null)
			{
				selectObject.renderer.materials = save;
				selectObject = null;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		vMouse = Input.mousePosition;
		vMouse.y = Screen.height - vMouse.y;
		if (Input.GetMouseButtonDown(0))
			raycast();
	}
	
	public static SelectObject instance
	{
		get {return selectobject_instance;}
	}
	
	public Material[] save_material
	{
		get {return save;}
	}
}
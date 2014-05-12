using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*

Manuel d'utilisation

Le script peut gérer tout les chronomètres du programme. 

Il suffit d'appeller la fonction "Create_new_timer" dans un autre script 
pour créer un chronomètre, et d'appeller ensuite la fonction Remove_timer
pour supprimer le timer de la liste.

 */

public class Chrono : MonoBehaviour 
{
	public static Chrono Chrono_instance;

	public List<Timer> timer_list;

	public GUISkin guiskin;

	void Awake()
	{
		Chrono_instance = this;
	}

	void Start () 
	{
		timer_list = new List<Timer>();	
	}

	public Timer Create_new_timer(bool is_active, string msg, float start_time, float timer, Rect rect_msg, int flag)
	{
		Timer time = new Timer();

		time.active = is_active;
		time.msg = msg;
		time.start_time = start_time;
		time.timer = timer;
		time.rect_msg = rect_msg;
		time.flag = flag;

		timer_list.Add(time);
		return time;
	}

	public void Remove_timer(Timer time)
	{
		timer_list.Remove(time);
	}

	void OnGUI()
	{
		GUI.skin = guiskin;
		if (timer_list.Count > 0)
		{
			foreach (Timer time in timer_list)
			{
				if (time.active == true)
				{
					if (time.flag == 0)
						GUI.Label (time.rect_msg, time.msg + (int)(Time.time - time.start_time));
					else if (time.flag == 1)
						GUI.Label (time.rect_msg, time.msg + (int)(time.timer - (Time.time - time.start_time)));
					else if (time.flag == 2)
						GUI.Label (time.rect_msg, time.msg);
					if (time.start_time + time.timer < Time.time)
						time.active = false;
				}
			}
		}
	}

	void Update ()
	{
	}

	public static Chrono chrono
	{
		get{return Chrono_instance;}
	}
}

public class Timer
{
	private bool _is_active;
	private string _message;
	private float _start_time;
	private float _timer;
	private Rect _rect_msg;
	private int _flag; // 0 : increment, 1 : decrement, 2 : hide chrono, 3 : hide chrono + msg

	public bool active
	{
		get{return _is_active;}
		set{_is_active = value;}
	}

	public string msg
	{
		get{return _message;}
		set{_message = value;}
	}

	public float timer
	{
		get{return _timer;}
		set{_timer = value;}
	}

	public float start_time
	{
		get{return _start_time;}
		set{_start_time = value;}
	}

	public Rect rect_msg
	{
		get{return _rect_msg;}
		set{_rect_msg = value;}
	}

	public int flag
	{
		get{return _flag;}
		set{_flag = value;}
	}
}
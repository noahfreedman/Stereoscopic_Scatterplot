using UnityEngine;
using System.Collections;

public class CameraState : MonoBehaviour 
{
	
	// physical location of each menu state
	public GameObject[] anchors;
	
	public const int STATE_BEGIN = 0;
	public const int STATE_MAIN_MENU = 1;
	public const int STATE_INTERCOMMUNICATION = 2;
	public const int STATE_RTT = 3;
	
	public int currentState;
	public float camSpeed;
	
	void Awake ()
	{
		anchors = new GameObject[4];
		anchors[0] = GameObject.Find ("AnchorBegin");
		anchors[1] = GameObject.Find ("AnchorMainMenu");
		anchors[2] = GameObject.Find ("AnchorIntercommunication");
		anchors[3] = GameObject.Find ("AnchorRTT");
		
		currentState = STATE_BEGIN;	
		camSpeed = 2.5f;
		
		transform.position = anchors[currentState].transform.position;
		transform.rotation = anchors[currentState].transform.rotation;
	}
		
	void Start () 
	{
	
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			currentState = 0;
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			currentState = 1;
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			currentState = 2;
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			currentState = 3;
		}
			
		transform.position = Vector3.Lerp(transform.position, anchors[currentState].transform.position, Time.deltaTime*camSpeed);
		transform.rotation = Quaternion.Slerp(transform.rotation, anchors[currentState].transform.rotation, Time.deltaTime*camSpeed);
	}
}

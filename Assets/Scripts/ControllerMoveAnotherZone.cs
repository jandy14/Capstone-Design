using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMoveAnotherZone : MonoBehaviour
{ 
    public enum Hand { Left, Right };
	[SerializeField] private Hand hand;
	[SerializeField] private GameObject portal;
    static private Transform[] handTransform = new Transform[2];
    static private bool[] isTrigged = new bool[2];
    private static bool shouldMovable;
	private GameObject gamePortal;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    void Start ()
    {
        //right
        if (hand == Hand.Right)
        {
            handTransform[0] = transform;
            isTrigged[0] = false;
        }
        //left
        else if (hand == Hand.Left)
        {
            handTransform[1] = transform;
            isTrigged[1] = false;
        }
    }

    void CheckEnableMoveZone()
    {
        if (isTrigged[0] == true && isTrigged[1] == true)
        {
            //Debug.Log(Vector3.Distance(handTransform[0].position, handTransform[1].position));
            if(Vector3.Distance(handTransform[0].position, handTransform[1].position) < 0.15)
                shouldMovable = true;
			if(hand == Hand.Right)
			{
				gamePortal = Instantiate(portal);
			}
        }
    }
    void CheckDisableMoveZone()
    {
        if (!(isTrigged[0] == true && isTrigged[1] == true))
        {
            shouldMovable = false;

			if (gamePortal)
				Destroy(gamePortal);
        }
    }

    void Update ()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (hand == Hand.Right)
            {
                isTrigged[0] = true;
            }
            else if (hand == Hand.Left)
            {
                isTrigged[1] = true;
            }
            CheckEnableMoveZone();
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            if (hand == Hand.Right)
            {
                isTrigged[0] = false;
            }
            else if (hand == Hand.Left)
            {
                isTrigged[1] = false;
            }
            CheckDisableMoveZone();
        }

        if (shouldMovable)
        {
			if(gamePortal)
			{
				gamePortal.transform.position = (handTransform[0].position + handTransform[1].position) / 2;
				gamePortal.transform.localScale = Vector3.one * 0.2f * Vector3.Distance(handTransform[0].position, handTransform[1].position);
				gamePortal.transform.eulerAngles = (handTransform[0].eulerAngles + handTransform[1].eulerAngles) / 2;
			}
			if (Vector3.Distance(handTransform[0].position, handTransform[1].position) > 1.2)
            {
                //Debug.Log(Vector3.Distance(handTransform[0].position, handTransform[1].position));
                //Debug.Log("MoveZone");
                if(SceneManager.GetActiveScene().buildIndex == 0)
                    SceneManager.LoadScene(1);
                else
                {
                    SceneManager.LoadScene(0);
                }
                shouldMovable = false;
            }
        }
    }
    void MoveZone()
    {

    }
}

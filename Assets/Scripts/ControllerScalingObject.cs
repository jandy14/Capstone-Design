using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScalingObject : MonoBehaviour {

    public enum Hand { Left, Right};

    private SteamVR_TrackedObject trackedObj;
    static private Transform[] handTransform = new Transform[2];
    static private bool[] isTrigged = new bool[2];
    private static bool shouldScale;
    private static float distance;
    private static Vector3 originalScale;
    private static Transform collidingObject;
    private static Transform collidingChildObject;
    private static FixedJoint fx;
    private float tmp_dis = 0f;

    public Hand controlHand;
    [Range(1f,10f)]
    public float maxScale;
    [Range(0.005f,0.1f)]
    public float minScale;


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
		if (controlHand == Hand.Left)
        {
            handTransform[0] = transform;
            isTrigged[0] = false;
        }
        else if (controlHand == Hand.Right)
        {
            handTransform[1] = transform;
            isTrigged[1] = false;
        }
        shouldScale = false;
	}

    void CheckEnableScaling()
    {
        if (isTrigged[0] == true && isTrigged[1] == true && collidingObject)
        {
            shouldScale = true;
            distance = Vector3.Distance(handTransform[0].position, handTransform[1].position);
            originalScale = collidingObject.localScale;
        }
    }
    void CheckDisableScaling()
    {
        if (!(isTrigged[0] == true && isTrigged[1] == true))
        {
            shouldScale = false;
            collidingObject = null;
            //tmp_dis = 0f;
            tmp_dis = 1f;
        }
    }

    void Scaling()
    {
        if(controlHand == Hand.Right)
        {
            float rate = Vector3.Distance(handTransform[0].position, handTransform[1].position) / distance;
            rate = Mathf.Round(rate * 100 + (rate * 100) % 2) * 0.01f;
            
            if (Mathf.Abs(tmp_dis - rate) > 0.2 )
            {
                if (originalScale.x * rate > maxScale)
                {
                    collidingObject.localScale = Vector3.one * maxScale;
                    rate = maxScale / originalScale.x;
                }
                else if (originalScale.x * rate < minScale)
                {
                    collidingObject.localScale = Vector3.one * minScale;
                    rate = minScale / originalScale.x;
                }
                else
                    collidingObject.localScale = originalScale * rate;

                //GetComponent<FixedJoint>().connectedBody = null;
                //Debug.DrawLine(collidingObject.position, collidingChildObject.position, Color.blue, 5f);
                collidingObject.position = collidingChildObject.position + (collidingObject.position - collidingChildObject.position) * (rate/tmp_dis);
                //Debug.DrawLine(collidingObject.position, collidingChildObject.position, Color.red,5f);
                //GetComponent<FixedJoint>().connectedBody = collidingObject.GetComponent<Rigidbody>();
                
                tmp_dis = rate;
            }
            
        }
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            if (controlHand == Hand.Left)
            {
                isTrigged[0] = true;
            }
            else if (controlHand == Hand.Right)
            {
                isTrigged[1] = true;
            }
            CheckEnableScaling();
        }
        if (Controller.GetHairTriggerUp())
        {
            if (controlHand == Hand.Left)
            {
                isTrigged[0] = false;
            }
            else if (controlHand == Hand.Right)
            {
                isTrigged[1] = false;
            }
            CheckDisableScaling();
        }

        if (shouldScale)
        {

            Scaling();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "POWERCUBE")
        {
            if (collidingObject) return;

            collidingObject = other.transform.parent;
            collidingChildObject = other.transform;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "POWERCUBE")
        {
            if (collidingObject) return;

            collidingObject = other.transform.parent;
            collidingChildObject = other.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}

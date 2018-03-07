﻿using System.Collections;
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

    public Hand controlHand;
    [Range(1f,10f)]
    public float maxScale;
    [Range(0.02f,0.2f)]
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
        }
    }

    void Scaling()
    {
        if(controlHand == Hand.Right)
        {
            float rate = Vector3.Distance(handTransform[0].position, handTransform[1].position) / distance;
            if(originalScale.x * rate > maxScale)
            {
                collidingObject.localScale = Vector3.one * maxScale;
            }
            else if (originalScale.x * rate < minScale)
            {
                collidingObject.localScale = Vector3.one * minScale;
            }
            else
                collidingObject.localScale = originalScale * rate;
            
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
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "POWERCUBE")
        {
            if (collidingObject) return;

            collidingObject = other.transform.parent;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}

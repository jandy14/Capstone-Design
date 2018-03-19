using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartPosition : MonoBehaviour {

    [SerializeField] private Transform v;
	// Use this for initialization
	void Start ()
    {
        transform.position = v.position + (v.rotation * Vector3.forward * 0.5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 tmp = v.rotation.eulerAngles * Mathf.Deg2Rad;
        //Vector3 result = Vector3.zero;
        //result += new Vector3(0, Mathf.Cos(tmp.x), Mathf.Sin(tmp.x));
        //transform.position = v.position + result;
        //transform.position = v.position + (v.rotation * Vector3.forward);
	}
}

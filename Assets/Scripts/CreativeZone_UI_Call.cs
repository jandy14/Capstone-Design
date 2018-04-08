using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeZone_UI_Call : MonoBehaviour {
    public Transform v;
    public  GameObject UI;
    private SteamVR_TrackedObject trackedObj;
    private bool UI_ON = false;
    private GameObject laser = GameObject.Find("CurvedUILaserBeam");

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        UI.transform.position = v.position + new Vector3(0, 2000, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if(Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
            if(UI_ON)
            {
                UI.transform.position = v.position + new Vector3(0, 2000, 0);
                UI_ON = false;
                laser.gameObject.SetActive(UI_ON);
            }
            else
            {
                UI.transform.rotation = v.rotation;
				UI.transform.eulerAngles = new Vector3(UI.transform.eulerAngles.x, UI.transform.eulerAngles.y,0f);
                UI.transform.position = v.position + (v.rotation * Vector3.forward * 2.0f);
                UI_ON = true;
                laser.gameObject.SetActive(UI_ON);

            }

        }

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    public GameObject Cube;

    public Transform cameraRigTransform;
    public GameObject teleortReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    private Transform hitObj;
    private bool shouldTeleport;
    private bool shouldMake;
    public GameObject whoisyourmother;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);

    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleortReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    // Update is called once per frame
    void Update () {
		if(Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {

            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                reticle.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;

                if (Controller.GetAxis().y >= 0)
                {
                    shouldMake = false;
                    shouldTeleport = true;

                }
                else
                {
                    shouldMake = true;
                    hitObj = hit.collider.gameObject.transform;
                    Debug.Log(hitObj);
                    shouldTeleport = false;
                }
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if(Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (shouldTeleport)
                Teleport();
            else if (shouldMake && hitObj.tag.Equals("POWERCUBE"))
                MakeCube();
        }
	}
    void MakeCube()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 10;
        GameObject g = Instantiate(Cube, whoisyourmother.transform);
        Vector3 test = whoisyourmother.transform.InverseTransformPoint(hitPoint);

        Vector3 tmp = test - hitObj.localPosition;

        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y) && Mathf.Abs(tmp.x) > Mathf.Abs(tmp.z))
            g.transform.localPosition= hitObj.localPosition + (tmp.x / Mathf.Abs(tmp.x)) * new Vector3(.15f, 0, 0);

        else if (Mathf.Abs(tmp.y) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.y / Mathf.Abs(tmp.y)) * new Vector3(0, .15f, 0);

        else
            g.transform.localPosition = hitObj.localPosition + (tmp.z / Mathf.Abs(tmp.z)) * new Vector3(0, 0, .15f);


        g.transform.parent = whoisyourmother.transform;
        

        hitObj = null;
    }

    
}

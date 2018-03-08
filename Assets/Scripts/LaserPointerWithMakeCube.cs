﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerWithMakeCube : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private GameObject previewCube;
    private Vector3 hitPoint;
    public GameObject Cube;

    public Transform cameraRigTransform;
    public Transform headTransform;
    public LayerMask makeCubeMask;
    private Transform hitObj;
    private bool shouldMake;
    private bool shouldDelete;
    private GameObject hitObjParent;

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
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        previewCube = Instantiate(Cube);
        previewCube.GetComponent<MeshRenderer>().material.color = new Color(1f, 0, 0, 0.3f);
        Destroy(previewCube.GetComponent<BoxCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {

            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, makeCubeMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);

                if (Controller.GetAxis().y < 0)
                {
                    shouldMake = true;
                    shouldDelete = false;
                    hitObj = hit.collider.gameObject.transform;
                    hitObjParent = hitObj.parent.gameObject;
                    laser.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    shouldMake = false;
                    shouldDelete = true;
                    hitObj = hit.collider.gameObject.transform;
                    hitObjParent = hitObj.parent.gameObject;
                    laser.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
            else
            {
                laser.SetActive(false);
                shouldDelete = false;
                shouldMake = false;
            }
        }
        else
        {
            laser.SetActive(false);
        }

        if (shouldMake)
        {
            ShowPreviewCube();
        }
        else
        {
            previewCube.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (hitObj)
            {
                if (shouldMake && hitObj.tag.Equals("POWERCUBE"))
                    MakeCube();
                else if (shouldDelete && hitObj.tag.Equals("POWERCUBE"))
                    DeleteCube();

                shouldMake = false;
                shouldDelete = false;
            }
        }
    }
    void MakeCube()
    {
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 10;
        GameObject g = Instantiate(Cube, hitObjParent.transform);
        Vector3 test = hitObjParent.transform.InverseTransformPoint(hitPoint);

        Vector3 tmp = test - hitObj.localPosition;

        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y) && Mathf.Abs(tmp.x) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.x / Mathf.Abs(tmp.x)) * new Vector3(.15f, 0, 0);

        else if (Mathf.Abs(tmp.y) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.y / Mathf.Abs(tmp.y)) * new Vector3(0, .15f, 0);

        else
            g.transform.localPosition = hitObj.localPosition + (tmp.z / Mathf.Abs(tmp.z)) * new Vector3(0, 0, .15f);


        g.transform.parent = hitObjParent.transform;


        hitObj = null;
    }
    void DeleteCube()
    {
        if(hitObjParent.transform.childCount > 1)
        {
            Destroy(hitObj.gameObject);
        }
    }
    void ShowPreviewCube()
    {
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 10;
        GameObject g = previewCube;
        Vector3 tmpScale = g.transform.localScale;
        g.transform.parent = hitObjParent.transform;
        g.transform.localScale = tmpScale;
        g.transform.localRotation = Quaternion.Euler(0, 0, 0);
        previewCube.SetActive(true);
        Vector3 test = hitObjParent.transform.InverseTransformPoint(hitPoint);

        Vector3 tmp = test - hitObj.localPosition;

        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y) && Mathf.Abs(tmp.x) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.x / Mathf.Abs(tmp.x)) * new Vector3(.15f, 0, 0);

        else if (Mathf.Abs(tmp.y) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.y / Mathf.Abs(tmp.y)) * new Vector3(0, .15f, 0);

        else
            g.transform.localPosition = hitObj.localPosition + (tmp.z / Mathf.Abs(tmp.z)) * new Vector3(0, 0, .15f);
    }
}

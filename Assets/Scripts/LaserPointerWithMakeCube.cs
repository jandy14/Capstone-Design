using System.Collections;
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
	public AudioClip makeSound;
	public AudioClip removeSound;

	public Transform cameraRigTransform;
	public Transform headTransform;
	public LayerMask makeCubeMask;
	private Transform hitObj;
	private bool shouldMake;
	private bool shouldDelete;
	private GameObject hitObjParent;
	[SerializeField] private AudioSource soundPlayer;

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

                if(hitObj)
                {
                    hitObj.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                hitObj = hit.collider.gameObject.transform;
                hitObjParent = hitObj.parent.gameObject;

                if (Controller.GetAxis().y < 0)
                {
                    shouldMake = true;
                    shouldDelete = false;
                    laser.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    shouldMake = false;
                    shouldDelete = true;
                    laser.GetComponent<MeshRenderer>().material.color = Color.blue;
                    hitObj.GetComponent<MeshRenderer>().material.color = new Color(.6f, .6f, 1f, 1f);
                }
            }
            else
            {
                laser.SetActive(false);
                shouldDelete = false;
                shouldMake = false;
                if(hitObj)
                    hitObj.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
        else
        {
            laser.SetActive(false);
            if (hitObj)
                hitObj.GetComponent<MeshRenderer>().material.color = Color.white;
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
		g.transform.localEulerAngles = Vector3.zero;
        Vector3 test = hitObjParent.transform.InverseTransformPoint(hitPoint);

        Vector3 tmp = test - hitObj.localPosition;

        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y) && Mathf.Abs(tmp.x) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.x / Mathf.Abs(tmp.x)) * new Vector3(1f, 0, 0);

        else if (Mathf.Abs(tmp.y) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.y / Mathf.Abs(tmp.y)) * new Vector3(0, 1f, 0);

        else
            g.transform.localPosition = hitObj.localPosition + (tmp.z / Mathf.Abs(tmp.z)) * new Vector3(0, 0, 1f);


        g.transform.parent = hitObjParent.transform;


        hitObj = null;

		soundPlayer.clip = makeSound;
		soundPlayer.Play();

    }
    void DeleteCube()
    {
        if(hitObjParent.transform.childCount > 1)
        {
            Destroy(hitObj.gameObject);
        }
		soundPlayer.clip = removeSound;
		soundPlayer.Play();
	}
    void ShowPreviewCube()
    {
        GameObject g = previewCube;
        g.transform.parent = hitObjParent.transform;
        previewCube.SetActive(true);
        Vector3 test = hitObjParent.transform.InverseTransformPoint(hitPoint);

        Vector3 tmp = test - hitObj.localPosition;

        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y) && Mathf.Abs(tmp.x) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.x / Mathf.Abs(tmp.x)) * new Vector3(1f, 0, 0);

        else if (Mathf.Abs(tmp.y) > Mathf.Abs(tmp.z))
            g.transform.localPosition = hitObj.localPosition + (tmp.y / Mathf.Abs(tmp.y)) * new Vector3(0, 1f, 0);

        else
            g.transform.localPosition = hitObj.localPosition + (tmp.z / Mathf.Abs(tmp.z)) * new Vector3(0, 0, 1f);

        g.transform.SetParent(null, true);
        g.transform.localScale = hitObj.transform.lossyScale;
        g.transform.rotation = hitObj.transform.rotation;
        previewCube.SetActive(true);
        
    }
}

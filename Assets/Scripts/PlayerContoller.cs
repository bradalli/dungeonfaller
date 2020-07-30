using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    float speedMultiplier;
    [SerializeField] float resetSpeed;
    [SerializeField] Transform cameraRig;
    [SerializeField] float camMoveAmount, camLookAmount;
    Camera mainCam;

    [SerializeField] Vector3 bodyDir;
    Vector3 bodyDirVel;
    [SerializeField] float bodyDirSmooth;
    [SerializeField] Transform body;
    Animator anim;
    public bool fast;

    [SerializeField] Transform landingSpot;
    LineRenderer landingLine;
    [SerializeField] LayerMask mask;
    RaycastHit hit;
    BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        bodyDir = body.forward;
        anim = GetComponentInChildren<Animator>();
        landingLine = landingSpot.GetComponent<LineRenderer>();
        col = body.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fast) { speedMultiplier = 0.75f;  col.size = Vector3.one * 0.6f;  col.center = new Vector3(0, 0.25f, 0); }
        else { speedMultiplier = 1; col.size = new Vector3(1, 1, 1.8f); col.center = new Vector3(0, 0.25f, -0.2f); }

        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(inputVec * playerSpeed * speedMultiplier * Time.deltaTime);
        fast = Input.GetMouseButton(0);
        anim.SetBool("FastFall?", fast);
        
        body.transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(body.forward, inputVec, ref bodyDirVel, bodyDirSmooth), body.up);

        mainCam.transform.position = new Vector3(transform.position.x * camMoveAmount, 5, transform.position.z * camMoveAmount);
        mainCam.transform.rotation = Quaternion.LookRotation(transform.position * camLookAmount - Camera.main.transform.position, Camera.main.transform.up);

        if (transform.position != Vector3.zero)
        {
            transform.Translate(Vector3.ClampMagnitude(-transform.position, 1) * resetSpeed * Time.deltaTime);
        }

        Physics.Raycast(transform.position, Vector3.down, out hit, 20, mask);
        if(hit.collider)
        {
            landingSpot.gameObject.SetActive(true);
            landingSpot.position = hit.point;
            landingLine.SetPosition(0, transform.position);
            landingLine.SetPosition(1, hit.point);
        }

        else { landingSpot.gameObject.SetActive(false); }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("IMPACT");
    }

}

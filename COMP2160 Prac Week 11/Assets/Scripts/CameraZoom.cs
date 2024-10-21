using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomChangeAmount = 5f;
    [SerializeField] private float minPerspectiveZoom = 30f;
    [SerializeField] private float maxPerspectiveZoom = 100f;
    [SerializeField] private float minOrthoZoom = 3f;
    [SerializeField] private float maxOrthoZoom = 10f;

    private Camera cam;
    private Actions actions;
    private InputAction zoomAction;
    private float zoomAmount = 50f;

    void Awake()
    {
        actions = new Actions();
        zoomAction = actions.camera.zoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        zoomAction.Enable();
    }

    void OnDisable()
    {
        zoomAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float zoomDirection = zoomAction.ReadValue<float>();

        // scrolling up
        if (zoomDirection > 0f && zoomAmount > 0f)
        {
            // zoom in
            zoomAmount -= zoomChangeAmount;
        }
        // scrolling down
        else if (zoomDirection < 0f && zoomAmount < 100f)
        {
            // zoom out
            zoomAmount += zoomChangeAmount;
        }

        if (cam.orthographic)
        {
            // set the orthographic camera size proportional the zoomAmount
            cam.orthographicSize = minOrthoZoom + (zoomAmount/100f * (maxOrthoZoom - minOrthoZoom));
        }
        else
        {
            // set the perspective camera size proportional the zoomAmount
            cam.fieldOfView = minPerspectiveZoom + (zoomAmount/100f * (maxPerspectiveZoom - minPerspectiveZoom));
        }

        //Debug.Log(zoomDirection);
    }
}

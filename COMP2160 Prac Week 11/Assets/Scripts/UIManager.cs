/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;
using WordsOnPlay.Utils;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
#endregion 

    [SerializeField] private bool useMouseDelta;
    [SerializeField] private float mouseSpeed = 1;
    [SerializeField] private RectTransform bounds;

    #region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    private void MoveCrosshair() 
    {
        if (!useMouseDelta)
        {
            if (Cursor.lockState != CursorLockMode.Confined)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }

            Vector2 mousePosVec2 = mouseAction.ReadValue<Vector2>();
            //Debug.Log(mousePosVec2);

            // FIXME: Move the crosshair position to the mouse position (in world coordinates)
            Camera cam = Camera.main;

            Vector3 mousePosVec3 = new Vector3(mousePosVec2.x, mousePosVec2.y, 0);

            Ray ray = cam.ScreenPointToRay(mousePosVec3);

            LayerMask groundMask = LayerMask.GetMask("Walls");

            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, groundMask))
            {
                crosshair.position = hit.point + (Vector3.up * 0.1f);
            }
        }
        // use mouse delta
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            Vector2 mouseDeltaVec2 = deltaAction.ReadValue<Vector2>();

            Vector3 mouseDeltaVec3 = new Vector3(mouseDeltaVec2.x, 0f, mouseDeltaVec2.y);

            crosshair.position += mouseDeltaVec3 / 100f * mouseSpeed;

            crosshair.position = bounds.rect.Clamp(crosshair.position);
        }
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }

#endregion Update

    void OnDrawGizmos()
    {
        //bounds.DrawGizmo();
    }
}

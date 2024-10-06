using Unity.Netcode;
using UnityEngine;

public class RigidbodyDragController : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _moveSpeed = 3.0f;

    [SerializeField]
    private float _rotationSpeed = 10.0f;

    [SerializeField]
    private Camera _camera;

    private bool _continueDrag;

    private Vector2 cursorHotspot = Vector2.zero; // The point of the cursor that "clicks", usually the center or top-left corner

    [SerializeField]
    private Texture2D _dragCursor;  // Assign your hand or grab icon here

    private void Start()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        if (_camera == null)
            _camera = transform.parent.gameObject.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        if (_rigidbody == null)
            return;

        if (_camera == null)
            return;

        // Detect when the user presses the mouse button to start dragging
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        // Detect when the user releases the mouse button to stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }

        if (_continueDrag)
        {
            MoveRigidbodyToMouse();
        }
        else
        {
            HandleCursorChange();  // Only change cursor when not dragging
        }
    }

    private void StopDrag()
    {
        _continueDrag = false;
        _rigidbody.useGravity = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  // Reset cursor when dragging stops
    }

    private void TryStartDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits a collider
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit has the rigidbody we're interested in
            if (hit.rigidbody != null && hit.rigidbody == _rigidbody)
            {
                _continueDrag = true;
            }
        }
    }

    private void MoveRigidbodyToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _camera.WorldToScreenPoint(_rigidbody.position).z;  // Keep the z depth consistent

        Vector3 targetPosition = _camera.ScreenToWorldPoint(mousePos);
        targetPosition.z = _rigidbody.position.z;  // Ensure it maintains the z position of the rigidbody

        // Raycast downwards from the balls current position to detect the ground
        Ray ray = new Ray(_rigidbody.position, Vector3.down); // Cast from the ball's current position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Get the Y position of the ground, adjusted for the coin's size
            float groundYPosition = hit.point.y + _rigidbody.GetComponent<Collider>().bounds.extents.y;
            targetPosition.y = Mathf.Max(groundYPosition, targetPosition.y); // Clamp Y to prevent going below ground
        }

        // Smoothly move the rigidbody to the target position
        Vector3 newPosition = Vector3.Lerp(_rigidbody.position, targetPosition, _moveSpeed * Time.deltaTime);
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.MovePosition(newPosition);

        // Define the target rotation with the upright rotation (lock X and Z to 0)
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        // Smoothly rotate towards the target rotation
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void HandleCursorChange()
    {
        if (_dragCursor == null)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the cursor is over the Rigidbody
        if (Physics.Raycast(ray, out hit) && hit.rigidbody == _rigidbody)
        {
            // Set the cursor to the drag icon when hovering over the rigidbody
            Cursor.SetCursor(_dragCursor, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            // Reset cursor to default when not hovering over the rigidbody
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
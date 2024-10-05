using UnityEngine;

public class RigidbodyDragToMousePositionController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _speed = 3.00f;

    private bool _continueDrag;

    private Vector2 cursorHotspot = Vector2.zero; // The point of the cursor that "clicks", usually the center or top-left corner

    [SerializeField]
    private Texture2D _dragCursor;  // Assign your hand or grab icon here

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody == null)
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        mousePos.z = Camera.main.WorldToScreenPoint(_rigidbody.position).z;  // Keep the z depth consistent

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.z = _rigidbody.position.z;  // Ensure it maintains the z position of the rigidbody

        // Smoothly move the rigidbody to the target position
        Vector3 newPosition = Vector3.Lerp(_rigidbody.position, targetPosition, _speed * Time.deltaTime);
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.MovePosition(newPosition);
    }

    private void HandleCursorChange()
    {
        if (_dragCursor == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
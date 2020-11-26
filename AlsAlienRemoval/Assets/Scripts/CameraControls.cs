using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    public CompositeCollider2D levelCollider;   // Composite collider of level's tilemap
    public CanvasScaler canvasScaler;           // Scaler of canvas camera renders to, used to access reference resolution for calculating aspect ratio of visible area
    public RectTransform mainUIPanel;           // Main UI panel of canvas, used for saem calculations as canvas

    private Camera _camera;     // Camera component
    private float _levelBoundX; // Distance from origin to which level extends in units
    private float _levelBoundY;

    private float _visibleAspectRatio;
    private float _visibleCenterOffset;
    private float _heightvisible;

    private float _maxOrthographicSize;

    // Start is called before the first frame update
    void Start()
    {
        // Get Camera component
        _camera = gameObject.GetComponent<Camera>();

        // Get bounds of level
        _levelBoundX = levelCollider.bounds.extents.x;
        _levelBoundY = levelCollider.bounds.extents.y;

        // Get aspect ratio of area not obstructed by UI, and width/height ratio of level bounds
        _heightvisible = 1 - mainUIPanel.rect.height / canvasScaler.referenceResolution.y; // Proportion of height not covered by UI
        _visibleAspectRatio = _camera.aspect / _heightvisible;   // (Width / Height) / heightVisible = Width / (Height * heightVisible)
        float levelBoundRatio = _levelBoundX / _levelBoundY;

        // Determine maximum camera size
        if (levelBoundRatio > _visibleAspectRatio)
        {
            // Size constrained by height
            _maxOrthographicSize = _levelBoundY * (1 / _heightvisible);
        }
        else
        {
            // Size constrained by width
            _maxOrthographicSize = _levelBoundX / _camera.aspect;
        }

        // Set default camera size to max size
        _camera.orthographicSize = _maxOrthographicSize;

        // Center camera's visible portion on level
        _visibleCenterOffset = _camera.orthographicSize * (1 - _heightvisible); // Distance between center of camera, and center of visible area in units. Same as area obstructed by UI
        transform.position = new Vector3(levelCollider.bounds.center.x, levelCollider.bounds.center.y - _visibleCenterOffset, -10);


    }

    // Update is called once per frame
    void Update()
    {
        // Adjust orthographic size/zoom according to scroll wheel and recalculate center offset
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 4, _maxOrthographicSize);
        _visibleCenterOffset = _camera.orthographicSize * (1 - _heightvisible);

        // Pan camera and clamp within bounds of level
        float panSpeed = _camera.orthographicSize / (_maxOrthographicSize * _maxOrthographicSize);  // Camera pans slower as zoom increases like 1/x^2
        transform.position = new Vector3(

            // X position
            Mathf.Clamp(transform.position.x + Input.GetAxis("Horizontal") * panSpeed, 
            levelCollider.bounds.center.x - _levelBoundX + _camera.orthographicSize * _camera.aspect,   // Minimum bound
            levelCollider.bounds.center.x + _levelBoundX - _camera.orthographicSize * _camera.aspect),  // Maximum bound

            // Y position
            Mathf.Clamp(transform.position.y + Input.GetAxis("Vertical") * panSpeed, 
            levelCollider.bounds.center.y - _levelBoundY + _camera.orthographicSize * _heightvisible - _visibleCenterOffset,    // Minimum bound
            levelCollider.bounds.center.y + _levelBoundY - _camera.orthographicSize * _heightvisible - _visibleCenterOffset),   // Maximum bound
            -10);
        

    }
}

using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 public class PinchableScrollRect : ScrollRect
 {
     [SerializeField] float _minZoom = .1f;
     [SerializeField] float _maxZoom = 10;
     [SerializeField] float _zoomLerpSpeed = 10f;
     float _currentZoom = 1;
     bool _isPinching = false;
     float _startPinchDist;
     float _startPinchZoom;
     Vector2 _startPinchCenterPosition;
     Vector2 _startPinchScreenPosition;
     float _mouseWheelSensitivity = 1;
     bool blockPan = false;
 
    /// <summary>
    /// Called on creation
    /// </summary>
    protected override void Awake()
    {
        Input.multiTouchEnabled = true;
    }
 
    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            if (!_isPinching)
            {
                _isPinching = true;
                OnPinchStart();
            }
            OnPinch();
        }
        else
        {
            _isPinching = false;
            if (Input.touchCount == 0)
                blockPan = false;
        }

        //pc input
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheelInput) > float.Epsilon)
        {
            _currentZoom *= 1 + scrollWheelInput * _mouseWheelSensitivity;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            _startPinchScreenPosition = (Vector2)Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, _startPinchScreenPosition, null, out _startPinchCenterPosition);
            Vector2 pivotPosition = new Vector3(content.pivot.x * content.rect.size.x, content.pivot.y * content.rect.size.y);
        }
       //pc input end

       if (Mathf.Abs(content.localScale.x - _currentZoom) > 0.001f)
           content.localScale = Vector3.Lerp(content.localScale, Vector3.one * _currentZoom, _zoomLerpSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Set the anchored position of the current page object
    /// </summary>
    /// <param name="position"></param>
    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        if (_isPinching || blockPan) return;
        base.SetContentAnchoredPosition(position);
    }

    /// <summary>
    /// Called when the scroll rect detects a pinch is occurring
    /// </summary>
    void OnPinchStart()
    {
        Vector2 pos1 = Input.touches[0].position;
        Vector2 pos2 = Input.touches[1].position;
        _startPinchDist = Distance(pos1, pos2) * content.localScale.x;
        _startPinchZoom = _currentZoom;
        blockPan = true;
    }
 
    /// <summary>
    /// Determine the new zoom based on th epinch
    /// </summary>
    void OnPinch()
    {
        float currentPinchDist = Distance(Input.touches[0].position, Input.touches[1].position) * content.localScale.x;
        _currentZoom = (currentPinchDist / _startPinchDist) * _startPinchZoom;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
    }

    /// <summary>
    /// Used to calculate the distance between two touch inputs, converting to screen space
    /// </summary>
    /// <param name="pos1">First touch input</param>
    /// <param name="pos2">Second touch input</param>
    /// <returns>float distance between the two points</returns>
    float Distance(Vector2 pos1, Vector2 pos2)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos1, null, out pos1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos2, null, out pos2);
        return Vector2.Distance(pos1, pos2);
    }
 }
using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BatterRotater : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 1;
    [SerializeField, Tooltip("自動で混ぜられる速度")] private float _defaultMixSpeed;
    [SerializeField] private float _scaleUpSpeed = 1;
    [SerializeField] private float _scaleDownSpeed;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private Image _batterImageObj;
    [SerializeField] private Text _angleText;
    [SerializeField, Tooltip("ケーキのUIが入っているCanvas")] private Canvas _parentCanvas;
    [SerializeField, Tooltip("回転方向、trueが右、falseが左")] private bool _rotateDirection;
    private RectTransform _canvasRectTransform;

    private float _currentScale = 1;
    private float _lastRotateRad;
    private float _currentMixSpeed;
    private float _prevRad;
    private float _velocity;
    [SerializeField, Range(0, 100)] private float _deceleration = 0.5f;
    public float Velocity => _velocity;

    private void Awake()
    {
        _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
        _currentMixSpeed = _defaultMixSpeed;
    }

    public void Initialize()
    {
        _currentScale = 1;
        _currentMixSpeed = _defaultMixSpeed;
    }
    public float BatterRotate(float deltaTime)
    {
        // CanvasのRectTransform内にあるマウスの座標をローカル座標に変換する
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            Input.mousePosition,
            _parentCanvas.worldCamera, 
            out var mousePosition);
        
        var rad = Mathf.Atan2(mousePosition.x, mousePosition.y);
        var dRad = Mathf.Tan(rad - _prevRad);
        if (Input.GetMouseButton(0) && dRad != 0)
        {
            _velocity += dRad * deltaTime;
            //  正しい方向に回していたらScaleを上げる
            if (_rotateDirection && dRad > 0)
            {
                _currentScale += dRad * 0.02f * _scaleUpSpeed;
            }
            else if(!_rotateDirection && dRad < 0)
            {
                _currentScale += Mathf.Abs(dRad) * 0.02f * _scaleUpSpeed;
            }
        }
        else
        {
            var sign = Mathf.Sign(_velocity);
            _velocity = Mathf.Clamp(Mathf.Abs(_velocity) - _deceleration * deltaTime, 0f, float.MaxValue) * sign;
        }

        _prevRad = rad;
        _currentScale -= _scaleDownSpeed * deltaTime;
        _currentScale = Mathf.Clamp(_currentScale, _minScale, _maxScale);
        _batterImageObj.transform.localScale = Vector3.one * _currentScale;

        return dRad;
    }
    
    private static Vector2 Rotate(Vector2 from, float angleRad)
    {
        Complex result = new Complex(from.x, from.y) 
                         * new Complex(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        return new Vector2((float) result.Real, (float) result.Imaginary);
    }
}

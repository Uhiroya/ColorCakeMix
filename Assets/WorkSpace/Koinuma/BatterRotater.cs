using System.Numerics;
using UniRx;
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
    public BoolReactiveProperty OppositeDirection { get; }= new();

    private float _currentScale = 1;
    private float _lastRotateRad;
    private float _prevRad;
    private float _velocity;
    private Vector2 _prevMousePosition;
    [SerializeField, Range(0, 100)] private float _deceleration = 0.5f;
    public float Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    /// <summary>回転方向、trueが右、falseが左</summary>
    public bool RotateDirection => _rotateDirection;

    private void Awake()
    {
        _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
    }

    public void Initialize()
    {
        _currentScale = 1;
        _batterImageObj.transform.localScale = Vector3.one * _currentScale;
    }
    public float BatterRotate(float deltaTime)
    {
        // CanvasのRectTransform内にあるマウスの座標をローカル座標に変換する
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            Input.mousePosition,
            _parentCanvas.worldCamera, 
            out var mousePosition);
        
        var angle = 0f;
        var distance = Vector2.zero;
        Vector2 angleFromLast = Rotate(mousePosition, _lastRotateRad);
        if (Input.GetMouseButton(0))
        {
            angle = Mathf.Atan2(angleFromLast.x, angleFromLast.y);
            distance = _prevMousePosition - mousePosition;
        }
        if (distance != Vector2.zero)
        {
            _velocity += angle * deltaTime;
            //  正しい方向に回していたらScaleを上げる
            if (_rotateDirection)
            {
                if (angle > 0)
                {
                    _currentScale += angle * 0.02f * _scaleUpSpeed;
                    OppositeDirection.Value = false;
                }
                else
                {
                    OppositeDirection.Value = true;
                }
            }
            else if(!_rotateDirection)
            {
                if (angle < 0)
                {
                    _currentScale += Mathf.Abs(angle) * 0.02f * _scaleUpSpeed;
                    OppositeDirection.Value = false;
                }
                else
                {
                    OppositeDirection.Value = true;
                }
            }
        }
        else
        {
            var sign = Mathf.Sign(_velocity);
            _velocity = Mathf.Clamp(Mathf.Abs(_velocity) - _deceleration * deltaTime, 0f, float.MaxValue) * sign;
        }

        _prevMousePosition = mousePosition;
        _lastRotateRad = Mathf.Atan2(mousePosition.x, mousePosition.y);
        _currentScale -= _scaleDownSpeed * deltaTime;
        _currentScale = Mathf.Clamp(_currentScale, _minScale, _maxScale);
        _batterImageObj.transform.localScale = Vector3.one * _currentScale;

        return angle;
    }
    
    private static Vector2 Rotate(Vector2 from, float angleRad)
    {
        Complex result = new Complex(from.x, from.y) 
                         * new Complex(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        return new Vector2((float) result.Real, (float) result.Imaginary);
    }
}

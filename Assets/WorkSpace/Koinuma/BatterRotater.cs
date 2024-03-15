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
    [SerializeField] private GameObject _batterImageObj;
    [SerializeField] private Text _angleText;

    private float _currentScale = 1;
    private float _lastRotateRad;

    public void Initialize()
    {
        _batterImageObj.transform.localScale = Vector3.one;
    }

    public float CurrentRotateSpeed;
    public float DefaultMixSpeed => _defaultMixSpeed;
    
    public float BatterRotate(float deltaTime)
    {
        Vector2 mousePos = Input.mousePosition - transform.position;
        float rotateRadFromLast = _defaultMixSpeed * deltaTime;
        CurrentRotateSpeed = _defaultMixSpeed;

        if (Input.GetMouseButton(0))
        {
            Vector2 angleFromLast = Rotate(mousePos, _lastRotateRad);
            rotateRadFromLast += Mathf.Atan2(angleFromLast.x, angleFromLast.y);
            CurrentRotateSpeed += Mathf.Atan2(angleFromLast.x, angleFromLast.y);
            _currentScale += Mathf.Clamp(rotateRadFromLast, 0, rotateRadFromLast) * 0.02f * _scaleUpSpeed;

            if (_angleText)
            {
                _angleText.text = (rotateRadFromLast * 100).ToString();
                _angleText.color = (rotateRadFromLast > 0)? Color.red : Color.blue;
            }
        }

        _lastRotateRad = Mathf.Atan2(mousePos.x, mousePos.y);
        _currentScale -= _scaleDownSpeed * deltaTime;
        _currentScale = Mathf.Clamp(_currentScale, _minScale, _maxScale);
        _batterImageObj.transform.localScale = Vector3.one * _currentScale;
        _batterImageObj.transform.Rotate(Vector3.forward, -Mathf.Clamp(rotateRadFromLast, 0, rotateRadFromLast) * _rotateSpeed);

        return rotateRadFromLast;
    }
    
    private static Vector2 Rotate(Vector2 from, float angleRad)
    {
        Complex result = new Complex(from.x, from.y) 
                         * new Complex(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        return new Vector2((float) result.Real, (float) result.Imaginary);
    }
}

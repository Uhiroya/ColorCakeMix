using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BatterRotater : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 1;
    [SerializeField] private float _scaleUpSpeed = 1;
    [SerializeField] private float _scaleDownSpeed;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private GameObject _batterImageObj;
    [SerializeField] private Text _angleText;

    private float _currentScale = 1;
    private float _lastRotateRad;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition - transform.position;

        if (Input.GetMouseButton(0))
        {
            Vector2 angleFromLast = Rotate(mousePos, _lastRotateRad);
            float rotateRadFromLast = Mathf.Atan2(angleFromLast.x, angleFromLast.y);
            _currentScale += Mathf.Abs(rotateRadFromLast) * 0.02f * _scaleUpSpeed;
            _batterImageObj.transform.Rotate(Vector3.forward, -rotateRadFromLast * _rotateSpeed);

            if (_angleText)
            {
                _angleText.text = (rotateRadFromLast * 100).ToString();
                _angleText.color = (rotateRadFromLast > 0)? Color.red : Color.blue;
            } 
        }

        _lastRotateRad = Mathf.Atan2(mousePos.x, mousePos.y);
        _currentScale -= _scaleDownSpeed * Time.deltaTime;
        _currentScale = Mathf.Clamp(_currentScale, _minScale, _maxScale);
        _batterImageObj.transform.localScale = Vector3.one * _currentScale;
    }
    
    private static Vector2 Rotate(Vector2 from, float angleRad)
    {
        Complex result = new Complex(from.x, from.y) 
                         * new Complex(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        return new Vector2((float) result.Real, (float) result.Imaginary);
    }
}

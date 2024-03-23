using UnityEngine;

public class HotCakePresenter : MonoBehaviour
{
    [SerializeField, Tooltip("入力")] private BatterRotater _batterRotater;
    [SerializeField, Tooltip("表示")] private HotCakeView _hotCakeView;
    [SerializeField] private float _rotateAngleMultiplier = 40f;
    [SerializeField] private float _rotateSpeedMultiplier = 0.5f;

    private void Update()
    {
        _hotCakeView.RotateSpeed = _batterRotater.Velocity * _rotateSpeedMultiplier;
        _hotCakeView.RotateAngle = _batterRotater.Velocity * _rotateAngleMultiplier;
    }
}

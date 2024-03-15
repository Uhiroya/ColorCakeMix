using UnityEngine;

public class HotCakePresenter : MonoBehaviour
{
    [SerializeField, Tooltip("入力")] private BatterRotater _batterRotater;
    [SerializeField, Tooltip("表示")] private HotCakeView _hotCakeView;
    [SerializeField] private float _defaultRotateSpeedMultiplier = 0.1f;

    private void Update()
    {
        if (_batterRotater.CurrentRotateSpeed == _batterRotater.DefaultMixSpeed)
        {
            _hotCakeView.RotateSpeed = Mathf.Abs(_batterRotater.CurrentRotateSpeed) * _defaultRotateSpeedMultiplier;
        }
        else
        {
            _hotCakeView.RotateSpeed = Mathf.Abs(_batterRotater.CurrentRotateSpeed);
        }
    }
}

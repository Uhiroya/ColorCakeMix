using UnityEngine;

public class HotCakePresenter : MonoBehaviour
{
    [SerializeField, Tooltip("入力")] private BatterManager _batterManager;
    [SerializeField, Tooltip("表示")] private HotCakeView _hotCakeView;

    private void Update()
    {
        _hotCakeView.RotateSpeed = Mathf.Abs(_batterManager.CurrentAmountRotation);
    }
}

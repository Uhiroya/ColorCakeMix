using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BatterManager : MonoBehaviour
{
    [Header("Param")]
    [SerializeField, Tooltip("Cookタイムが終わる回転量 1回転2pi")] private float _finishAmountRotation;
    [SerializeField, Tooltip("ベストタイミング開始となりうる最低値")] private float _minBakingBestTime;
    [SerializeField, Tooltip("ベストタイミング開始となりうる最大値")] private float _maxBakingBestTime;
    [SerializeField, Tooltip("ベストタイミングの長さ最低値")] private float _minLengthBestTiming;
    [SerializeField, Tooltip("ベストタイミングの長さ最大値")] private float _maxLengthBestTiming;
    
    [Space(10)]
    [SerializeField] private BatterRotater _batterRotater;
    [SerializeField] private Image _timerGauge;
    [SerializeField] private Image _bestTimingAreaImage;
    [SerializeField] private Slider _amountMixSlider;
    [Space(10)]
    [SerializeField] private Text _amountMixedText;
    [SerializeField] private HotCakeView _hotCakeView;

    /// <summary> Score減点計算のBase満点 </summary>
    private float _baseScore = 500;
    private float _minBestTiming;
    private float _maxBestTiming;
    private float _bakingTimer;
    private float _overTime = 10;
    private float _amountMixed;
    private float _nextMixSoundAmount;
    private float _mixSoundRate = 6.28f; // 1周
    private Vector2 _bestTimeingAreaPosDiff;
    public event Action FinishAction;

    public void InitializeParameter()
    {
        _amountMixed = 0;
        _bakingTimer = 0;
        _nextMixSoundAmount = 0;
    }
    public void InitializeRandomValue()
    {
        _minBestTiming = Random.Range(_minBakingBestTime, _maxBakingBestTime);
        float bestTimingRange = Random.Range(_minLengthBestTiming, _maxLengthBestTiming);
        _maxBestTiming = _minBestTiming + bestTimingRange;
        _timerGauge.fillAmount = 0;
        _bestTimingAreaImage.fillAmount = bestTimingRange / (_maxBestTiming + _overTime);
        _bestTimeingAreaPosDiff = _bestTimingAreaImage.rectTransform.rect.width * _overTime / (_maxBestTiming + _overTime) * Vector3.right;
        _bestTimingAreaImage.rectTransform.anchoredPosition -= _bestTimeingAreaPosDiff;
        _batterRotater.Initialize();
    }
    
    public void BakingUpdate(float deltaTime)
    {
        _bakingTimer += deltaTime;
        var currentRotate = _batterRotater.BatterRotate(deltaTime);
        if (_batterRotater.Velocity > 0 && currentRotate > 0)  //  回すべき方向が右の時に右に回していたら混ざり具合を上げる
        {
            _amountMixed += currentRotate;
        }
        else if(_batterRotater.Velocity < 0 && currentRotate < 0)  //  回すべき方向が左の時に左に回していたら混ざり具合を上げる
        {
            _amountMixed += -currentRotate;
        }
        if (_amountMixedText) _amountMixedText.text = _amountMixed.ToString("0.00");
        
        // ui
        _timerGauge.fillAmount = _bakingTimer / (_maxBestTiming + _overTime);
        _amountMixSlider.value = _amountMixed / _finishAmountRotation;
        _hotCakeView.Progress = _amountMixed / _finishAmountRotation;   //  _amountMixedの値からシェーダーの混ざり具合を調整する

        if (_amountMixed >= _finishAmountRotation || _bakingTimer >= _maxBestTiming + _overTime)
        {
            FinishAction?.Invoke();
            _hotCakeView.Progress = 1f; //  強制的にシェーダーを混ぜ終わらせる。
        }

        if (_nextMixSoundAmount < _amountMixed)
        {
            _nextMixSoundAmount += _mixSoundRate;
            AudioManager.Instance.PlaySe(SeType.RotateCake);
        }
    }

    /// <summary> cook終了スコア加算 </summary>
    public void CalcCookedScore()
    {
        float bakeScore = 0;

        if (_bakingTimer < _minBestTiming)
        {
            bakeScore = _bakingTimer - _minBestTiming;
        }
        else if (_bakingTimer > _maxBestTiming)
        {
            bakeScore = _bakingTimer - _maxBestTiming;
        }

        ResetParam();
        if (InGameManager.Instance) InGameManager.Instance.AddCookedScore((int)(_baseScore - Mathf.Abs(bakeScore) * 50));
    }

    void ResetParam()
    {
        _bestTimingAreaImage.rectTransform.anchoredPosition += _bestTimeingAreaPosDiff;
        _amountMixSlider.value = 0;
        _timerGauge.fillAmount = 0;
        _bakingTimer = 0;
        _amountMixed = 0;
    }
}

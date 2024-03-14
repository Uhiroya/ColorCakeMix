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
    [Space(10)]
    [SerializeField] private Text _amountMixedText;

    /// <summary> Score減点計算のBase </summary>
    private float _baseScore = 10;
    private float _minBestTiming;
    private float _maxBestTiming;
    private float _bakingTimer;
    private float _overTime = 5;
    private float _amountMixed;

    private void Awake()
    {
        OnEnter();
    }

    private void OnEnter()
    {
        _minBestTiming = Random.Range(_minBakingBestTime, _maxBakingBestTime);
        float bestTimingRange = Random.Range(_minLengthBestTiming, _maxLengthBestTiming);
        _maxBestTiming = _minBestTiming + bestTimingRange;
        _timerGauge.fillAmount = 0;
        _bestTimingAreaImage.fillAmount = bestTimingRange / (_maxBestTiming + _overTime);
        _bestTimingAreaImage.rectTransform.position -= 
            _bestTimingAreaImage.rectTransform.rect.width * _overTime / (_maxBestTiming + _overTime) * Vector3.right;
    }
    
    private void Update()
    {
        _bakingTimer += Time.deltaTime;
        _amountMixed += _batterRotater.BatterRotate(Time.deltaTime);
        if (_amountMixedText) _amountMixedText.text = _amountMixed.ToString("0.00");
        
        // ui
        _timerGauge.fillAmount = _bakingTimer / (_maxBestTiming + _overTime);

        if (_amountMixed >= _finishAmountRotation)
        {
            // switch state でOnExitを呼び出す
            OnExit();
        }
    }

    /// <summary> cook終了スコア加算 </summary>
    public void OnExit()
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
        
        InGameManager.Instance.AddJudgeScore((int)(_baseScore - bakeScore));
    }
}

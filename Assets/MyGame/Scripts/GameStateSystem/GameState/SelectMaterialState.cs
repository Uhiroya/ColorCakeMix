using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Takechi.ColorUtility;

/// <summary>
/// 色選択フェーズ
/// </summary>
public class SelectMaterialState : GameStateBase
{
    [SerializeField] private GameObject _materialPanel;
    [SerializeField] private Text _selectMaterialText;
    [SerializeField] private Text _gotoCookText;
    [SerializeField] float _cookTextWaitTime;
    [SerializeField] private HotCakeView _cakeView;
    [SerializeField, Tooltip("Score減点計算のBase満点")] private float _baseScore = 500;
    [SerializeField, Tooltip("減点用のスコアの乗数")] private float _colorScoreMultiplier = 5f;
    
    private CancellationToken _ct;

    private void Awake()
    {
        _materialPanel.SetActive(false);
        _selectMaterialText.gameObject.SetActive(false);
        _gotoCookText.gameObject.SetActive(false);
    }

    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        _materialPanel.SetActive(true);
        _selectMaterialText.gameObject.SetActive(true);
        ColorManager.Instance.PowderPrefabs.ToList().ForEach(go=>go.SetActive(true));   //  色選択ボタンを全てアクティブにする
        SelectColorAsync();
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public override void OnExit()
    {
        _materialPanel.SetActive(false);
        _selectMaterialText.gameObject.SetActive(false);
        _cakeView.Color1 = ColorManager.Instance.SelectMaterials[0];
        _cakeView.Color2 = ColorManager.Instance.SelectMaterials[1];
        _cakeView.Color3 = ColorManager.Instance.SelectMaterials[2];
        AudioManager.Instance.PlaySe(SeType.CookStart);
        ColorManager.Instance.SelectMaterials.Clear();
    }

    private async void SelectColorAsync()
    {
        const int selectColorLimit = 3;
        try
        {
            await UniTask.WaitUntil(() => selectColorLimit <= ColorManager.Instance.SelectMaterials.Count,
                cancellationToken: _ct);
            JudgeMaterial();
            // Let'sCook表示await
            _gotoCookText.gameObject.SetActive(true);
            GameStateMachine.Instance.ChangeNextState(GamePhase.Cook);
            Invoke(nameof(Inactive), 2f);
        }
        catch
        {
            // ignored
        }
    }

    void Inactive() =>  _gotoCookText.gameObject.SetActive(false);

    private void JudgeMaterial()
    {
        var colorManager = ColorManager.Instance;
        Color colorSum = default;
        foreach (var color in colorManager.SelectMaterials)
        {
            colorSum += color;
        }
        colorSum /= 3f;
        var colorScore = _baseScore - ColorUtility.ColorDistance(colorManager.OrderColor, colorSum) * _colorScoreMultiplier;
        InGameManager.Instance.AddColorScore((int)colorScore, colorSum);
        Debug.Log($"ColorScore {colorScore}");
    }
}

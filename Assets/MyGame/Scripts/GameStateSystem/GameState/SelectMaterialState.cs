using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Takechi;
using ColorUtility = Takechi.ColorUtility;

/// <summary>
/// 色選択フェーズ
/// </summary>
public class SelectMaterialState : GameStateBase
{
    [SerializeField] private GameObject _materialPanel;
    [SerializeField] private HotCakeView _cakeView;
    [SerializeField, Tooltip("Score減点計算のBase満点")] float _baseScore = 500;
    [SerializeField, Tooltip("減点用のスコアの乗数")] private float _colorScoreMultiplier = 50f;
    
    private CancellationToken _ct;

    private void Awake()
    {
        _materialPanel.SetActive(false);
    }

    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        _materialPanel.SetActive(true);
        ColorManager.Instance.PowderPrefabs.ToList().ForEach(go=>go.SetActive(true));   //  色選択ボタンを全てアクティブにする
        SelectColorAsync();
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public override void OnExit()
    {
        _materialPanel.SetActive(false);
        _cakeView.Color1 = ColorManager.Instance.SelectMaterials[0];
        _cakeView.Color2 = ColorManager.Instance.SelectMaterials[1];
        _cakeView.Color3 = ColorManager.Instance.SelectMaterials[2];
        ColorManager.Instance.SelectMaterials.Clear();
        // TODO オーダーUIを非アクティブ化
    }

    private async void SelectColorAsync()
    {
        const int selectColorLimit = 3;
        await UniTask.WaitUntil(() => selectColorLimit <= ColorManager.Instance.SelectMaterials.Count,
            cancellationToken: _ct);
        JudgeMaterial();
        GameStateMachine.Instance.ChangeNextState(GamePhase.Cook);
    }

    private void JudgeMaterial()
    {
        var colorManager = ColorManager.Instance;
        Color colorSum = default;
        foreach (var color in colorManager.SelectMaterials)
        {
            colorSum += color;
        }
        colorSum /= 3f;
        var colorScore = ColorUtility.ColorDistance(colorManager.OrderColor, colorSum) * _colorScoreMultiplier;
        InGameManager.Instance.AddColorScore((int)colorScore, colorSum);
        Debug.Log($"ColorScore {colorScore}");
    }

    private void CalcColorScore()
    {
        
    }
}

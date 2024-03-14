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
    
    private HotCakeView _cakeView = new();
    private CancellationToken _ct;

    public override void OnEnter(CancellationToken ct)
    {
        _ct = ct;
        _materialPanel.SetActive(true);
        SelectColorAsync();
        JudgeMaterial();
        GameStateMachine.Instance.ChangeNextState(GamePhase.Cook);
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
    }

    private void JudgeMaterial()
    {
        var colorManager = ColorManager.Instance;
        Color colorSum = default;
        foreach (var color in colorManager.SelectMaterials)
        {
            colorSum += color;
        }

        var colorScore = ColorUtility.ColorDistance(colorManager.OrderColor, colorSum / 3);
        Debug.Log($"ColorScore {colorScore}");
    }
}
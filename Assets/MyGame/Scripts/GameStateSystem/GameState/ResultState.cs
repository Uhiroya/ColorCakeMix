using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト
/// </summary>
public class ResultState : GameStateBase
{
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private GameObject _cakeLayoutGroup;
    [SerializeField] private ResultCakeContller _resultCakePrefab;
    [SerializeField] private Text _totalPriceText;
    
    private CancellationToken _ct;
    
    public override void OnEnter(CancellationToken ct)
    {
        _resultPanel.SetActive(true);
        int totalPrice = 0;

        foreach (var cakeData in InGameManager.Instance.FinishedCakeDatas)
        {
            Instantiate(_resultCakePrefab, _cakeLayoutGroup.transform).InitializeCake(cakeData.Color, cakeData.Price);
            totalPrice += cakeData.Price;
        }

        _totalPriceText.text = totalPrice.ToString();
        AudioManager.Instance.PlaySe(SeType.Result);
        AudioManager.Instance.PlayBGM(BGMType.Result);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameStateMachine.Instance.ChangeNextState(GamePhase.Title);
        }
    }

    public override void OnExit()
    {
        _resultPanel.SetActive(false);
    }
}

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
        Debug.Log("result enter");
        _resultPanel.SetActive(true);
        int totalPrice = 0;

        foreach (var cakeData in InGameManager.Instance.FinishedCakeDatas)
        {
            Instantiate(_resultCakePrefab, _cakeLayoutGroup.transform).InitializeCake(cakeData.Color, cakeData.Price);
            totalPrice += cakeData.Price;
        }

        _totalPriceText.text = totalPrice.ToString();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameStateMachine.Instance.ChangeNextState(GamePhase.Title);
        }
    }

    public override void OnExit()
    {

    }
}

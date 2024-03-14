using System.Threading;
using UnityEngine;

/// <summary>
/// リザルト
/// </summary>
public class ResultState : GameStateBase
{
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private GameObject _cakeLayoutGroup;
    [SerializeField] private ResultCakeContller _resultCakePrefab;
    
    private CancellationToken _ct;
    
    public override void OnEnter(CancellationToken ct)
    {
        _resultPanel.SetActive(true);

        foreach (var cakeData in InGameManager.Instance.FinishedCakeDatas)
        {
            Instantiate(_resultCakePrefab, _cakeLayoutGroup.transform).InitializeCake(cakeData.Color, cakeData.Price);
        }
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

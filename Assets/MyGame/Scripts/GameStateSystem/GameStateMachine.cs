using System.Threading;
using UnityEngine;

public enum GamePhase
{
    Title,
    StartGame,
    Order,
    SelectMaterial,
    Cook,
    Judge,
    EndGame,
    Result,
}
public class GameStateMachine : SingletonMonoBehavior<GameStateMachine>
{
    [SerializeField] private GameStateBase _titleState;
    [SerializeField] private GameStateBase _startGameState;
    [SerializeField] private GameStateBase _orderState;
    [SerializeField] private GameStateBase _selectMaterialState;
    [SerializeField] private GameStateBase _cookState;
    [SerializeField] private GameStateBase _judgeState;
    [SerializeField] private GameStateBase _endGameState;
    [SerializeField] private GameStateBase _resultState;
    
    private static GamePhase _currentPhase;
    private static GameStateBase _currentState;
    private static GameStateBase _nextState;
    private CancellationTokenSource _cts;
    private float _remainingTime;

    void Start()
    {
        _cts = new CancellationTokenSource();
        ChangeNextState(GamePhase.Title);
    }
    
    void Update()
    {
        if (_nextState)
        {
            if (_currentState)
            {
                _currentState.OnExit();
                //Stateから抜ける際に待機処理があればキャンセルする
                _cts.Cancel();
                _cts = new CancellationTokenSource();
            }
            _nextState.OnEnter(_cts.Token);
            _currentState = _nextState;
            _nextState = null;
        }
        if(_currentState) _currentState.OnUpdate(Time.deltaTime);
    }

    public void ChangeNextState(GamePhase nextPhase)
    {
        switch (nextPhase)
        {
            case GamePhase.Title :
                _nextState = _titleState;
                break;
            case GamePhase.StartGame :
                _nextState = _startGameState;
                break;
            case GamePhase.Order :
                _nextState = _orderState;
                break;
            case GamePhase.SelectMaterial :
                _nextState = _selectMaterialState;
                break;
            case GamePhase.Cook :
                _nextState = _cookState;
                break;
            case GamePhase.Judge :
                _nextState = _judgeState;
                break;
            case GamePhase.EndGame :
                _nextState = _endGameState;
                break;
            case GamePhase.Result :
                _nextState = _resultState;
                break;
        }
    }
}


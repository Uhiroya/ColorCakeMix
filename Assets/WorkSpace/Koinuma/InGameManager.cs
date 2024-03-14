using UnityEngine;

public class InGameManager : SingletonMonoBehavior<InGameManager>
{
    [SerializeField] private float _timeLimit;

    private bool _isInGame;
    private float _inGameTimer;
    private int _totalScore;
    private int _colorScore;
    
    /// <summary> ゲーム開始時の初期化 </summary>
    public void InitializeToStart()
    {
        _isInGame = true;
        _totalScore = 0;
        _inGameTimer = _timeLimit;
    }

    private void Update()
    {
        if (!_isInGame) return;
        
        _inGameTimer -= Time.deltaTime;

        if (_inGameTimer <= 0)
        {
            _isInGame = false;
            _inGameTimer = 0;
            // to end game use _totalScore
        }
    }

    public void AddJudgeScore(int score)
    {
        _totalScore += _colorScore;
        _totalScore += score;
    }

    public void AddColorScore(int score)
    {
        _colorScore = score;
    }
}

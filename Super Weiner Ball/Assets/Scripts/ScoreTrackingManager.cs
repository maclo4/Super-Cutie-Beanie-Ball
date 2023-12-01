
public class ScoreTrackingManager : Singleton<ScoreTrackingManager>
{
    private int _playerScore;
    
    public void AddScore(int score)
    {
        _playerScore += score;
        DashboardManager.Instance.SetScoreTextValue(_playerScore);
    }

    public int GetPlayerScore()
    {
        return _playerScore;
    }
}

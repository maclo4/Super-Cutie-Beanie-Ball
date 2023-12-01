using TMPro;
using UnityEngine;

public class DashboardManager : Singleton<DashboardManager>
{
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text speed;
    [SerializeField] TMP_Text timer;
    [SerializeField] Color red;
    private Rigidbody _playerRb;
    private TimerManager _timerManager;

    private void Start()
    {
        _playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        _timerManager = TimerManager.Instance;
    }
    
    public void SetScoreTextValue(int m_score)
    {
        score.text = m_score.ToString();
    }

    private void Update()
    {
        var kph = _playerRb.velocity.magnitude * 2f;
        speed.text = kph.ToString("F0");
        var timerFloat = _timerManager.GetTimer();
        timer.text = timerFloat.ToString("F2").Replace(".", "\n");
        if (timerFloat < 15 && timer.color != red)
        {
            timer.color = red;
        }
    }
}

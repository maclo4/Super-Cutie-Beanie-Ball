using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionManager : Singleton<LevelTransitionManager>
{
    [SerializeField] Image levelTransitionImage;
    [SerializeField] TMP_Text levelNameText;
    
    [SerializeField] private GameObject introCamera;
    [SerializeField] private GameObject outroCamera;
    
    [SerializeField] private bool menuScene;
    
    private static readonly int Intro = Animator.StringToHash("Intro");
    
    private Animator _introCameraAnimator;
    private CinemachineVirtualCamera _outroCameraCm;
    private IntroLookAtController _introLookAtController;
    
    private TimerManager _timerManager;
    
    private Player _player;
    private Rigidbody _playerRb;
    private LevelCompleteAnimationController _levelCompleteAnimationController;

    private void Start()
    {
        if (menuScene)
        {
            return;
        }

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _levelCompleteAnimationController = GameObject.FindWithTag("Player Parent")
            .GetComponentInChildren<LevelCompleteAnimationController>();
        _introCameraAnimator = introCamera.GetComponentInChildren<Animator>();
        _outroCameraCm = outroCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        _introLookAtController = introCamera.GetComponentInChildren<IntroLookAtController>();
        _timerManager = TimerManager.Instance;

        StartCoroutine(FadeInCoroutine());
        StartCoroutine(TransitionIn());
    
        
    }
    
    private IEnumerator TransitionIn()
    {
        _introCameraAnimator.SetTrigger(Intro);

        // Wait one frame to let the animator enter the next animation first
        yield return null;
        
        _introLookAtController.SmoothDampToPlayer( _introCameraAnimator.GetCurrentAnimatorStateInfo(0).length);
        
        
        yield return new WaitUntil(() => 
            _introCameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f);

        levelNameText.text = "";
        
        yield return new WaitUntil(() => 
            _introCameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        yield return new WaitForSeconds(.5f);
        
        _timerManager.SetActive(true);
        _player.EnableInputAndGravity();
    }
    
    public void FastForwardCutscene(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            _introCameraAnimator.speed = 2.5f;
            _introLookAtController.SetSpeedMultiplier(2.5f);
        } 
        else
        {
            _introCameraAnimator.speed = 1;
            _introLookAtController.SetSpeedMultiplier(1);
        }
    }
    public void LoadNextScene()
    {
        StartCoroutine(TransitionOut());
    }

    private IEnumerator TransitionOut()
    {
        _timerManager.SetActive(false);
        _outroCameraCm.m_Priority = 12;
        
        _player.DisableInputAndGravity();
        _player.DisableCollision();
        _levelCompleteAnimationController.BeginSpinning();
        _levelCompleteAnimationController.BeginCelebration();
        
        yield return new WaitForSeconds(2f);

        _outroCameraCm.m_Follow = null;
        _levelCompleteAnimationController.FlyAway();
        
        levelTransitionImage.color = new Color(0,0,0,0);
        while (levelTransitionImage.color.a < 1)
        {
            levelTransitionImage.color = new Color(levelTransitionImage.color.r, levelTransitionImage.color.g, 
                levelTransitionImage.color.b,levelTransitionImage.color.a + .02f);
            yield return new WaitForFixedUpdate();
        }
        
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RestartScene()
    {
        StartCoroutine(RestartSceneCoroutine());
    }
    
    private IEnumerator RestartSceneCoroutine()
    {
        levelTransitionImage.color = new Color(0,0,0,0);
        while (levelTransitionImage.color.a < 1)
        {
            levelTransitionImage.color = new Color(levelTransitionImage.color.r, levelTransitionImage.color.g, 
                levelTransitionImage.color.b,levelTransitionImage.color.a + .02f);
            yield return new WaitForFixedUpdate();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator FadeInCoroutine()
    {
        levelTransitionImage.color = new Color(0,0,0,1);
        while (levelTransitionImage.color.a > 0)
        {
            levelTransitionImage.color = new Color(levelTransitionImage.color.r, levelTransitionImage.color.g, 
                levelTransitionImage.color.b,levelTransitionImage.color.a - .02f);
            yield return new WaitForFixedUpdate();
        }
    }

    public void FadeOutScene()
    {
        StartCoroutine(FadeOutSceneCoroutine());
    }

    public void FadeOutSceneByIndex(int sceneIndex)
    {
        StartCoroutine(FadeOutSceneCoroutine(sceneIndex));
    }
    
    private IEnumerator FadeOutSceneCoroutine(int sceneIndex = -1)
    {
        levelTransitionImage.color = new Color(0,0,0,0);
        while (levelTransitionImage.color.a < 1)
        {
            levelTransitionImage.color = new Color(levelTransitionImage.color.r, levelTransitionImage.color.g, 
                levelTransitionImage.color.b,levelTransitionImage.color.a + .02f);
            yield return new WaitForFixedUpdate();
        }
        
        if(sceneIndex == -1)
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }


    public void TimedOut()
    {
        _player.DisableInputAndGravity();
        
        StartCoroutine(TimedOutCoroutine());
    }
    private IEnumerator TimedOutCoroutine()
    {
        _outroCameraCm.m_Priority = 12;
        
        _player.DisableInputAndGravity();
        _levelCompleteAnimationController.SmoothVelocityToZero();
        
        yield return new WaitForSeconds(.5f);

        RestartScene();
    }
}

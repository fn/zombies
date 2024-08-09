using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zombies;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject MenuActive;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject MenuWin;
    [SerializeField] GameObject MenuLose;

    public GameSettings gameOptions;
    [SerializeField] GameObject MenuOptions;
    [SerializeField] Slider effectSlider;

    public GameObject PromptBackground;
    public TMP_Text PromptText;

    public TMP_Text MoneyText;

    public TMP_Text WaveHudText;
    public TMP_Text AmmoHudText;
    public Image HurtScreen;

    public bool ItemInHand;
    public bool IsPaused;
    float origTimescale;
    int enemyCount;
    
    public Player LocalPlayer;
    public List<BaseZombie> zombieDead = new List<BaseZombie>();

    void Awake()
    {
        Instance = this;
        var scene = SceneManager.GetActiveScene();
        if (scene.name != "TitleScreen")
            LocalPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();

        origTimescale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MenuActive == null)
            {
                StatePause();
                MenuActive = MenuPause;
                MenuActive.SetActive(IsPaused);
            }
            else if (MenuActive == MenuPause)
                StateRun();
        }
        if (LocalPlayer != null)
        {
            MoneyText.SetText($"$ {LocalPlayer.Money}");
        }
    }

    public void LockInput()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
        } else if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void StatePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        LockInput();
    }

    public void StateRun()
    {
        IsPaused = !IsPaused;
        Time.timeScale = origTimescale;

        LockInput();

        if (MenuActive != null)
        {
            MenuActive.SetActive(IsPaused);
            MenuActive = null;
        }
    }

    public void UpdateGameGoal(int amt)
    {
        enemyCount += amt;

        if (enemyCount <= 0)
        {
            StatePause();

            MenuActive = MenuWin;
            MenuActive.SetActive(IsPaused);
        }
    }

    public void StateLose()
    {
        StatePause();

        MenuActive = MenuLose;
        MenuActive.SetActive(IsPaused);
    }

    public void StateWin()
    {
        StatePause();

        MenuActive = MenuWin;
        MenuActive.SetActive(IsPaused);
    }

    public void GameOptions()
    {
        MenuActive = MenuOptions;
        MenuActive.SetActive(IsPaused);
    }

    public void Back()
    {
        MenuActive.SetActive(false);
        MenuActive = MenuPause;
    }

    public void EffectVolume()
    {
        gameOptions.effectVolume = effectSlider.value;
    }
}
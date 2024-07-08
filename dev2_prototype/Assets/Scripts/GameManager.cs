using UnityEngine;
using Zombies;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject MenuActive;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject MenuWin;
    [SerializeField] GameObject MenuLose;

    public Player LocalPlayer;
    
    public bool IsPaused;

    float origTimescale;

    int enemyCount;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
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
    }

    public void StatePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StateRun()
    {
        IsPaused = !IsPaused;
        Time.timeScale = origTimescale;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        MenuActive.SetActive(IsPaused);
        MenuActive = null;
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
}
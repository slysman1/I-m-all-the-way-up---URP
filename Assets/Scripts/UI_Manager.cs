using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    Player player;
    RespawnCage cage;

    [SerializeField] private GameObject TapToStartBTN;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        //cage = GameObject.Find("FirstCage").GetComponent<CageController>();
        Invoke("TapToStart", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TapToStart()
    {
        player.RespawnPlayer();
        // cage.StartAnimation();
        //TapToStartBTN.SetActive(false);
    }



    public void RestartGame()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

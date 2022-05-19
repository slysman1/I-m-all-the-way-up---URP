using UnityEngine;

public class DeadZone : MonoBehaviour
{


    [SerializeField] private float regularSpeed;
    [SerializeField] private float maxSpeed;

    private float speed;
    private Player player;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        speed = regularSpeed;

        InvokeRepeating("CheckForSpeed", 0, 0.7f);
    }

    void Update()
    {
        transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
    }


    private void CheckForSpeed()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 16)
            transform.position = new Vector2(transform.position.x, player.transform.position.y - 13);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}

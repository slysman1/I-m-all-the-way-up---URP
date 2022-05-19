using UnityEngine;

public class Trigger_TrapBox : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject headCollider;


    void Start()
    {
        anim = GetComponent<Animator>();
        headCollider.SetActive(false);
    }

    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("trapBoxClosed");
            Invoke("CloseTheLid", 0.1f);
            Debug.Log("Game over!");
        }

        if (collision.tag == "Cage")
        {
            Debug.Log("cage was here");
            collision.transform.parent.position = new Vector2(-10, 0);
        }
    }

    private void CloseTheLid() => headCollider.SetActive(true);

}

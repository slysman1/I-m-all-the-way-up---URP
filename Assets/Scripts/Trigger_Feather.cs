using UnityEngine;

public class Trigger_Feather : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().feathers++;
            transform.position = new Vector3(-10, 0, 0);
        }
    }

    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * 1 * Time.deltaTime;
    }

}

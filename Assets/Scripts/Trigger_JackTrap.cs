using UnityEngine;

public class Trigger_JackTrap : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed;

    private Vector2 choosenDirection;
    [SerializeField] private Vector2[] knockbackDirection;
    [SerializeField] private float knockbackPower;

    [SerializeField] private float knockbackRadius;
    [SerializeField] private LayerMask whatIsKnockable;
    [SerializeField] private Transform checkkTransform;


    private bool wasActivated;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
    }

    public void SetDirection()
    {
        if (transform.position.x < -1.5f)
        {
            choosenDirection = knockbackDirection[0];
        }
        else if (transform.position.x > -1.5f && transform.position.x < 1.5f)
        {
            choosenDirection = knockbackDirection[1];
        }
        else
        {
            choosenDirection = knockbackDirection[2];
        }

        anim.SetFloat("xDirection", choosenDirection.x);
    }

    private void DoKnockback()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(checkkTransform.position, knockbackRadius, whatIsKnockable);

        foreach (Collider2D collider in detectedObjects)
        {
            var iknockable = collider.GetComponent<IKnockable>();

            if (iknockable != null)
            {
                iknockable.Knockback(choosenDirection, knockbackPower);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasActivated)
        {
            wasActivated = true;

            SetDirection();

            anim.SetTrigger("activate");
            DoKnockback();
        }

        if (collision.tag == "Cage")
        {
            Debug.Log("cage was here");
            collision.transform.parent.position = new Vector2(-10, collision.transform.parent.position.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkkTransform.position, knockbackRadius);
    }

    private void ResetTrap()
    {
        wasActivated = false;
    }
}

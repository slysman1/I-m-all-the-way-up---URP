using UnityEngine;
using System.Collections;

public class CageImpactController : MonoBehaviour, IImpactable
{
    private MeshRenderer mesh;
    private Rigidbody2D rb;
    private BoxCollider2D cd;
    private Player player;

    [SerializeField] private float impactCooldown = 1.5f;
    private float impactCountdown;

    void Start()
    {
        player = Player.instance.GetComponent<Player>();
        mesh = GetComponentInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<BoxCollider2D>();

        StartCoroutine(MeshCheckCoroutine());
    }

    void Update()
    {
        impactCooldown -= 1 * Time.deltaTime;
    }


    private IEnumerator MeshCheckCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        MeshCheck();
        StartCoroutine(MeshCheckCoroutine());
    }

    private void MeshCheck()
    {
        bool playerNearBy = Vector2.Distance(player.transform.position, transform.position) < 20;
        mesh.enabled = playerNearBy;

    }


    public void TakeImpact(float x)
    {
        if (impactCountdown <= 0)
        {
            rb.velocity = new Vector2(x, rb.velocity.y);
            impactCountdown = impactCooldown;
        }
    }
    public float myTopPoint() => cd.bounds.max.y;

    public float myXposition() => transform.position.x;

}

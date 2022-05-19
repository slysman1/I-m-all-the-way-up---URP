using UnityEngine;

public class Props : MonoBehaviour, IImpactable
{
    private Rigidbody2D rb;
    [SerializeField] private float impactMultiplier;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    public void TakeImpact(float power)
    {
        float randomValue = Random.Range(-1f, 5.5f);

        rb.AddForce(new Vector2(0, power * impactMultiplier + randomValue));
        Debug.Log("I was impacted" + this.gameObject);
    }



    public float myTopPoint() => 1;
    public float myXposition() => transform.position.x;
}

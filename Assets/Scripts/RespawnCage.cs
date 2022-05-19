using UnityEngine;

public class RespawnCage : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartAnimation() => anim.SetTrigger("animBegin");
    public void SelfDestroy() => Destroy(this.gameObject);
}

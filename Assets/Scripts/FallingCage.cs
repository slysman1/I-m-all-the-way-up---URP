using UnityEngine;

public class FallingCage : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject feather;
    [SerializeField] private float chanceToSpawnFather;



    private int myId;

    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
    }

    public void SetupCage(bool canSpawnFeather, int playersMilestone)
    {
        Vector3 offset = new Vector3(0, 0.3f, 0);

        if (canSpawnFeather)
            ObjectPool.instance.SpawnFromPool("Feather", transform.position + offset, transform.rotation);

        myId = Random.Range(0, transform.childCount + playersMilestone);



        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }


        GameObject choosenCage = transform.GetChild(myId).gameObject;
        choosenCage.SetActive(true);
    }
}

using UnityEngine;

public class WallDecorGenerator : MonoBehaviour
{
    [SerializeField] private float spawnTransformZ;

    [SerializeField] private Transform propsParent;
    [SerializeField] private GameObject[] prop;

    private void Start()
    {
        GenerateProp();
    }

    private void GenerateProp()
    {
        for (int i = 0; i < 500; i++)
        {
            int randomProp = Random.Range(0, prop.Length);
            float xPosition = Random.Range(-5.25f, 6.75f);
            float yPosition = Random.Range(1.5f, 3f);

            Vector3 spawnPos = new Vector3(xPosition, transform.position.y + yPosition, propsParent.position.z);
            //Vector3 spawnPosOffset = new Vector3(xPosition, yPosition,0);

            GameObject newProp = Instantiate(prop[randomProp], spawnPos, Quaternion.identity,propsParent);


            transform.position = new Vector2(transform.position.x, transform.position.y + 10);
        }

    }
}

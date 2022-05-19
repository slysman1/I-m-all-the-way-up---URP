using UnityEngine;

public class Goose : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Vector3[] startPos;
    [SerializeField] private Vector3[] movementDirection;
    [SerializeField] private Vector3[] rotation;

    private int choosenWay;
    private void Start()
    {
        Vector3 offset = new Vector3(0, Player.instance.transform.position.y, 0);
        choosenWay = Random.Range(0, startPos.Length);

        transform.Rotate(rotation[choosenWay]);
        transform.position = startPos[choosenWay] + offset;
    }

    private void Update()
    {
        transform.position +=
            new Vector3(movementDirection[choosenWay].x, movementDirection[choosenWay].y, movementDirection[choosenWay].z) * speed * Time.deltaTime;
    }
}

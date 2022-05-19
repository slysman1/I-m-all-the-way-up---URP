using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();



        if (Random.Range(0, 100) > 35)
            mesh.enabled = false;
    }
}

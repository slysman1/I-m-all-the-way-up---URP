using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDecor : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = Player.instance.GetComponent<Player>();
        InvokeRepeating("CheckForSelfDelete", 0, 1f);
    }

    private void CheckForSelfDelete()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > 50)
        {
            Destroy(this.gameObject);
        }
    }
}

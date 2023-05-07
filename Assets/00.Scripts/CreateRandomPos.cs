using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomPos : MonoBehaviour
{
    [SerializeField]
    Transform[] positions;

    [SerializeField]
    GameObject pool;

    int beforePosIndx = -1;
    int idx;
    public Transform CreatePrefab()
    {
        while (idx == beforePosIndx)
            idx = Random.Range(0, positions.Length);

        Transform prefab = pool.transform.GetChild(0);
        prefab.SetParent(null, true);
        prefab.position = positions[idx].position;
        prefab.rotation = Quaternion.identity;

        return prefab;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _goose;

    public const float walkSpeed = 0.03f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _goose.position = new Vector3(_goose.position.x + walkSpeed, _goose.position.y, _goose.position.z);
    }
}

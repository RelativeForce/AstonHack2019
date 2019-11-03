using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GooseLogic : MonoBehaviour
{
    public const float WalkSpeed = 0.03f;
    public const float Boundary = 5f;
    public static Random random = new Random();

    [SerializeField]
    private Transform _goose;
    [SerializeField]
    private AudioSource _audio;

    private Vector3 _velocity;
    private bool _isAlive;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        SetVelocity(RandomVelocity() * Direction(), RandomVelocity() * Direction());
        _isAlive = true;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            float xVelocity = FixVelocity(_velocity.x, _goose.position.x);

            float zVelocity = FixVelocity(_velocity.z, _goose.position.z);

            SetVelocity(xVelocity, zVelocity);

            UpdateRotation();

            UpdatePosition();
        }
    }

    private static float FixVelocity(float currentVelocity, float currentPos)
    {
        if (Math.Abs(currentPos + currentVelocity) < Boundary)
        {
            return currentVelocity;
        }

        var v = RandomVelocity();

        if (currentVelocity > 0)
        {
            return v  * -1;
        }

        return v;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bat" && _isAlive)
        {
            _isAlive = false;
            _animator.SetBool("isDead", true);
            _goose.rotation = Quaternion.Euler(0, 90 * random.Next(0, 3), 90);
            _audio.Play();
        }
    }

    private void UpdatePosition()
    {
        _goose.position = new Vector3(_goose.position.x + _velocity.x, _goose.position.y, _goose.position.z + _velocity.z);
    }

    private void UpdateRotation()
    {
        var angle = (float) (((Math.Atan(_velocity.x / _velocity.z) * 180) / Math.PI )) + (_velocity.z < 0 ? 180 : 0);

        _goose.rotation = Quaternion.Euler(0, angle, 0);

    }

    private void SetVelocity(float x, float z)
    {
        _velocity = new Vector3(x, 0f, z);
    }

    private static int Direction()
    {
        return (random.Next(1) == 1 ? -1 : 1);
    }

    private static float RandomVelocity()
    {
        var v = random.NextDouble() * WalkSpeed;

        return (float) v;
    }
}

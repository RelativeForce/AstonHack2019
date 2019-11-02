using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BossGooseLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _goose;
    [SerializeField]
    private AudioSource _roarAudio;
    [SerializeField]
    private AudioSource _deathAudio;

    public static Random random = new Random();

    private Animator _animator;
    private bool _isAlive;
    private bool _isEnraged;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isAlive = true;
        _isEnraged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (!_isEnraged)
            {
                var aliveGeese = GameObject.FindGameObjectsWithTag("Small Goose")
                    .Select(o => o.GetComponent<Animator>())
                    .Any(a => !a.GetBool("isDead"));

                if (!aliveGeese)
                {
                    _animator.SetBool("isEnraged", true);
                    _isEnraged = true;
                    _roarAudio.Play();
                }
            }
            else
            {
                var player = GameObject.FindGameObjectsWithTag("Player").First();

                var playerPosition = player.transform.position;

                var velocity = (playerPosition - _goose.position) / 100;

                velocity.y = 0;

                if (Math.Sqrt((velocity.x * velocity.x) + (velocity.z * velocity.z)) < 0.02)
                {
                    player.SendMessage("Die");
                }

                _goose.position += velocity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bat" && _isAlive && _isEnraged)
        {
            _isAlive = false;
            _animator.SetBool("isDead", true);
            _goose.rotation = Quaternion.Euler(0, 90 * random.Next(0, 3), 90);

            _deathAudio.Play();
        }
    }
}

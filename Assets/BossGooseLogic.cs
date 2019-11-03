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
    [SerializeField]
    private AudioSource _fightAudio;

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
                    _fightAudio.PlayDelayed(3);
                }
            }
            else
            {
                var player = GameObject.FindGameObjectsWithTag("Player").First();

                var playerPosition = player.transform.position;

                var difference = playerPosition - _goose.position;

                var velocity = (difference).normalized;

                velocity *= 0.02f;

                velocity.y = 0;

                if (Math.Sqrt((difference.x * difference.x) + (difference.z * difference.z)) < 3)
                {
                    player.SendMessage("Die");
                    _fightAudio.Stop();
                    Destroy(gameObject);
                }

                _goose.position += velocity;

                var angle = (float)(((Math.Atan(velocity.x / velocity.z) * 180) / Math.PI)) + (velocity.z < 0 ? 180 : 0);

                _goose.rotation = Quaternion.Euler(0, angle, 0);
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
            _fightAudio.Stop();
        }
    }
}

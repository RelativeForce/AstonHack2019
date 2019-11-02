using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossGooseLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _goose;
    [SerializeField]
    private AudioSource _audio;

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
                    _audio.Play();
                }
            }
            else
            {
                var player = GameObject.FindGameObjectsWithTag("Player").First();

                var playerPosition = player.transform.position;

                var velocity = (playerPosition - _goose.position) / 100;

                velocity.y = 0;

                _goose.position += velocity;
            }
        }
    }
}

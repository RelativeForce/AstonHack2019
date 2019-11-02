using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private AudioSource _deathAudio;

    private bool _isDead;

    // Start is called before the first frame update
    void Start()
    {
        _isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        if (!_isDead)
        {
            _deathAudio.Play();
            _isDead = true;
        }
    }
}

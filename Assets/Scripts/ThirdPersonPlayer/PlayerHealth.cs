using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable<int>, IKillable
{
    public event Action Died = delegate { };
    public event Action<int> TookDamage = delegate { };

    [SerializeField] int _maxHp = 50;
    private int _hp;
    public int HP
    {
        get
        {
            return _hp;
        }
        private set
        {
            if (value > _maxHp)
                _hp = _maxHp;
            else if (value <= 0)
                _hp = 0;
            else
                _hp = value;
        }
    }

    public void Damage(int damageTaken)
    {
        HP -= damageTaken;
        TookDamage?.Invoke(HP);
        if (HP <= 0)
            Kill();
    }

    public void Kill()
    {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        _hp = _maxHp;
    }
}

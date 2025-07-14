using System;
using UnityEngine;

public class HealthComponent : EntityComponent
{
    // Regen delay when taking damage
    private const float HealthRegenDelayTime = 3;

    public event Action<int> OnHealthChanged;

    public event Action OnDead;

    [SerializeField]
    private float _maxHealth, _currentHealth, _healthRegenPerSecond;

    [SerializeField]
    private bool _isDead;

    private float _healthRegenDelayTimer;

    [SerializeField] private int demoTakenDamage;

    public void SetMaxHealth(float value)
    {
        _maxHealth = value;
        _currentHealth = value;
    }

    public void SetHealthRegenPerSecond(float value)
    {
        _healthRegenPerSecond = value;
    }

    public void SetCurrentHealth(int value)
    {
        _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
    }

    public void ChangeCurrentHealth(float changeValue)
    {
        if (IsDead())
        {
            return;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + changeValue, 0, _maxHealth);

        if (changeValue <= 0)
        {
            _healthRegenDelayTimer = HealthRegenDelayTime;
        }

        if (_currentHealth <= 0)
        {
            _isDead = true;
            OnDead?.Invoke();
        }

        if (changeValue != 0)
        {
            OnHealthChanged?.Invoke((int)_currentHealth);
        }
    }

    public void Regenerate()
    {
        if (_isDead)
        {
            return;
        }

        if (_healthRegenDelayTimer > 0)
        {
            _healthRegenDelayTimer = Mathf.Clamp(_healthRegenDelayTimer - Time.deltaTime, 0, HealthRegenDelayTime);
        }
        else
        {
            if (_currentHealth >= _maxHealth)
            {
                return;
            }

            float regenAmount = _healthRegenPerSecond * Time.deltaTime;
            ChangeCurrentHealth(regenAmount);
        }
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void RefreshHealth()
    {
        ChangeCurrentHealth((int)_maxHealth);
        _isDead = false;
    }

    public float GetCurrentHealthPercent()
    {
        return _currentHealth / _maxHealth;
    }

    [ContextMenu("Test Dead")]
    public void TestDead()
    {
        ChangeCurrentHealth(-demoTakenDamage);
    }

    private void Update()
    {
        Regenerate();
    }
}
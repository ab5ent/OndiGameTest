using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;

    [SerializeField] private SpriteRenderer healthBarBackgroundSpriteRenderer;

    [SerializeField] private SpriteRenderer healthBarSpriteRenderer;

    private float _spriteRendererWidth;

    private float timer = 0;

    private void Awake()
    {
        _spriteRendererWidth = healthBarSpriteRenderer.sprite.rect.width / healthBarSpriteRenderer.sprite.pixelsPerUnit;
        healthBarBackgroundSpriteRenderer.gameObject.SetActive(false);
        healthComponent.OnHealthChanged += ShowHealthBar;
    }

    private void OnDestroy()
    {
        healthComponent.OnHealthChanged -= ShowHealthBar;
    }

    private void ShowHealthBar(int currentHealth)
    {
        if (healthComponent.IsDead())
        { 
            healthBarBackgroundSpriteRenderer.gameObject.SetActive(false);
        }
        else
        {
            timer = 3;
            healthBarBackgroundSpriteRenderer.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                healthBarBackgroundSpriteRenderer.gameObject.SetActive(false);
            }
        }

        float value = healthComponent.GetCurrentHealthPercent();
        float newScaleX = value;

        float offsetX = (_spriteRendererWidth * (1f - newScaleX)) / 2f;

        healthBarSpriteRenderer.transform.localPosition = new Vector3(-offsetX, 0, 0);
        healthBarSpriteRenderer.transform.localScale = new Vector3(newScaleX, 1, 1);
    }
}
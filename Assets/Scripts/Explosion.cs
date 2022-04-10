using System;
using UnityEngine;

using DG.Tweening;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Color _startColor = Color.white;
    [SerializeField] private Color _endColor = Color.red;
    [SerializeField] private float _animDuration = .3f;
    [SerializeField] private Ease _animEase;
    [SerializeField] private float _delay0 = .5f;
    [SerializeField] private float _delay1 = .1f;
    [SerializeField] private float _delay2 = .07f;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
    // _spriteRenderer.DOColor(_endColor, _animDuration).SetEase(_animEase).SetLoops(2, LoopType.Yoyo);
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(new Vector3(4.0f, 4.0f, 1.0f), _animDuration));
            seq.AppendInterval(_delay0);
            seq.Append(_spriteRenderer.DOColor(_startColor, 0));
            seq.AppendInterval(_delay1);
            seq.Append(_spriteRenderer.DOColor(_endColor, _animDuration).SetEase(_animEase));
            seq.AppendInterval(_delay2);
            seq.Append(_spriteRenderer.DOFade(0, _animDuration).SetEase(_animEase));
            seq.OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        // Return to object pool
        GameManager.instance.AddToPool(TypeOfPool.EXPLOSION, gameObject);
    }
}
using System;
using UnityEngine;

using DG.Tweening;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Color _startColor = Color.white;
    [SerializeField] private Color _endColor = Color.red;
    [SerializeField] private float _animDuration = .3f;
    [SerializeField] private Ease _animEase;
    [SerializeField] private float _delay0 = .01f;
    [SerializeField] private float _delay1 = .1f;
    [SerializeField] private float _delay2 = .07f;
    
    private SpriteRenderer _spriteRenderer;
    private Sequence _seq;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
    }

    private void OnEnable()
    {
        _seq = DOTween.Sequence();
        Debug.Log("OnEnable");
        // _spriteRenderer.DOColor(_endColor, _animDuration).SetEase(_animEase).SetLoops(2, LoopType.Yoyo);
        _seq.Append(transform.DOScale(new Vector3(4.0f, 4.0f, 1.0f), .15f));
        _seq.AppendInterval(_delay0);
        _seq.Append(_spriteRenderer.DOColor(_startColor, 0));
        _seq.AppendInterval(_delay1);
        _seq.Append(_spriteRenderer.DOColor(_endColor, _animDuration).SetEase(_animEase));
        _seq.AppendInterval(_delay2);
        _seq.Append(_spriteRenderer.DOFade(0, _animDuration).SetEase(_animEase));
        _seq.OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        _seq.Kill();
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        _spriteRenderer.color = _startColor;
        // Return to object pool
        GameManager.instance.AddToPool(TypeOfPool.EXPLOSION, gameObject);
    }
}
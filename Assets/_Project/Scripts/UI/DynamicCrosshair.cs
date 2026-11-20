using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float _defaultScale = 1f;
    [SerializeField] private float _expandedScale = 1.2f;
    [SerializeField] private float _smoothSpeed = 12f;

    [Header("Aim")]
    [SerializeField] private float _maxOffset = 12f;
    [SerializeField] private float _aimSmoothSpeed = 10f;

    [Header("Colors")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _maxChargeColor = Color.red;

    [SerializeField] private Image _dot;

    private RectTransform _rect;

    private float _scale;
    private float _targetScale;
    private Color _targetColor;
    private Vector2 _targetOffset;

    private void Awake()
    {
        _rect = (RectTransform)transform;

        _scale = _targetScale = _defaultScale;
        _targetColor = _defaultColor;
        _rect.localScale = Vector3.one * _scale;
        _rect.anchoredPosition = Vector2.zero;
    }
    
    private void OnEnable()
    {
        BallState.OnLocalHoldChanged += HandleHoldChanged;
    }
    
    private void OnDisable()
    {
        BallState.OnLocalHoldChanged -= HandleHoldChanged;
    }
    
    private void HandleHoldChanged(bool isHolding)
    {
        if (isHolding)
            Expand();
        else
            ResetCrosshair();
    }

    private void Update()
    {
        float t = Time.deltaTime * _smoothSpeed;

        _scale = Mathf.Lerp(_scale, _targetScale, t);
        _rect.localScale = Vector3.one * _scale;
        _dot.color = Color.Lerp(_dot.color, _targetColor, t);

        _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, _targetOffset, Time.deltaTime * _aimSmoothSpeed);
    }

    public void Expand()
    {
        _targetScale = _expandedScale;
        _targetColor = _defaultColor;
    }

    public void SetCharge(float progress)
    {
        _targetScale = Mathf.Lerp(_expandedScale, _defaultScale, progress);
        _targetColor = Color.Lerp(_defaultColor, _maxChargeColor, progress);
    }

    public void SetAim(Vector2 aim)
    {
        _targetOffset = Vector2.ClampMagnitude(aim, 1f) * _maxOffset;
    }

    public void ResetCrosshair()
    {
        _targetScale = _defaultScale;
        _targetColor = _defaultColor;
        _targetOffset = Vector2.zero;
    }
}
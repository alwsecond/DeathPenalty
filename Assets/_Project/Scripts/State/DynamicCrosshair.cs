using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public static DynamicCrosshair Instance { get; private set; }

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
    [SerializeField] private Image _circle;

    private RectTransform _rect;

    private float _scale;
    private float _targetScale;
    private Color _targetColor;
    private Vector2 _targetOffset;

    private void Awake()
    {
        Instance = this;
        _rect = (RectTransform)transform;

        _scale = _targetScale = _defaultScale;
        _targetColor = _defaultColor;
        _rect.localScale = Vector3.one * _scale;
        _rect.anchoredPosition = Vector2.zero;
        _circle.enabled = false;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
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
        _circle.enabled = true;
    }

    public void ResetCrosshair()
    {
        _targetScale = _defaultScale;
        _targetColor = _defaultColor;
        _targetOffset = Vector2.zero;
        _circle.enabled = false;
    }
}
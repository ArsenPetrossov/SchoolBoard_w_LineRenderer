using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _lineRendererPrefab;
    [SerializeField] private GameObject _mouseTagPrefab;
    [SerializeField] private Collider2D _drawingAreaCollider;

    private Color _color;
    private bool IsColorSelected = false;

    private List<Vector3> _mousePositionList;
    private List<LineRenderer> _lineList;
    private LineRenderer _currentLineRenderer;

    private GameObject _mouseTag;

    void Awake()
    {
        _mousePositionList = new List<Vector3>();
        _lineList = new List<LineRenderer>();
        _mouseTag = CreateMouseTag();
    }

    void Update()
    {
        if (IsColorSelected)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (!_drawingAreaCollider.OverlapPoint(mousePosition))
            {
                return;
            }

            _mouseTag.transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                StartNewLine();
            }

            if (Input.GetMouseButton(0))
            {
                if (_mousePositionList.Count == 0 || Vector3.Distance(_mousePositionList[^1], mousePosition) > 0.1f)
                {
                    _mousePositionList.Add(mousePosition);
                    _currentLineRenderer.positionCount = _mousePositionList.Count;
                    _currentLineRenderer.SetPosition(_mousePositionList.Count - 1, mousePosition);
                }
            }
        }
    }


    public void ClearAllLines()
    {
        foreach (var lineRenderer in _lineList)
        {
            Destroy(lineRenderer.gameObject);
        }

        _lineList.Clear();
    }

    public void SetDrawingColor(Color color)
    {
        _color = color;
        IsColorSelected = true;
        if (_mouseTag != null)
        {
            _mouseTag.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void StartNewLine()
    {
        _mousePositionList.Clear();
        _currentLineRenderer = Instantiate(_lineRendererPrefab, transform).GetComponent<LineRenderer>();
        SetDefaultProperties(_currentLineRenderer);
        _lineList.Add(_currentLineRenderer);
    }

    private GameObject CreateMouseTag()
    {
        return  Instantiate(_mouseTagPrefab, transform.position, Quaternion.identity, this.transform);
    }

    private void SetDefaultProperties(LineRenderer lineRenderer)
    {
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = _color;
        lineRenderer.endColor = _color;
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = _lineList.Count;
    }
}
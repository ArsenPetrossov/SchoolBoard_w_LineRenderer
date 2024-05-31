using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chalk : MonoBehaviour
{
    private Color _color;
    private Button button;
    [SerializeField] private DrawBehavior _drawBehavior;

    void Start()
    {
        _color = GetComponent<Image>().color;
        button = GetComponent<Button>();
        Debug.Log(gameObject.name + " " + _color);

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        _drawBehavior.SetDrawingColor(_color);
    }
}

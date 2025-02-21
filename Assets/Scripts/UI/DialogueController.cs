using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] float showSpeed = 1;
    [SerializeField] int fontSize = 16;
    [SerializeField] int boardSize = 16;
    [SerializeField] int maxLineLength = 40;
    [SerializeField] TMP_Text text;
    Image _image;
    Vector2 _targetSize;
    float _process = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<Image>();
        ShowDialog("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");
    }

    // Update is called once per frame
    void Update()
    {
        if (_image.rectTransform.sizeDelta == _targetSize) return;
        if (_process > 0.99) _image.rectTransform.sizeDelta = _targetSize;

        _image.rectTransform.sizeDelta = Vector2.Lerp(_image.rectTransform.sizeDelta, _targetSize, _process);
        _process += Time.deltaTime * showSpeed;
    }

    void ShowDialog(string dialog)
    {
        Debug.Log($"msg length is {dialog.Length}");
        int width = 0;
        int height = 0;
        // calculat dialogue size
        if (dialog.Length > maxLineLength) width = maxLineLength;
        else width = dialog.Length;
        height = dialog.Length / maxLineLength + 1;
        width = width * fontSize + boardSize * 2;
        height = height * fontSize + boardSize * 2;
        SetSize(width, height);
        text.text = dialog;

    }

    void SetSize(float x, float y)
    {
        _targetSize = new Vector2(x, y);
        _process = 0;
    }
}

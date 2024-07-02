using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomCursor : MonoBehaviour
{
    // Start is called before the first frame update
    public Image cursorImage; // UI 이미지로 커서 설정
    public RectTransform canvasRectTransform; // 커서가 위치할 캔버스

    void Start()
    {
        Cursor.visible = false; // 시스템 커서 숨기기
    }

    void Update()
    {
        Vector2 cursorPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            null,
            out cursorPosition
        );
        cursorImage.rectTransform.anchoredPosition = cursorPosition;
    }
}

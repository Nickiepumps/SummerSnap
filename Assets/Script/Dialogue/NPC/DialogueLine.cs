using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string text;      // ข้อความ
    public bool isChoice;               // ถ้า true จะขึ้น Choice
    public string[] choices;            // ตัวเลือกของ Choice
    public int[] nextLineIndices;       // index ของข้อความต่อไป

    // แสดงชื่อ element สั้น ๆ ใน Inspector
    public override string ToString()
    {
        return string.IsNullOrEmpty(text) ? "Dialogue Line" : text.Substring(0, Mathf.Min(text.Length, 20));
    }
}

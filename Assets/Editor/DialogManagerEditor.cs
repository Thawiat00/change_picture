using UnityEditor;  // ต้องใช้ UnityEditor เพื่อสร้าง Editor script
using UnityEngine;

[CustomEditor(typeof(DialogManager))]
public class DialogManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // แสดง UI ปกติของ Inspector
        DrawDefaultInspector();

        // สร้าง instance ของ DialogManager
        DialogManager dialogManager = (DialogManager)target;

        // ปุ่มที่จะเรียกฟังก์ชัน AddNewDialogFromInspector
        if (GUILayout.Button("Add New Dialog From Inspector"))
        {
            dialogManager.AddNewDialogFromInspector(); // เรียกใช้ฟังก์ชัน
        }
    }
}
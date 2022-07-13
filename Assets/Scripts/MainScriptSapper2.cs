//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MainScripsSapper2 : MonoBehaviour
//{
//    [SerializeField]
//    int xCount = 16;
//    [SerializeField]
//    int yCount = 10;
//    [SerializeField]
//    GameObject cellPrefab;


//    void Start()
//    {
//        for (int i = 0; i < xCount; i++)
//            for (int j = 0; j < yCount; j++)
//            {
//                GameObject obj = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
//                RectTransform rt = obj.GetComponent<RectTransform>();
//                rt.anchorMin = new Vector2 (i * 1.0f / xCount, j * 1.0f / yCount);
//                rt.anchorMax = new Vector2 ((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
//                rt.offsetMin = Vector2.zero;
//                rt.offsetMax = Vector2.zero;
//            }
//    }

//    void Update()
//    {

//    }
//}

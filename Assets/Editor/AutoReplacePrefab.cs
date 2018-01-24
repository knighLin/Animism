using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


/// <summary>
/// 該工具是用來協助 Replace Prefab 的, 
/// 因為該工具引用到 UnityEditor, 
/// 所以必須存放在 Editor 資料夾內. 
/// </summary>
public class AutoReplacePrefab
{
    [MenuItem("MyTools/ReplacePrefab/ConnectToPrefab")]
    private static void ReplacePrefab_ConnectToPrefab()
    {
        // 該陣列是存放已經執行過 Replace 的 GameObject, 
        // 目的是避免同一個的 GameObject 執行了多次 Replace, 
        // 造成系統資源的浪費. 
        List<GameObject> listGameObject = new List<GameObject>();


        // 開始執行 Replace, 這邊程式也會自動判斷是否重複 Replace 同一個物件, 
        // 是的話則會跳開, 避免資源浪費. 
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            // 找 GameObject 最在場景中的源頭父層, 
            // 這邊其實也可以使用 PrefabUtility.FindPrefabRoot 去得取物件的源頭, 
            // 但 PrefabUtility.FindPrefabRoot 有時候會因為物件與父層的連結斷開, 
            // 導致搜尋結果是錯誤的, 所以這邊我們自己手動搜尋. 
            GameObject gobjRoot = Selection.objects[i] as GameObject;
            while (true)
            {
                GameObject gobj = PrefabUtility.GetPrefabParent(gobjRoot) as GameObject;

                if (gobj != null && gobj == PrefabUtility.FindPrefabRoot(gobj))
                    break;

                if (gobjRoot.transform.parent == null)
                    break;

                gobjRoot = gobjRoot.transform.parent.gameObject;
            }

            // 檢查該 GameObject 是否存在紀錄中, 若回傳值不為 -1, 
            // 則表示該 GameObject 已經執行過 Replace 了, 
            // 所以跳過該次迴圈, 不重複 Replace. 
            if (listGameObject.FindIndex(d => d.GetHashCode() == gobjRoot.GetHashCode()) != -1)
                continue;

            // 若該物件無 Prefab 或者 Prefab Root 不正確的話,
            // 也跳出迴圈, 不繼續執行. 
            GameObject target = PrefabUtility.GetPrefabParent(gobjRoot) as GameObject;
            if (target == null || (target != PrefabUtility.FindPrefabRoot(target)))
                continue;

            // 紀錄已經執行過 Replace 的 GameObjec
            listGameObject.Add(gobjRoot);

            // 開始覆蓋 Prefab
            PrefabUtility.ReplacePrefab(gobjRoot, target, ReplacePrefabOptions.ConnectToPrefab);

            // 以下 Log 是可以關閉的
            Debug.Log(string.Format("{0} 使用 {1} 的方式覆寫了 {2}", gobjRoot.name, ReplacePrefabOptions.ConnectToPrefab.ToString(), target.name));
            Debug.Log(string.Format("GameObject {0} 位置", gobjRoot.name), gobjRoot);
            Debug.Log(string.Format("Prefab {0} 位置", target.name), target);
            Debug.Log("");
        }
    }
}
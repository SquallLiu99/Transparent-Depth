using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class CombineMeshEditor : EditorWindow
{
    Transform root;

    [MenuItem("Window/Combine Mesh")]
    static void Init()
    {
        GetWindow<CombineMeshEditor>();
    }

    private void OnGUI()
    {
        root = EditorGUILayout.ObjectField("Root:", root, typeof(Transform), true) as Transform;
        if (!root)
        {
            return;
        }

        if (GUILayout.Button("Combine"))
        {
            List<CombineInstance> ciList = new List<CombineInstance>();
            MeshFilter[] mfs = root.GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i < mfs.Length; i++)
            {
                for (int j = 0; j < mfs[i].sharedMesh.subMeshCount; j++)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = mfs[i].sharedMesh;
                    ci.transform = mfs[i].transform.localToWorldMatrix;
                    ci.subMeshIndex = j;
                    ciList.Add(ci);
                }
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.name = root.name + "_Combined";
            combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedMesh.CombineMeshes(ciList.ToArray(), true, true, false);

            AssetDatabase.CreateAsset(combinedMesh, "Assets/" + combinedMesh.name + ".asset");
            AssetDatabase.SaveAssets();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            ciList.Clear();
        }
    }
}

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
public class GutesUtilityWindow : EditorWindow
{
    string[] scenesToLoad;
    int index;

    Scene[] offeneScenes;

    GameObject canvas;
    GameObject splashScreen;

    [MenuItem("Window/UtilityWindow")]
    public static void ShowWindow()
    {
        GetWindow<GutesUtilityWindow>("UtilityWindow");
    }
    private void OnGUI()
    {
        GUILayout.Label("Utility Window", EditorStyles.boldLabel);
        try
        {
            //    if (!Application.isPlaying)
            //    {
            //        offeneScenes = new Scene[SceneManager.sceneCount];
            //        scenesToLoad = new string[SceneManager.sceneCount];
            //        for (int i = 0; i < offeneScenes.Length; i++)
            //        {
            //            offeneScenes[i] = SceneManager.GetSceneAt(i);
            //            scenesToLoad[i] = offeneScenes[i].name;
            //        }

            //        // Popup Menu das alle Scenen anzeigt. Dort kann man die zu ladende Scene auswaehlen
            //        index = EditorGUILayout.Popup(index = EditorPrefs.GetInt("SceneIndex"), scenesToLoad);
            //        if (index != EditorPrefs.GetInt("SceneIndex"))
            //        {
            //            EditorPrefs.SetInt("SceneIndex", index);
            //        }
            //        if (index <= scenesToLoad.Length)
            //        {
            //            PlayerPrefs.SetString("currentScene", scenesToLoad[index]);
            //        }
            //    }
            if (!Application.isPlaying)
            {
                // Button der Master Scene aktiv setzt und dann Play startet
                GUI.backgroundColor = new Color(0, .9f, 0f, .42f);
                float buttonHeight = 60f;

                if (GUILayout.Button("Start Game", GUILayout.Height(buttonHeight)))
                {
                    for (int i = 0; i < SceneManager.sceneCount; i++)
                    {
                        if (SceneManager.GetSceneAt(i).name == "TD Level")
                        {
                            EditorSceneManager.CloseScene(SceneManager.GetSceneAt(i), false);
                        }
                    }

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
                    EditorApplication.isPlaying = true;
                }
            }
            GUILayout.Space(8f);
            GUI.backgroundColor = new Color(0, .7f, .7f, .5f);

            // Button der Player in die Mitte des Scene View verschiebt
            if (GUILayout.Button("Move Player"))
            {
                Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
                SceneView sv = SceneView.lastActiveSceneView;
                sv.MoveToView(playerTrans);
                playerTrans.position += sv.rotation * new Vector3(1, 0, 1);
            }

            GUI.backgroundColor = new Color(0, .7f, .7f, .3f);
            // Button der Player selektiert
            if (GUILayout.Button("Select Player"))
            {
                Transform playerTrans = GameObject.FindGameObjectsWithTag("Player")[0].transform;
                Selection.activeGameObject = playerTrans.gameObject;
            }

            GUILayout.Space(8f);

            GUI.backgroundColor = new Color(0, .7f, .7f, .5f);

            if (GUILayout.Button("Move Object Position"))
            {
                Transform trans = Selection.activeGameObject.transform;
                SceneView.lastActiveSceneView.MoveToView(trans);
                trans.position = new Vector3(trans.position.x, trans.position.y, 0f);
            }

            if (GUILayout.Button("Random Rotate Selected Objects"))
            {
                Transform[] transforms = Selection.transforms;
                foreach(Transform trans in transforms)
                {
                    trans.rotation = Random.rotation;
                }
            }

            GUI.backgroundColor = new Color(.1f, .7f, .3f, .4f);
            // Button der das selektierte GameObject in die im NautiWindow selektierte Szene verschiebt
            if (GUILayout.Button("Set Object Scene"))
            {
                Scene sceneToMoveTo = SceneManager.GetSceneAt(EditorPrefs.GetInt("SceneIndex"));
                foreach (GameObject go in Selection.gameObjects)
                {
                    if (go.transform.parent == null)
                        SceneManager.MoveGameObjectToScene(go, sceneToMoveTo);
                    else
                    {
                        go.transform.parent = null;
                        SceneManager.MoveGameObjectToScene(go, sceneToMoveTo);
                    }
                }
            }

            /*
            GUI.backgroundColor = new Color(.1f, .7f, .3f, .4f);
            // Button der das selektierte GameObject in die im NautiWindow selektierte Szene verschiebt
            if (GUILayout.Button("Random Object Rotation X"))
            {
                foreach (GameObject go in Selection.gameObjects)
                {
                    if (go.transform.parent == null)
                    {
                        // rotate

                    }
                    else
                    {
                            // rotate
                    }
                }
            }
            */

            GUI.backgroundColor = new Color(0, .6f, .2f, .35f);
            // Button der das selektierte GameObject in die im NautiWindow selektierte Szene verschiebt
            if (GUILayout.Button("Delete SaveGame and Prefs"))
            {
                SaveSystem.DeleteSaveGame();
            }   

            if (GUILayout.Button("Toggle Splashscreen"))
            {
                if (canvas == null)
                    canvas = GameObject.Find("Canvas");

                if (splashScreen == null)
                    splashScreen = canvas.transform.Find("SplashScreen").gameObject;

                splashScreen.SetActive(!splashScreen.activeSelf);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("UtilityWindow Error");
            Debug.LogException(e);
        }
    }
}
#endif
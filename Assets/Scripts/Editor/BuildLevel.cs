using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEditor;
using UnityEngine;

public class BuildLevel : EditorWindow
{
    private TextAsset _curLevel;
    private GameObject _world;

    [MenuItem("Tools/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<BuildLevel>("Level Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Assign Level File:");
        _curLevel = EditorGUILayout.ObjectField(_curLevel,typeof(TextAsset),false) as TextAsset;

        EditorGUILayout.LabelField("Assign Parent Transform");
        _world = EditorGUILayout.ObjectField(_world, typeof(GameObject), true) as GameObject;

        if(GUILayout.Button("Create Level") && _curLevel != null)
        {
            CreateLevel();
        }
    }

    private void CreateLevel()
    {
        Debug.Log("Creating level... " + _curLevel.name);
        try
        {
            string jsonData = _curLevel.text;
            Dictionary<string,object> gameData = MiniJSON.Json.Deserialize(jsonData) as Dictionary<string, object>;
            int height = int.Parse(gameData["height"].ToString());
            int width = int.Parse(gameData["width"].ToString());

            List<object> layers = (List<object>)gameData["layers"];
            foreach(object obj in layers)
            {
                Dictionary<string, object> layerData = (Dictionary<string, object>)obj; 
                if(layerData.ContainsKey("data"))
                {
                   List<object> levelTiles = (List<object>)layerData["data"];
                    for(int i=0; i < levelTiles.Count;i++)
                    {
                        switch(levelTiles[i].ToString())
                        {
                            case "1": CreateGameObject("Prefab_Flower",i,height,width);break;
                            case "2": CreateGameObject("Prefab_Star", i,height,width);break;
                            case "3": CreateGameObject("Prefab_Mario", i,height,width);break;
                            case "4": CreateGameObject("Prefab_Floor", i,height,width);break;
                            case "5": CreateGameObject("Prefab_Coin", i,height,width); break;
                            case "6": CreateGameObject("Prefab_Spikes", i, height, width);break;
                        }
                    }
                }
            }

            Debug.Log("Height: " + height + ", Width: " + width);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void CreateGameObject(string prefabName,int index,int height,int width)
    {
        GameObject temp = Instantiate(Resources.Load("Tiles/" +prefabName)) as GameObject;
        int colCalc = index % width;
        string col = colCalc.ToString();
        if(colCalc < 10)
            col = "0" + colCalc;

        int rowCalc = (int)((height - 1) - ((int)(index / width)));
        string row = rowCalc.ToString();
        if(rowCalc < 10)
            row = "0" + rowCalc.ToString();

        temp.name = row + col;
        temp.transform.localPosition = new Vector3(colCalc, rowCalc, 0);

        if (_world)
            temp.transform.SetParent(_world.transform);
    }
}

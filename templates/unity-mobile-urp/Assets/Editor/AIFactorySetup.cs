#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AIFactorySetup
{
    [MenuItem("AI Factory/01. Create or Fix Scenes")]
    public static void CreateOrFixScenes()
    {
        // ensure folders
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        // Bootstrap scene
        var bootstrapPath = "Assets/Scenes/Bootstrap.unity";
        Scene bootstrap;
        if (!System.IO.File.Exists(bootstrapPath))
        {
            bootstrap = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var go = new GameObject("_Bootstrap");
            go.AddComponent<Bootstrap>();
            EditorSceneManager.SaveScene(bootstrap, bootstrapPath);
        }
        else bootstrap = EditorSceneManager.OpenScene(bootstrapPath, OpenSceneMode.Single);

        // Main scene
        var mainPath = "Assets/Scenes/Main.unity";
        Scene main;
        if (!System.IO.File.Exists(mainPath))
        {
            main = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            // Camera to Orthographic
            var cam = Camera.main; if (cam!=null){ cam.orthographic = true; cam.orthographicSize = 5; cam.backgroundColor = new Color(0.10f,0.12f,0.17f); }

            // Systems + ScoreSystem
            var systems = new GameObject("Systems");
            systems.AddComponent<ScoreSystem>();

            // Block
            var blockGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
            blockGO.name = "Block";
            blockGO.transform.localScale = new Vector3(3f,0.5f,1f);
            blockGO.AddComponent<MovingBlock>();

            // Spawner
            var spawnerGO = new GameObject("Spawner");
            var spawner = spawnerGO.AddComponent<BlockSpawner>();
            spawnerGO.transform.position = Vector3.zero;

            // Difficulty
            var diff = systems.AddComponent<DifficultyRamp>();
            typeof(DifficultyRamp).GetField("spawner", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)
                ?.SetValue(diff, spawner);

            EditorSceneManager.SaveScene(main, mainPath);
        }
        else main = EditorSceneManager.OpenScene(mainPath, OpenSceneMode.Single);

        Debug.Log("[AI Factory] Scenes ready at Assets/Scenes/ (Bootstrap & Main).");
    }

    [MenuItem("AI Factory/02. Configure Build Settings (iOS)")]
    public static void ConfigureBuildSettings()
    {
        // add scenes
        var list = EditorBuildSettings.scenes;
        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Bootstrap.unity", true));
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Main.unity", true));
        EditorBuildSettings.scenes = scenes.ToArray();

        // PlayerSettings
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1); // ARM64
        PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new[] { UnityEngine.Rendering.GraphicsDeviceType.Metal });
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.ai.factory.stackslice");
        Debug.Log("[AI Factory] Build Settings configured for iOS.");
    }

    [MenuItem("AI Factory/03. Build iOS to ai-game-factory/builds/iOS/StackSlice-Xcode")]
    public static void BuildIOS()
    {
        ConfigureBuildSettings();

        var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "builds/iOS/StackSlice-Xcode");
        System.IO.Directory.CreateDirectory(path);
        var scenes = new[] {
            "Assets/Scenes/Bootstrap.unity",
            "Assets/Scenes/Main.unity"
        };
        var report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.iOS, BuildOptions.None);
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            Debug.Log("[AI Factory] iOS Xcode build completed: " + path);
        else
            Debug.LogError("[AI Factory] Build failed: " + report.summary.result);
    }
}
#endif

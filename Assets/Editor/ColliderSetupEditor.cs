using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class ColliderSetupEditor : EditorWindow
{
    GameObject selectedGameObject;
    AnimatorController animatorController;

    [MenuItem("Tools/Collider Setup Helper")]
    public static void ShowWindow()
    {
        GetWindow<ColliderSetupEditor>("Collider Setup Helper");
    }

    void OnGUI()
    {
        GUILayout.Label("Collider Setup for Animations", EditorStyles.boldLabel);

        selectedGameObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", selectedGameObject, typeof(GameObject), true);
        animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", animatorController, typeof(AnimatorController), false);

        if (GUILayout.Button("Setup Colliders"))
        {
            SetupColliders();
        }
    }

    void SetupColliders()
    {
        if (selectedGameObject == null || animatorController == null)
        {
            Debug.LogError("Please assign both a target GameObject and an Animator Controller.");
            return;
        }

        ColliderSwitcher colliderSwitcher = selectedGameObject.GetComponent<ColliderSwitcher>();
        if (colliderSwitcher == null)
        {
            Debug.LogError("ColliderSwitcher component not found on the selected GameObject.");
            return;
        }

        foreach (var clip in animatorController.animationClips)
        {
            // Clear existing animation events
            AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[0]);

            List<float> keyFrames = GetKeyFramesForClip(clip.name);

            foreach (var frame in keyFrames)
            {
                string colliderName = DetermineColliderName(clip.name, frame);
                AddAnimationEvent(clip, frame / clip.frameRate, "SwitchCollider", colliderName);
            }
        }

        Debug.Log("Collider setup complete.");
    }


    List<float> GetKeyFramesForClip(string clipName)
    {
        AnimationClip clip = null;
        foreach (var ac in animatorController.animationClips)
        {
            if (ac.name == clipName)
            {
                clip = ac;
                break;
            }
        }

        List<float> keyFrames = new List<float>();
        if (clip != null)
        {
            float frameRate = clip.frameRate;
            float totalFrames = Mathf.Floor(clip.length * frameRate);

            for (int i = 0; i < totalFrames; i++)
            {
                float time = i;
                keyFrames.Add(time);
            }
        }
        return keyFrames;
    }


    string DetermineColliderName(string clipName, float frame)
    {
        return clipName + "_" + frame;
    }

    void AddAnimationEvent(AnimationClip clip, float time, string functionName, string parameter)
    {
        AnimationEvent evt = new AnimationEvent
        {
            functionName = functionName,
            time = time,
            stringParameter = parameter
        };

        var existingEvents = new List<AnimationEvent>(AnimationUtility.GetAnimationEvents(clip));
        existingEvents.Add(evt);
        AnimationUtility.SetAnimationEvents(clip, existingEvents.ToArray());
    }
}

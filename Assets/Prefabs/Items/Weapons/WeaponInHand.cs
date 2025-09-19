using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponInHand : MonoBehaviour
{
    private Animator animator;
    private int attackAnimationCount = 1;

    [Header("Base weapon animation controller")]
    [SerializeField] private RuntimeAnimatorController baseController;

    public void ApplyModelAnimations()
    {
#if UNITY_EDITOR
        animator = GetComponent<Animator>();

        // Get the FBX model asset
        GameObject modelAsset = PrefabUtility.GetCorrespondingObjectFromSource(PrefabUtility.GetNearestPrefabInstanceRoot(gameObject));
        if (modelAsset == null)
        {
            Debug.LogError("No model asset found for this prefab!");
            return;
        }

        // Grab all clips from that FBX
        string fbxPath = AssetDatabase.GetAssetPath(modelAsset);
        AnimationClip[] clips = AssetDatabase.LoadAllAssetsAtPath(fbxPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.Contains("__preview__")) // Exclude preview clips
            .ToArray();

        // Debug
        Debug.Log($"{clips.Length} clips found on model {modelAsset.name}");

        // Make override controller
        AnimatorOverrideController overrideController = new(baseController)
        {
            name = $"{baseController.name}_Override_{modelAsset.name}"
        };

        // Save the override controller as an asset
        string path = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(modelAsset)), $"{gameObject.name}_Override.controller");
        if (AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(path) != null)
            AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(overrideController, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Map clips by name
        attackAnimationCount = 0;
        foreach (AnimationClip clip in clips)
        {
            string clipName = string.Empty;
            int underscoreIndex = clip.name.IndexOf('_');
            if (underscoreIndex >= 0)
                clipName = clip.name[(underscoreIndex + 1)..];

            if (overrideController[clipName] != null)
            {
                overrideController[clipName] = clip;
                Debug.Log($"Mapped: {clipName}");
            }

            if(clipName.Contains("Attack"))
                attackAnimationCount++;
        }

        // Assign the override controller to the animator
        animator.runtimeAnimatorController = overrideController;
        EditorUtility.SetDirty(this);
#endif
    }

    public enum AnimationType
    {
        Attack,
        BlockStart,
        BlockEnd,
        Reload,
        Use,
    }
    public void PlayAnimation(AnimationType animationType)
    {
        if (!animator) animator = GetComponent<Animator>();

        switch (animationType)
        {
            case AnimationType.Attack:
                animator.SetTrigger($"Attack_{new System.Random().Next(1, attackAnimationCount + 1)}");
                break;
            case AnimationType.BlockStart:
                animator.SetBool("Block", true);
                break;
            case AnimationType.BlockEnd:
                animator.SetBool("Block", false);
                break;
            case AnimationType.Reload:
                animator.SetTrigger("Reload");
                break;
            case AnimationType.Use:
                animator.SetTrigger("Use");
                break;
        }
    }
}

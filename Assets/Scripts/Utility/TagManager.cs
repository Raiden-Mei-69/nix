using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static TagManager Instance;
    
    [TagSelector] public string PlayerTag;
    [TagSelector] public string EnemyTag;
    [TagSelector] public string etherTag;
    [TagSelector] public string lootTag;
    [TagSelector] public string[] groundTags;
    [TagSelector] public string ChestTag;
    [TagSelector] public string animalTag;
    [TagSelector] public string fishTag;
    [TagSelector] public string destructableTag;

    private void Awake()
    {
        if(Instance!=null)
            Destroy(Instance);
        Instance = this;    
    }
}
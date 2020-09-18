using UnityEngine;

//===================================================================||  INSTANTIATION
public class ParallaxSettings : MonoBehaviour {
    public ParallaxClass[] parallaxes;
}
//===================================================================||  END OF INSTANTIATION

//===================================================================||  THE CLASS
[System.Serializable] public class ParallaxClass {
    public string name;
    public float scaleFactor;
}
//===================================================================||  END OF THE CLASS
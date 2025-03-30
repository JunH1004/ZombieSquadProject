using System.Collections.Generic;
using UnityEngine;
public enum FacilityType {
    Farm,
    Laboratory
}
public class FacilityZone : MonoBehaviour{
    
    public FacilityType facilityType;
    [SerializeField]
    int level;
    List<Effect> effects = new List<Effect>();
    
}

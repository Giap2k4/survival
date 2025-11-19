using System;

[Serializable]
public class ResourceModel
{
    public PackageReward package;
}

[Serializable]
public class PackageReward
{
    public EnumBase.ResourcesType resType;
    public int resId;
    public int resQuantity;
}

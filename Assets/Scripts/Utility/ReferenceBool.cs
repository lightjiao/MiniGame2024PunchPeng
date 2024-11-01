using System;

[Serializable]
public class ReferenceBool
{
    public int RefCnt;
    public static implicit operator bool(ReferenceBool self) => self.RefCnt > 0;
}
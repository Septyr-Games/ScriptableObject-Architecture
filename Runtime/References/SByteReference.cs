namespace Com.Septyr.ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class SByteReference : BaseReference<sbyte, SByteVariable>
    {
        public SByteReference() : base() { }
        public SByteReference(sbyte value) : base(value) { }
    } 
}
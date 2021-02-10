namespace Com.Septyr.ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class SceneReference : BaseReference<SceneInfo, SceneVariable>
    {
        public SceneReference()
        {
        }
        public SceneReference(SceneInfo value) : base(value)
        {
        }
    }
}
namespace Assets.Tools.Script.Sound
{
    /// <summary>
    /// 声音控制
    /// </summary>
    public interface IGameVolume
    {
        /// <summary>
        /// 背景音
        /// </summary>
        float backgroundSound { get; set; }
        /// <summary>
        /// 音效音
        /// </summary>
        float effectSound { get; set; }
    }
}

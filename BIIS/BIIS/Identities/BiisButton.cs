namespace BIIS.Identities
{
    /// <summary>
    /// Type, that describes action, BiisButton can do
    /// </summary>
    /// <param name="biisControl"></param>
    public delegate void ButtonDelegate(BiisLabel biisControl);

    /// <summary>
    /// BiisButton class, that represents "biisButton" tag
    /// </summary>
    public class BiisButton : BiisControl
    {
        /// <summary>
        /// Target label name
        /// </summary>
        public string OnClickTarget { get; set; }
        /// <summary>
        /// BissButton action
        /// </summary>
        public ButtonDelegate ButtonOnClickDelegate { get; set; }
    }
}
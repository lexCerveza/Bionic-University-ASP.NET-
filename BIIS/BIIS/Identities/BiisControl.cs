namespace BIIS.Identities
{
    /// <summary>
    /// Basic class, represents all biis tags 
    /// </summary>
    public abstract class BiisControl
    {
        /// <summary>
        /// Name of element
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Text, which element holds
        /// </summary>
        public string Text { get; set; }
    }
}
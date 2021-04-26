namespace Melberg.Core.Messaging
{
    public enum MessageProcessingResult
    {
        Success = 1,
        /// <summary>
        /// fail, but retry
        /// </summary>
        Retry = 2,
        /// <summary>
        /// reject.  dead letter
        /// </summary>
        Failed = 3
    }
}
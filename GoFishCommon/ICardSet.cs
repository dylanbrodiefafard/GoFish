using System;

namespace GoFishCommon
{
    /// <summary>
    ///     Interface defining a card set
    /// </summary>
    public interface ICardSet
    {
        Guid CardSetId { get; }
        int ItemCount { get; }
    }
}
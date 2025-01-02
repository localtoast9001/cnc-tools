//-----------------------------------------------------------------------
// <copyright file="InternalExtensions.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode;

/// <summary>
/// Internal extension methods.
/// </summary>
internal static class InternalExtensions
{
    /// <summary>
    /// Adds a sequence of items to a collection.
    /// </summary>
    /// <typeparam name="T">The type of element.</typeparam>
    /// <param name="target">The target collection.</param>
    /// <param name="items">The items to add.</param>
    public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            target.Add(item);
        }
    }
}
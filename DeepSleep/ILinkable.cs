namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface ILinkable
    {
        /// <summary>Gets or sets the links.</summary>
        /// <value>The links.</value>
        List<Link> Links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ILinkableExtensionMethods
    {
        /// <summary>Adds the link.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="linkable">The linkable.</param>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        public static T AddLink<T>(this T linkable, Link link) where T : ILinkable
        {
            if (linkable == null)
                return default(T);

            if (link == null)
                return linkable;

            if (linkable.Links == null)
                linkable.Links = new List<Link>();

            if (!linkable.Links.Exists(i => i.Rel == link.Rel && i.Href == link.Href))
            {
                linkable.Links.Add(link);
            }

            return linkable;
        }

        /// <summary>Determines whether the specified linkable has link.</summary>
        /// <param name="linkable">The linkable.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool HasLink(this ILinkable linkable, string type)
        {
            if (linkable == null || linkable.Links == null)
                return false;

            return linkable.Links.Exists(i => i.Rel == type);
        }
    }
}

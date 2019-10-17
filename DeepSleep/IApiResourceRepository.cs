namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>Handles data for retrieval/updating of API resource related data.</summary>
    public interface IApiResourceRepository
    {
        /// <summary>Gets the API resource by unique identifier.</summary>
        /// <param name="guid">The unique identifier to search for.</param>
        /// <returns>The resource matching the supplied GUID or null.</returns>
        Task<ApiResourceConfig> GetApiResourceByGuid(string guid);
    }
}
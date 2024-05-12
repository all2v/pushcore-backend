using System;
using System.Threading.Tasks;
using Astrum.Application.Models;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Application.Services;

public interface IApplicationConfigurationService
{
    #region Public Methods

    /// <summary>
    ///     Insert a configuration to the database
    /// </summary>
    /// <param name="applicationConfiguration">
    ///     Configuration details. See <see cref="ApplicationConfigurationDto" /> for more
    ///     information.
    /// </param>
    Task<Result> Create(ApplicationConfigurationDto applicationConfiguration);

    /// <summary>
    ///     Retrieves application configuration details.
    /// </summary>
    /// <param name="id">The ID of record</param>
    /// <param name="decrypt">If is true the value of application configuration returned is decrypted.</param>
    /// <returns></returns>
    Task<Result<ApplicationConfigurationDto>> Details(string id, bool decrypt);

    /// <summary>
    ///     Edit an exsisting configuration to the database
    /// </summary>
    /// <param name="appConfig">Configuration details. See <see cref="ApplicationConfigurationDto" /> for more information.</param>
    Task<Result> Edit(ApplicationConfigurationDto appConfig);

    /// <summary>
    ///     Deletes an entity from the database.
    /// </summary>
    /// <param name="id">Primary key of the entity.</param>
    /// <returns>True if the delete was successfull.</returns>
    Task<Result<bool>> Delete(string id);

    /// <summary>
    ///     Retrieves a configuration value based on the given key.
    /// </summary>
    /// <param name="key">Configuration Key.</param>
    /// <returns>Result with the configuration value.</returns>
    Task<Result<string>> GetValue(string key);

    /// <summary>
    ///     Retrieves a configuration value converted to an INT type based on the given key.
    /// </summary>
    /// <param name="key">Configuration Key.</param>
    /// <returns>Result with the configuration value converted to an INT type.</returns>
    Task<Result<int>> GetValueInt(string key);

    /// <summary>
    ///     Retrieves a configuration value converted to an Bool type based on the given key.
    /// </summary>
    /// <param name="key">Configuration Key.</param>
    /// <returns>Result with the configuration value converted to an Bool type.</returns>
    Task<Result<bool>> GetValueBool(string key);

    /// <summary>
    ///     Retrieves a configuration value converted to a DateTimeOffset type based on the given key.
    /// </summary>
    /// <param name="key">Configuration Key.</param>
    /// <returns>Result with the configuration value converted to an DateTime type.</returns>
    Task<Result<DateTimeOffset>> GetValueDateTime(string key);

    /// <summary>
    ///     Retrieves a configuration value which is delimited by the default '|' character
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Result<string[]>> GetMultiple(string key);

    /// <summary>
    ///     Retrieves a configuration value which is delimited by a character
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Result<string[]>> GetMultiple(string key, string delimiter);

    #endregion Public Methods
}
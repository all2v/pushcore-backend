using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astrum.Application.Aggregates;
using Astrum.Application.Models;
using Astrum.Application.Repositories;
using Astrum.SharedLib.Common;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Astrum.Application.Services;

/// <inheritdoc cref="IApplicationConfigurationService" />
internal class ApplicationConfigurationService : IApplicationConfigurationService
{
    private const string MULTIPLE_VALUE_DELIMITER = "|";
    private static List<ApplicationConfiguration> _appConfigs;
    private readonly IApplicationConfigurationRepository _appConfigRepository;
    private readonly ILogger<ApplicationConfigurationService> _logger;
    private readonly IMapper _mapper;

    public ApplicationConfigurationService(ILogger<ApplicationConfigurationService> logger,
        IApplicationConfigurationRepository appConfigRepository, IMapper mapper)
    {
        _logger = logger;
        _appConfigRepository = appConfigRepository;
        _mapper = mapper;
    }

    private async Task<List<ApplicationConfiguration>> GetConfigurations()
    {
        if (_appConfigs == null || _appConfigs.Count == 0) _appConfigs = await _appConfigRepository.ListAsync();

        return _appConfigs;
    }

    #region Public Methods

    public async Task<Result> Create(ApplicationConfigurationDto dto)
    {
        try
        {
            _appConfigs = null;
            if (dto.IsEncrypted)
                dto.Value = StringCipherHelper.EncryptWithRandomSalt(dto.Value);

            await _appConfigRepository.AddAsync(_mapper.Map<ApplicationConfiguration>(dto));
            await _appConfigRepository.UnitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to create application configuration");
            return Result.Error("Error while trying to create application configuration");
        }
    }

    public async Task<Result<ApplicationConfigurationDto>> Details(string id, bool decrypt)
    {
        try
        {
            if (id == null) throw new Exception("Config not found");

            var entity = await _appConfigRepository.GetByIdAsync(id);

            if (entity == null) throw new Exception("Config not found");

            if (decrypt && entity != null && entity.IsEncrypted)
                entity.Value = StringCipherHelper.DecryptWithRandomSalt(entity.Value);

            return Result.Success(_mapper.Map<ApplicationConfigurationDto>(entity));
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result> Edit(ApplicationConfigurationDto dto)
    {
        try
        {
            _logger.LogDebug($"Editing application configuration {nameof(dto.Id)}: {dto.Id}");
            _appConfigs = null;
            var originalEntity = await _appConfigRepository.GetByIdAsync(dto.Id);
            if ((dto.IsEncrypted && !originalEntity.IsEncrypted) ||
                (dto.IsEncrypted && dto.Value != originalEntity.Value))
                dto.Value = StringCipherHelper.EncryptWithRandomSalt(dto.Value);

            var originalEntityId = originalEntity.Id;
            var updatedEntity = _mapper.Map(dto, originalEntity);
            if (originalEntity == null) throw new Exception("Config not found");

            // updatedEntity.Id = originalEntityId; //to avoid lower case discrepancy during IMapper mapping TODO uncomment
            await _appConfigRepository.UnitOfWork.SaveChangesAsync();

            //reset the appconfigs in order to refresh on next request
            _appConfigs = null;
            _logger.LogDebug($"Application configuration '{dto.Id}' edited succesfully");
            return Result.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to edit application configuration";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<bool>> Delete(string id)
    {
        try
        {
            var entity = await _appConfigRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Page not found");
            await _appConfigRepository.DeleteAsync(entity);
            await _appConfigRepository.UnitOfWork.SaveChangesAsync();
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to delete application configuration";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<string>> GetValue(string key)
    {
        try
        {
            var entity = await _appConfigRepository.GetByIdAsync(key);
            var value = entity.Value;
            if (entity.IsEncrypted)
                value = StringCipherHelper.DecryptWithRandomSalt(entity.Value);

            return Result.Success(value);
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration value";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<int>> GetValueInt(string key)
    {
        try
        {
            var entity = await _appConfigRepository.GetByIdAsync(key);
            var value = entity.Value;
            if (entity.IsEncrypted)
                value = StringCipherHelper.DecryptWithRandomSalt(entity.Value);

            if (int.TryParse(value, out var valueInt))
                return Result.Success(valueInt);

            throw new Exception("Conversion of application configuration value to int was not successful");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration value";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<bool>> GetValueBool(string key)
    {
        try
        {
            var entity = await _appConfigRepository.GetByIdAsync(key);
            var value = entity.Value;
            if (entity.IsEncrypted)
                value = StringCipherHelper.DecryptWithRandomSalt(entity.Value);

            if (bool.TryParse(value, out var valueBool))
                return Result.Success(valueBool);

            throw new Exception("Convertion of application configuration value to bool is not successful");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration value from the database.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<DateTimeOffset>> GetValueDateTime(string key)
    {
        try
        {
            var entity = await _appConfigRepository.GetByIdAsync(key);
            var value = entity.Value;
            if (entity.IsEncrypted)
                value = StringCipherHelper.DecryptWithRandomSalt(entity.Value);

            if (DateTimeOffset.TryParse(value, out var valueDate))
                return Result.Success(valueDate);

            throw new Exception("Convertion of application configuration value to bool is not successful");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration value";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<string[]>> GetMultiple(string key)
    {
        return await GetMultiple(key, MULTIPLE_VALUE_DELIMITER);
    }

    public async Task<Result<string[]>> GetMultiple(string key, string delimiter)
    {
        try
        {
            var valueResult = await GetValue(key);
            if (!valueResult.IsSuccess)
                Result.Error("Unable to fetch multiple keys from");
            return Result.Success(valueResult.Data.Split(delimiter));
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to retrieve application configuration value";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    #endregion Public Methods
}
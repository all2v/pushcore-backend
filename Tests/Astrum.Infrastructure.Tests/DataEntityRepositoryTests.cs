using Astrum.Application.Aggregates;
using Astrum.Application.Repositories;
using Astrum.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Astrum.Infrastructure.Tests;

public class DataEntityRepositoryTests : TestBase
{
    private readonly IApplicationConfigurationRepository _appConfigRepo;

    public DataEntityRepositoryTests()
    {
        _appConfigRepo = ServiceProvider.GetService<IApplicationConfigurationRepository>();
    }

    [Fact]
    public void ApplicationConfigurationRepo_Add_AddsEntityToContext()
    {
        _appConfigRepo.Add(new ApplicationConfiguration("test") {Id = "test"});
        _appConfigRepo.UnitOfWork.SaveChanges();
        var appConfig = ApplicationContext.ApplicationConfigurations.SingleOrDefault(a => a.Id == "test");

        Assert.NotNull(appConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_AddAsync_AddsEntityToContext()
    {
        await _appConfigRepo.AddAsync(new ApplicationConfiguration("test") {Id = "test"});
        await _appConfigRepo.UnitOfWork.SaveChangesAsync();
        var appConfig = ApplicationContext.ApplicationConfigurations.SingleOrDefault(a => a.Id == "test");

        Assert.NotNull(appConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_AddRange_AddsEntitiesToContext()
    {
        await _appConfigRepo.AddRangeAsync(new List<ApplicationConfiguration>
        {
            new("test") {Id = "test"},
            new("test2") {Id = "test2"}
        });
        _appConfigRepo.UnitOfWork.SaveChanges();
        var appConfigs = ApplicationContext.ApplicationConfigurations.ToList();
        var appConfig1 = appConfigs.SingleOrDefault(a => a.Id == "test");
        var appConfig2 = appConfigs.SingleOrDefault(a => a.Id == "test2");

        Assert.NotNull(appConfig1);
        Assert.NotNull(appConfig2);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_AddRangeAsync_AddsEntitiesToContext()
    {
        await _appConfigRepo.AddRangeAsync(new List<ApplicationConfiguration>
        {
            new("test") {Id = "test"},
            new("test2") {Id = "test2"}
        });
        await _appConfigRepo.UnitOfWork.SaveChangesAsync();
        var appConfigs = ApplicationContext.ApplicationConfigurations.ToList();
        var appConfig1 = appConfigs.SingleOrDefault(a => a.Id == "test");
        var appConfig2 = appConfigs.SingleOrDefault(a => a.Id == "test2");

        Assert.NotNull(appConfig1);
        Assert.NotNull(appConfig2);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_Delete_RemovesEntityFromContext()
    {
        AddApplicationData(ApplicationContext);
        var dummyAppConfig = ApplicationContext.ApplicationConfigurations.Single(a => a.Id == "dummy1");
        await _appConfigRepo.DeleteAsync(dummyAppConfig);
        await _appConfigRepo.UnitOfWork.SaveChangesAsync();

        var appConfig = ApplicationContext.ApplicationConfigurations.SingleOrDefault(a => a.Id == "dummy1");

        Assert.Null(appConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_DeleteRange_RemovesEntitiesFromContext()
    {
        AddApplicationData(ApplicationContext);
        var dummyAppConfig1 = ApplicationContext.ApplicationConfigurations.Single(a => a.Id == "dummy1");
        var dummyAppConfig2 = ApplicationContext.ApplicationConfigurations.Single(a => a.Id == "dummy2");
        await _appConfigRepo.DeleteRangeAsync(new List<ApplicationConfiguration> {dummyAppConfig1, dummyAppConfig2});
        _appConfigRepo.UnitOfWork.SaveChanges();

        var appConfig1 = ApplicationContext.ApplicationConfigurations.SingleOrDefault(a => a.Id == "dummy1");
        var appConfig2 = ApplicationContext.ApplicationConfigurations.SingleOrDefault(a => a.Id == "dummy2");

        Assert.Null(appConfig1);
        Assert.Null(appConfig2);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_Exists_IfEntityExists_ReturnsTrue()
    {
        AddApplicationData(ApplicationContext);
        var exists = await _appConfigRepo.GetByIdAsync("dummy1") is not null;
        Assert.True(exists);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_Exists_IfEntityDoesNotExist_ReturnsFalse()
    {
        var exists = await _appConfigRepo.GetByIdAsync("some entity which doesnt exist") is not null;
        Assert.False(exists);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_ExistsAsync_IfEntityExists_ReturnsTrue()
    {
        AddApplicationData(ApplicationContext);
        var exists = await _appConfigRepo.GetByIdAsync("dummy1") is not null;
        Assert.True(exists);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_ExistsAsync_IfEntityDoesNotExist_ReturnsFalse()
    {
        var exists = await _appConfigRepo.GetByIdAsync("dummy1") is not null;
        Assert.False(exists);
    }

    [Fact]
    public void ApplicationConfigurationRepo_GetAll_FetchesAllRecords()
    {
        AddApplicationData(ApplicationContext);
        var count = ApplicationContext.ApplicationConfigurations.Count();
        var allConfigs = _appConfigRepo.Items;

        Assert.NotEmpty(allConfigs);
        Assert.Equal(count, allConfigs.Count());
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetBy_IfSatisfiesPredicate_ReturnsListWithEntity()
    {
        AddApplicationData(ApplicationContext);
        var appConfigs = await _appConfigRepo.ListAsync(a => a.Id == "dummy1" && a.Value == "dummyvalue1");

        var appConfigInList = appConfigs.First(a => a.Id == "dummy1" && a.Value == "dummyvalue1");

        Assert.NotNull(appConfigs);
        Assert.NotEmpty(appConfigs);
        Assert.Contains(appConfigs, a => a.Id == "dummy1" && a.Value == "dummyvalue1");
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetBy_IfDoesNotSatisfyPredicate_ReturnsEmptyList()
    {
        AddApplicationData(ApplicationContext);
        var appConfigs = await _appConfigRepo.ListAsync(a => a.Id == "dummy1" && a.Value == "dummyValue1NotExists");

        Assert.NotNull(appConfigs);
        Assert.Empty(appConfigs);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetFirst_IfExists_ReturnsFirstEntity()
    {
        var duplicateValue = "appConfigValue";
        ApplicationContext.ApplicationConfigurations.Add(new ApplicationConfiguration(duplicateValue)
            {Id = "appconfig1"});
        ApplicationContext.ApplicationConfigurations.Add(new ApplicationConfiguration(duplicateValue)
            {Id = "appconfig2"});
        await ApplicationContext.SaveChangesAsync();

        var retrievedAppConfig =
            await _appConfigRepo.FirstOrDefaultAsync(a => a.Value == duplicateValue);

        Assert.Equal("appconfig1", retrievedAppConfig.Id);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetFirst_IfDoesNotExist_ReturnsNull()
    {
        ApplicationContext.Add(new ApplicationConfiguration("appConfig1Value") {Id = "appconfig1"});

        var retrievedAppConfig =
            await _appConfigRepo.FirstOrDefaultAsync(a => a.Value == "some value which does not exist");

        Assert.Null(retrievedAppConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetFirstAsync_IfExists_ReturnsFirstEntity()
    {
        var duplicateValue = "appConfigValue";
        ApplicationContext.ApplicationConfigurations.Add(new ApplicationConfiguration(duplicateValue)
            {Id = "appconfig1"});
        ApplicationContext.ApplicationConfigurations.Add(new ApplicationConfiguration(duplicateValue)
            {Id = "appconfig2"});
        ApplicationContext.SaveChanges();

        var retrievedAppConfig =
            await _appConfigRepo.FirstOrDefaultAsync(a => a.Value == duplicateValue);

        Assert.Equal("appconfig1", retrievedAppConfig.Id);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetFirstAsync_IfDoesNotExist_ReturnsNull()
    {
        ApplicationContext.Add(new ApplicationConfiguration("appConfig1Value") {Id = "appconfig1"});

        var retrievedAppConfig =
            await _appConfigRepo.FirstOrDefaultAsync(a => a.Value == "some value which does not exist");

        Assert.Null(retrievedAppConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetSingle_IfExists_ReturnsSingleEntity()
    {
        AddApplicationData(ApplicationContext);
        var appConfig = await _appConfigRepo.FirstOrDefaultAsync(a => a.Value == "dummyvalue1");

        Assert.NotNull(appConfig);
    }

    [Fact]
    public async Task ApplicationConfigurationRepo_GetSingle_IfDoesNotExist_ReturnsNull()
    {
        AddApplicationData(ApplicationContext);
        var appConfig = await _appConfigRepo.GetByIdAsync("does not exist");

        Assert.Null(appConfig);
    }

    [Fact]
    public void ApplicationConfigurationRepo_Update_UpdatesEntityCorrectly()
    {
        AddApplicationData(ApplicationContext);
        var appConfig = ApplicationContext.ApplicationConfigurations.Single(a => a.Value == "dummyvalue1");
        var changedValue = "changedValue";
        appConfig.Value = changedValue;
        _appConfigRepo.UnitOfWork.SaveChanges(true);

        var retrievedAppConfig = ApplicationContext.ApplicationConfigurations.AsNoTracking()
            .Single(a => a.Id == appConfig.Id);

        Assert.Equal(changedValue, retrievedAppConfig.Value);
    }

    [Fact]
    public void ApplicationConfigurationRepo_UpdateRange_UpdatesAllEntitiesCorrectly()
    {
        AddApplicationData(ApplicationContext);
        var appConfig1 = ApplicationContext.ApplicationConfigurations.Single(a => a.Value == "dummyvalue1");
        var appConfig2 = ApplicationContext.ApplicationConfigurations.Single(a => a.Value == "dummyvalue2");
        var changedValue1 = "changedValue1";
        var changedValue2 = "changedValue2";
        appConfig1.Value = changedValue1;
        appConfig2.Value = changedValue2;

        _appConfigRepo.UpdateRangeAsync(new List<ApplicationConfiguration> {appConfig1, appConfig2});
        _appConfigRepo.UnitOfWork.SaveChanges(true);

        var retrievedAppConfig1 = ApplicationContext.ApplicationConfigurations.AsNoTracking()
            .Single(a => a.Id == appConfig1.Id);
        var retrievedAppConfig2 = ApplicationContext.ApplicationConfigurations.AsNoTracking()
            .Single(a => a.Id == appConfig2.Id);

        Assert.Equal(changedValue1, retrievedAppConfig1.Value);
        Assert.Equal(changedValue2, retrievedAppConfig2.Value);
    }
}
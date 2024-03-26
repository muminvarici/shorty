using Microsoft.Extensions.DependencyInjection;
using Shorty.IntegrationTests.Helpers;

namespace Shorty.IntegrationTests;

public abstract class TestBase
{
    
    protected ApiFactory Factory { get; } 
    
    protected IServiceScope Scope { get; set; }
    
    
    protected TestBase()
    {
        Factory = new();
        Scope = Factory.Services.CreateScope();
    }

}
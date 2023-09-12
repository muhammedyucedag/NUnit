using System.ComponentModel;

namespace JobApplication.Services;

public interface IIdentityValidator
{
    bool IsValid(string identityNumber);
    //bool CheckConnectionToRemoteServer();
    ICountryDataProvider CountryDataProvider { get; }
    
    public ValidationMode ValidationMode { get; set; }

}

public enum ValidationMode
{
    [Description("Quick")] Quick,    
    [Description("Detailed")] Detailed
}

public interface ICountryData
{
    string Country { get; }
}

public interface ICountryDataProvider
{
    ICountryData CountryData { get; }
}
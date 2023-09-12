namespace JobApplication.Services;

public interface IIdentityValidator
{
    bool IsValid(string identityNumber);
}
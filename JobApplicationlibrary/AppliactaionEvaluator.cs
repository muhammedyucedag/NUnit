using System.ComponentModel;
using JobApplication.Services;

namespace JobApplication;

public class AppliactaionEvaluator
{
    private const int minAge = 18;
    private const int autoAcceptedYearOfExperience = 15;
    private List<string> techStackList = new() {"C#", "RabbitMQ", "Microservice", "Visual Studio"};
    private readonly IIdentityValidator _identityValidator;

    public AppliactaionEvaluator(IIdentityValidator identitValidator)
    {
        _identityValidator = identitValidator;
    }
    
    public ApplicationResult Evaluate(JobApplicationlibrary.Models.JobApplication form)
    {
        if (form.Applicant.Age < minAge)
            return ApplicationResult.AutoRejected;

        var connectionSucceed = _identityValidator.CheckConnectionToRemoteServer();
        var validIdentity = _identityValidator.IsValid(form.Applicant.IdentityNumber);
        if (!validIdentity)
            return ApplicationResult.TransferredToHR;

        var sr = GetTechStackSimilarityRate(form.TechStackList);
        if (sr < 25)
            return ApplicationResult.AutoRejected;
        if (sr > 75 && form.YearsOFeXperience >= autoAcceptedYearOfExperience)
            return ApplicationResult.AutoAccepted;

        return ApplicationResult.AutoAccepted;
    }

    private int GetTechStackSimilarityRate(List<string> techStacks)
    {
        var matchedCount = 
            techStacks
                .Count(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase));
        return (int)((double)matchedCount / techStackList.Count) * 100;
    }
}

public enum ApplicationResult
{
    [Description("AutoRejected")] AutoRejected,
    [Description("TransferredToHR")] TransferredToHR,
    [Description("TransferredToLead")] TransferredToLead,
    [Description("TransferredToCTO")] TransferredToCTO,
    [Description("AutoAccepted")] AutoAccepted,

}
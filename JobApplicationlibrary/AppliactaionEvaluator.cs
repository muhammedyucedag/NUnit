using System.ComponentModel;

namespace JobApplication;

public class AppliactaionEvaluator
{
    private const int minAge = 18;
    public ApplicationResult Evaluate(JobApplicationlibrary.Models.JobApplication form)
    {
        if (form.Applicant.Age < minAge)
            return ApplicationResult.AutoRejected;
        
        return ApplicationResult.AutoAccepted;
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
using JobApplication;
using JobApplicationlibrary.Models;

namespace JobApplicationLibraryUnitTest;

public class ApplicationEvaluateUnitTest
{
   [Test]
   public void Application_WithUnderAge_TransferredToAutoReject()
   {
      // Arrange
      var evaluator = new AppliactaionEvaluator();
      var form = new JobApplicationlibrary.Models.JobApplication()
      {
         Applicant = new Applicant()
         {
            Age = 17
         }
      };
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
   }
}
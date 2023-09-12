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
   
   [Test]
   public void Application_WithNoTechStack_TransferredToAutoReject()
   {
      // %25 oranında bezerlik yakaladık ve otomatik reddettik
      
      // Arrange
      var evaluator = new AppliactaionEvaluator();
      var form = new JobApplicationlibrary.Models.JobApplication()
      {
         Applicant = new Applicant{ Age = 19},
         TechStackList = new List<string> {""}
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
   }
   
   [Test]
   public void Application_WithTechStackOver75P_TransferredToAutoReject()
   {
      // %75 oranında benzerlik yakaladık ve otomatik bir şekilde kabul ettik
      
      // Arrange
      var evaluator = new AppliactaionEvaluator();
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant{ Age = 19},
         TechStackList = new List<string> {"C#", "RabbitMQ", "Microservice", "Visual Studio"},
         YearsOFeXperience = 16
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoAccepted));
   }
}
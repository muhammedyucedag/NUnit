using JobApplication;
using JobApplication.Services;
using JobApplicationlibrary.Models;
using Moq;

namespace JobApplicationLibraryUnitTest;

public class ApplicationEvaluateUnitTest
{
   [Test]
   public void Application_WithUnderAge_TransferredToAutoReject()
   {
      // Arrange
      var evaluator = new AppliactaionEvaluator(null);
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
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.DefaultValue = DefaultValue.Mock;
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
      mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true); // IIdentityValidator çağrılırsa geriye true döneceğiz
      
      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
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
   public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
   {
      // %75 oranında benzerlik yakaladık ve otomatik bir şekilde kabul ettik
      
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.DefaultValue = DefaultValue.Mock;
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
      mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true); // IIdentityValidator çağrılırsa geriye true döneceğiz
      
      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
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
   
   [Test]
   public void Application_WithInValidIdentityNumber_TransferredToHR()
   {
      // %75 oranında benzerlik yakaladık ve otomatik bir şekilde kabul ettik
      
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.DefaultValue = DefaultValue.Mock;
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
      mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false); // IIdentityValidator çağrılırsa geriye true döneceğiz
      
      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant{ Age = 19}
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToHR));
   }
   
   [Test]
   public void Application_WithOfficeLocation_TransferredToCTO()
   {
      // %75 oranında benzerlik yakaladık ve otomatik bir şekilde kabul ettik
      
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN");

      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant{ Age = 19}
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToCTO));
   }
}
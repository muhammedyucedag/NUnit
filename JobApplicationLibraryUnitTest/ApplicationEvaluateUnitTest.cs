using JobApplication;
using JobApplication.Services;
using JobApplicationlibrary.Models;
using Moq;
using FluentAssertions;

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
      //Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));

      appResult.Should().Be(ApplicationResult.AutoRejected);
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
      //mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Throws<Exception>();

      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant{ Age = 19},
         TechStackList = new List<string> {""}
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      //Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
      appResult.Should().Be(ApplicationResult.AutoRejected);

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
      //Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoAccepted));
      appResult.Should().Be(ApplicationResult.AutoAccepted);

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
      //Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToHR));
      appResult.Should().Be(ApplicationResult.TransferredToHR);

   }
   
   [Test]
   public void Application_WithOfficeLocation_TransferredToCTO()
   {
      
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
      //Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToCTO));
      appResult.Should().Be(ApplicationResult.TransferredToCTO);

   }

   [Test]
   public void Application_WithOver50_ValidationModeToDetailed()
   {
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.SetupAllProperties();
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN");
      
      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant{ Age = 51}
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert 
      //Assert.That(mockValidator.Object.ValidationMode, Is.EqualTo(ValidationMode.Detailed));
      mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);

   }

   [Test]
   public void Application_WithNullApplicatant_ThrowsArgumentNullException()
   {
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>();
      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication();

      // Action
      Action appResultAction = () => evaluator.Evaluate(form).Should();
      
      // Assert
      appResultAction.Should().Throw<ArgumentNullException>();
   }

   [Test]
   public void Application_WithDefaultValue_IsValidCalled()
   {
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.DefaultValue = DefaultValue.Mock;
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant
         {
            Age = 19,
            IdentityNumber = "1234"
            
         }
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      mockValidator.Verify(i => i.IsValid(It.IsAny<string>()),Times.Once());
   }
   
   
   [Test]
   public void Application_WithYoungAge_IsValidNeverCalled()
   {
      // Arrange
      var mockValidator = new Mock<IIdentityValidator>(); // böyle bir interface varmış gibi (fake class)
      mockValidator.DefaultValue = DefaultValue.Mock;
      mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

      var evaluator = new AppliactaionEvaluator(mockValidator.Object);
      var form = new JobApplicationlibrary.Models.JobApplication
      {
         Applicant = new Applicant
         {
            Age = 15
         }
      };
      
      // Action
      var appResult = evaluator.Evaluate(form);
      
      // Assert
      mockValidator.Verify(i => i.IsValid(It.IsAny<string>()),Times.Exactly(0)); 
      //Times.Never() testi geçti buradaki is valid hiç çağırlmamı oldu
      //Times.Exactly(0) is validi 0 kere çağırıyorz 
   }
}
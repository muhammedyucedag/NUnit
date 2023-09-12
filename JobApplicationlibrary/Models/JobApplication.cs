namespace JobApplicationlibrary.Models;

public class JobApplication
{
    public Applicant Applicant { get; set; }
    public int YearsOFeXperience { get; set; }
    public List<string> TechStackList { get; set; }
}
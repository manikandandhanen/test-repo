using System;
using System.Collections.Generic;

namespace HRMS.Entities
{
    public interface IAuditable
    {
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
    public abstract class Auditable : IAuditable
    {
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }



   


    public class Address
    {
        public string AddressId { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
    }

    public class PassportInfo
    {
        public string PassportInfoId { get; set; }
        public string PassportNumber { get; set; }
        public string AnyValidVisa { get; set; }
    }

    public class BankInfo
    {
        public string BankInfoId { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IFSCode { get; set; }
        public string BankAddress { get; set; }
    }

    public class SpouseInfo
    {
        public string SpouseInfoId { get; set; }
        public string SpouseName { get; set; }
        public string EmployerName { get; set; }
        public string WorkPhone { get; set; }
    }

    public class FamilyInfo
    {
        public string FamilyInfoId { get; set; }
        public string PersonName { get; set; }
        public DateTime PersonDOB { get; set; }
        public Relationship PersonRelationship { get; set; }
    }

    public class PFNomineeInfo
    {
        public string PFNomineeInfoId { get; set; }
        public DependentInfo Nominee { get; set; }

        public string NomineeShare { get; set; }

    }

    public class DependentInfo
    {
        public string DependentInfoId { get; set; }
        public Relationship DependentRelationship { get; set; }

        public string DependentName { get; set; }
        public DateTime DependentDOB { get; set; }
    }
   

    public enum Relationship
    {
        Self = 1,
        Father = 2,
        Mother = 3,
        Husband = 4,
        Son = 5,
        Daughter = 6,
        Brother = 7,
        Sister = 8,
        GrandFather = 9,
        GrandMother = 10,
        Uncle = 11,
        Aunty = 12,
        Friend = 13,
        Other = 14
    }

    public enum GuardianType
    {
        Father = 1,
        Husband = 2,
        Guardian = 3,
        Other = 4
    }

    //guardian
    public class PreviousJobInfo
    {
        public string PreviousJobInfoId { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }

        public int YearOfExperience { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double CTC { get; set; }
    }

    public class CurrentEmployemntInfo
    {
        public string CurrentEmployemntInfoId { get; set; }
        public DateTime DOJ { get; set; }
        public double GrossSalary { get; set; }
        public string Location { get; set; }
        public string Project { get; set; }
    }

    public enum InstitutionType
    {
        University=1,
        College = 2,
        Institution = 3,
        School = 4
    }

    public enum ProgramType
    {
        FullTime =1,
        PartTime =2,
        Distance = 3,
        Correspondence = 4,
        other = 5
    }

    public class EducationInfo
    {
        public string EducationInfoId { get; set; }
        public string ProgramName { get; set; }
        public string InstitutionType { get; set; }

        public DateTime YearOfCompletion { get; set; }
        public ProgramType ProgramType { get; set; }
        public string Aggregate { get; set; }
        public string Grade { get; set; }
    }

    public class EmployeeReferenceInfo
    {
        public string EmployeeReferenceInfoId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
    }


    public class MedicalEmergencyInfo
    {
        public string MedicalEmergencyInfoId { get; set; }
        public MedicalContactInfo MedicalContactInfo { get; set; }
        public ICollection<EmergencyContactInfo> EmergencyContactInfos { get; set; }

    }

    public class EmergencyContactInfo
    {
        public string EmergencyContactInfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Relationship Relationship { get; set; }
        public Address Address { get; set; }
        public string HomePhone { get; set; }
        public string ContactPhone { get; set; }
        public string CellPhone { get; set; }
    }

    public class MedicalContactInfo
    {
        public string MedicalContactInfoId { get; set; }
        public string Physician { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodGroup { get; set; }
    }
}

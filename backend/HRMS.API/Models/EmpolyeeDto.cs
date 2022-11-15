using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.API.Models
{

    public abstract class AuditableDto
    {
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public abstract class IUserProfileDto : AuditableDto
    {
        [Required]
        public string UserId { get; set; }
    }

    public class UserProfileDto : IUserProfileDto
    {
        public UserBasicInfoDto UserBasicInfo { get; set; }
        public UserContactInfoDto UserContactInfo { get; set; }
    }

    public class UserBasicInfoDto : IUserProfileDto
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
        
        [Required]
        public DateTime DOB { get; set; }

        public string AadhaarName { get; set; }
        public string AadhaarNumber { get; set; }
        public string AadhaarAttachmentUrl { get; set; }
        public IFormFile AadhaarFile { get; set; }
        public string PANNumber { get; set; }
        public IFormFile PANFile { get; set; }
        public string PANAttachmentUrl { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string GuardianType { get; set; }

        [Required]
        public string GuardianName { get; set; }

        public string PassportNumber { get; set; }
        public string ValidVisaInformation { get; set; }
    }



    public class UserContactInfoDto : IUserProfileDto
    {
        public AddressDto CorrespondenceAddress { get; set; }
        public AddressDto PermanentAddress { get; set; }
        public string CorrespondenceAddressId { get; set; }
        public string PermanentAddressId { get; set; }
        public bool IsCorrespondenceSameAsPermanent { get; set; }
        public string HomePhone { get; set; }

        [Required]
        [Phone]
        public string MobilePhone { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }




 

    public class AddressDto
    {
        public string AddressId { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
    }

    public class PassportInfoDto
    {
        public string PassportInfoId { get; set; }
        public string PassportNumber { get; set; }
        public string AnyValidVisa { get; set; }
    }

    public class BankInfoDto
    {
        public string BankInfoId { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IFSCode { get; set; }
        public string BankAddress { get; set; }
    }

    public class SpouseInfoDto
    {
        public string SpouseInfoId { get; set; }
        public string SpouseName { get; set; }
        public string EmployerName { get; set; }
        public string WorkPhone { get; set; }
    }

    public class UserPersionalInfoDto
    {
        public string UserPersionalInfo { get; set; }
        public string PersonName { get; set; }
        public DateTime PersonDOB { get; set; }
        public RelationshipDto PersonRelationship { get; set; }
    }

    public class FamilyInfoDto
    {
        public string FamilyInfoId { get; set; }
        public string PersonName { get; set; }
        public DateTime PersonDOB { get; set; }
        public RelationshipDto PersonRelationship { get; set; }
    }

    public class PFNomineeInfoDto
    {
        public string PFNomineeInfoId { get; set; }
        public DependentInfoDto Nominee { get; set; }

        public string NomineeShare { get; set; }

    }

    public class DependentInfoDto
    {
        public string DependentInfoId { get; set; }
        public RelationshipDto DependentRelationship { get; set; }

        public string DependentName { get; set; }
        public DateTime DependentDOB { get; set; }
    }
   

    public enum RelationshipDto
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


    //guardian
    public class PreviousJobInfoDto
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

    public class CurrentEmployemntInfoDto
    {
        public string CurrentEmployemntInfoId { get; set; }
        public DateTime DOJ { get; set; }
        public double GrossSalary { get; set; }
        public string Location { get; set; }
        public string Project { get; set; }
    }

    public enum InstitutionTypeDto
    {
        University=1,
        College = 2,
        Institution = 3,
        School = 4
    }

    public enum ProgramTypeDto
    {
        FullTime =1,
        PartTime =2,
        Distance = 3,
        Correspondence = 4,
        other = 5
    }

    public class EducationInfoDto
    {
        public string EducationInfoId { get; set; }
        public string ProgramName { get; set; }
        public string InstitutionType { get; set; }

        public DateTime YearOfCompletion { get; set; }
        public ProgramTypeDto ProgramType { get; set; }
        public string Aggregate { get; set; }
        public string Grade { get; set; }
    }

    public class EmployeeReferenceInfoDto
    {
        public string EmployeeReferenceInfoId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
    }


    public class MedicalEmergencyInfoDto
    {
        public string MedicalEmergencyInfoId { get; set; }
        public MedicalContactInfoDto MedicalContactInfo { get; set; }
        public ICollection<EmergencyContactInfoDto> EmergencyContactInfos { get; set; }

    }

    public class EmergencyContactInfoDto
    {
        public string EmergencyContactInfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RelationshipDto Relationship { get; set; }
        public AddressDto Address { get; set; }
        public string HomePhone { get; set; }
        public string ContactPhone { get; set; }
        public string CellPhone { get; set; }
    }

    public class MedicalContactInfoDto
    {
        public string MedicalContactInfoId { get; set; }
        public string Physician { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodGroup { get; set; }
    }
}

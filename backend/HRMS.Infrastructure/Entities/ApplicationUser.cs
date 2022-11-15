using HRMS.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace HRMS.Entities
{
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public string FullName { get; set; }

        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public UserProfile UserProfile { get; set; }

        public ProfileStatus ProfileStatus { get; set; }

        public UserStatus UserStatus { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    //Last activity

    public abstract class IUserProfile : Auditable
    {
        public string UserId { get; set; }
    }

    public class UserProfile : IUserProfile
    {
        public ApplicationUser User { get; set; }
        public UserBasicInfo UserBasicInfo { get; set; }
        public UserContactInfo UserContactInfo { get; set; }
    }

    public class ProfileReview: IUserProfile
    {
        public string ProfileReviewId { get; set; }
        public string Comment { get; set; }
    }

    public enum UserStatus
    {
        None = 0,
        Invited = 1,
        Active = 2,
        Deactivated = 3,
        Revoked = 4,

    }

    public enum ProfileStatus
    {
        None = 0,
        Pending = 1,
        InProgress = 2,
        Submitted = 3,
        RequestedForEdit = 4,
        Completed = 5
    }
    
    public class UserBasicInfo : IUserProfile 
    {
        public UserProfile UserProfile { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DOB { get; set; }

        public string AadhaarName { get; set; }
        public string AadhaarNumber{ get; set; }
        public string AadhaarAttachmentUrl { get; set; }
        public string PANNumber { get; set; }
        public string PANAttachmentUrl { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public string ValidVisaInformation { get; set; }

        public GuardianType GuardianType { get; set; }
        public string GuardianName { get; set; }
    }

    public class UserContactInfo : IUserProfile
    {
        public UserProfile UserProfile { get; set; }

        public UserAddress CorrespondenceAddress { get; set; }

        public UserAddress PermanentAddress { get; set; }

        public string CorrespondenceAddressId { get; set; }
        public string PermanentAddressId { get; set; }

        public bool IsCorrespondenceSameAsPermanent { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class UserAddress
    {
        public string UserAddressId { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
    }
}

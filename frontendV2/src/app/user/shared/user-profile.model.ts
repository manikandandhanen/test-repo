export abstract class Auditable {
    dateCreated: Date;
    dateModified: Date;
    userCreated: string;
    userModified: string;
}

export abstract class IUserProfile extends Auditable {
    userId: string;
}

export class UserProfile extends IUserProfile {
    UserBasicInfo: UserBasicInfo;
    UserContactInfo: UserContactInfo;
}

export class UserBasicInfo extends IUserProfile {
    firstName: string;
    middleName: string;
    lastName: string;
    profilePictureFile: File;
    profilePictureUrl: string;
    dob: Date;
    aadhaarName: string;
    aadhaarNumber: string;
    aadhaarFile: File;
    aadhaarAttachmentUrl: string;
    panNumber: string;
    panFile: File;
    panAttachmentUrl: string;
    nationality: string;
    //GaurdianType
    guardianType: string;
    guardianName: string;
    passportNumber: string;
    validVisaInformation: string;
}
//GaurdianType
//guardian
export enum GaurdianType {
    Father,
    Husband,
    Guardian,
}


export class CorrespondenceAddress {
    userAddressId: string;
    homePhone: string;
    mobilePhone: string;
    email: string;
    street: string;
    apartment: string;
    city: string;
    state: string;
    pincode: string;
}

export class PermanentAddress {
    userAddressId: string;
    street: string;
    apartment: string;
    city: string;
    state: string;
    pincode: string;
}

export class UserContactInfo extends IUserProfile {
    CorrespondenceAddress: UserAddress;
    PermanentAddress: UserAddress;
    CorrespondenceAddressId: string;
    PermanentAddressId: string;
    IsCorrespondenceSameAsPermanent: boolean;
    HomePhone: string;
    MobilePhone: string;
    Email: string;
}

export class UserAddress {
    UserAddressId: string;
    Street: string;
    Apartment: string;
    City: string;
    State: string;
    PinCode: string;
}
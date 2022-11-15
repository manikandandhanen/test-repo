export class BasicInfo {
    userId: string;
    firstName: string;
    middleName: string;
    lastName: string;
    profilePictureUrl: string;
    profilePictureFile: File;
    dob: Date;
    aadhaarName: string;
    aadhaarNumber: string;
    aadhaarFile: File;
    aadhaarAttachmentUrl: string;
    panNumber: string;
    panFile: File;
    panAttachmentUrl: string;
    nationality: string;
    guardianType: string;
    guardianName: string;
    passportNumber: string;
    validVisaInformation: string;
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

export class PersonalInfo {
    userId: string;
    name: string;
    relationshipType: Selection;
    dob: Date;
}

export class SpouseInfo {
    userId: string;
    maritialStatus: string;
    spouseName: string;
    spouseEmployer: string;
    spousePhone: string;
}

export class ReferenceInfo {
    userId: string;
    referenceName: string;
    mobileNumber: string;
    relationshipType: Selection;
}

export class JobHistory {
    userId: string;
    position: string;
    companyName: string;
    companyAddress: string;
    numberOfYears: string;
    period: string;
    ctc: string;

}

export class Educational {
    userId: string;
    university: string;
    completionYear: string;
    program: string;
    aggregate: string;
    grade: string;
}

export class Nominee {
    userId: string;
    nomineeRelationship: string;
    nomineeName: string;
    dob: string;
    gender: string;
    nomineeShare: string;

}

export class Dependent {
    userId: string;
    dependentRelationship: string;
    name: string;
    dob: string;
    gender: string;

}

export class DateOfJoiningInfo {
    userId: string;
    doj: string;
    salary: number;
    location: string;
    project: string;
}

export class BankInfo {
    userId: string;
    bankName: string;
    accountNumber: number;
    ifscCode: string;
    nameInBank: string;
}

export class EmergencyContactInfo {
    userId: string;
    physician: string;
    phoneNumber: number;
    bloodGroup: string;
    firstName: string;
    lastName: string;
    relationship: string;
    street: string;
    city: string;
    state: string;
    pincode: string;
    homePhone: number;
    workPhone: number;
    mobilePhone: number;
}

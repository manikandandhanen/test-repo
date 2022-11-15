export class InviteUser {
  email: string;
  userRole: string;
  requestProfile: boolean;
}

export class UserStatus {
  userId: string;
  status: string;
}

export abstract class Auditable {
  dateCreated: Date;
  dateModified: Date;
  userCreated: string;
  userModified: string;
}

export class User extends Auditable {
  userId: string;
  userName: string;
  password: string;
  email: string;
  fullName: string;
  userRoles: string[];
  phoneNumber: string;
  profileStatus: ProfileStatus;
  userStatus: string;
}

export abstract class IUserProfile extends Auditable {
  userId: string;
}

export class UserProfile extends IUserProfile {
  userBasicInfo: UserBasicInfo;
  userContactInfo: UserContactInfo;
}

export class UserBasicInfo extends IUserProfile {
  firstName: string;
  middleName: string;
  lastName: string;
  profileImageFile: File;
  profileImageUrl: string;
  dob: Date;
  aadharName: string;
  aadharFile: File;
  aadharNumber: string;
  aadharAttachmentUrl: string;
  panNumber: string;
  panFile: File;
  panAttachmentUrl: string;
  nationality: string;
  passportNumber: string;
  validVisaInformation: string;
  guardianType: string;
  guardianName: string;
}

export enum GuardianType {
  Father,
  Husband,
  Guardian,
}

export enum ProfileStatus {
  None = 0,
  Pending = 1,
  InProgress = 2,
  Submitted = 3,
  RequestedForEdit = 4,
  Completed = 5,
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

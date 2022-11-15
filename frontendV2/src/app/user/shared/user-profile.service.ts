import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { UserBasicInfo, UserProfile } from './user-profile.model';
import {
  BankInfo, CorrespondenceAddress, PermanentAddress, BasicInfo, DateOfJoiningInfo,
  Dependent, Educational, EmergencyContactInfo, JobHistory, Nominee, PersonalInfo,
  ReferenceInfo, SpouseInfo
} from './user.model';


@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  //GET: api/v1/user/profile
  //GET: api/v1/user/profile/id
  //api/v1/user/profile/id
  apiEndpoint: string = `${environment.API}/api/v1/user/profile`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<UserProfile[]> {
    return this.http.get<UserProfile[]>(`${this.apiEndpoint}`);
  }

  getById(id: string): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiEndpoint}/${id}`);
  }

  getResourceFileData(filename: string): Observable<string> {
    return this.http.get<string>(`${this.apiEndpoint}/reources/${encodeURI(filename)}`);
  }

  saveUserBasicInfo(basicInfo: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiEndpoint}/basicInfo`, basicInfo);
  }

  saveSampleUpload(formdata: any ): Observable<any> {
    return this.http.post<any>(`${this.apiEndpoint}/basicInfo/upload`, formdata);
  }

  saveUserCorrespondenceAddressInfo(correspondenceAddress: CorrespondenceAddress): Observable<CorrespondenceAddress> {
    return this.http.post<CorrespondenceAddress>(`${this.apiEndpoint}/contactInfo`, correspondenceAddress);
  }

  saveUserPermanentAddressInfo(permanentAddress: PermanentAddress): Observable<PermanentAddress> {
    return this.http.post<PermanentAddress>(`${this.apiEndpoint}/permanentAddress`, permanentAddress);
  }

  saveUserPersonalInfo(personalInfo: PersonalInfo): Observable<PersonalInfo> {
    return this.http.post<PersonalInfo>(`${this.apiEndpoint}/personalInfo`, personalInfo);
  }

  saveUserSpouseInfo(spouseInfo: SpouseInfo): Observable<SpouseInfo> {
    return this.http.post<SpouseInfo>(`${this.apiEndpoint}/spouseInfo`, spouseInfo);
  }

  saveUserReferenceInfo(referenceInfo: ReferenceInfo): Observable<ReferenceInfo> {
    return this.http.post<ReferenceInfo>(`${this.apiEndpoint}/referenceInfo`, referenceInfo);
  }

  saveUserJobHistoryInfo(jobHistory: JobHistory): Observable<JobHistory> {
    return this.http.post<JobHistory>(`${this.apiEndpoint}/jobHistoryInfo`, jobHistory);
  }

  saveUserEducationalInfo(educational: Educational): Observable<Educational> {
    return this.http.post<Educational>(`${this.apiEndpoint}/educationalInfo`, educational);
  }

  saveUserNomineeInfo(nominee: Nominee): Observable<Nominee> {
    return this.http.post<Nominee>(`${this.apiEndpoint}/nomineeInfo`, nominee);
  }

  saveUserDependentInfo(dependent: Dependent): Observable<Dependent> {
    return this.http.post<Dependent>(`${this.apiEndpoint}/dependentInfo`, dependent);
  }

  saveUserDateOfJoiningInfo(dateOfJoiningInfo: DateOfJoiningInfo): Observable<DateOfJoiningInfo> {
    return this.http.post<DateOfJoiningInfo>(`${this.apiEndpoint}/dateOfJoiningInfo`, dateOfJoiningInfo);
  }

  saveUserBankInfo(bankInfo: BankInfo): Observable<BankInfo> {
    return this.http.post<BankInfo>(`${this.apiEndpoint}/bankInfo`, bankInfo);
  }

  saveUserEmergencyContactInfo(emergencyContactInfo: EmergencyContactInfo): Observable<EmergencyContactInfo> {
    return this.http.post<EmergencyContactInfo>(`${this.apiEndpoint}/emergencyContactInfo`, emergencyContactInfo);
  }
}

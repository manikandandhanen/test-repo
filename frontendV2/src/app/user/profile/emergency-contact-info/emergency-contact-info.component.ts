import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { EmergencyContactInfo } from 'app/user/shared/user.model';


@Component({
  selector: 'emergency-contact-info',
  templateUrl: './emergency-contact-info.component.html'
})
export class EmergencyContactInfoComponent implements OnInit {

  emergencyInfo: EmergencyContactInfo = new EmergencyContactInfo();
  emergencyForm: UntypedFormGroup;

  constructor(private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.emergencyForm = new UntypedFormGroup({
      physician: new UntypedFormControl(''),
      phoneNumber: new UntypedFormControl(''),
      bloodGroup: new UntypedFormControl(''),
      firstName: new UntypedFormControl(''),
      lastName: new UntypedFormControl(''),
      relationship: new UntypedFormControl(''),
      street: new UntypedFormControl(''),
      city: new UntypedFormControl(''),
      state: new UntypedFormControl(''),
      pincode: new UntypedFormControl(''),
      homePhone: new UntypedFormControl(''),
      workPhone: new UntypedFormControl(''),
      mobilePhone: new UntypedFormControl(''),
    });
  }

  onEmergencySubmit(): void {
    this.userProfileService.saveUserEmergencyContactInfo(this.emergencyInfo).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}

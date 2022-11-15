import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, UntypedFormArray, UntypedFormControl, Validators } from '@angular/forms';
import { Nominee } from 'app/user/shared/user.model';
import { UserProfileService } from 'app/user/shared/user-profile.service';

@Component({
  selector: 'nominee-info',
  templateUrl: './nominee-info.component.html'
})
export class NomineeInfoComponent implements OnInit {

  nomineeForm = new UntypedFormGroup({});
  nominee: Nominee = new Nominee();
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.nomineeForm = this.fb.group({
      userArray: new UntypedFormArray([this.getUserForm()])
    });
  }

  get userArray() {
    return (<UntypedFormArray>this.nomineeForm.get('userArray'));
  }

  addUser() {
    if (this.nomineeForm.valid) {
      this.userArray.push(this.getUserForm());
    }
  }

  removeUser(i: number) {
    this.userArray.removeAt(i);
  }

  getUserForm() {
    return this.fb.group({
      nomineeRelationship: new UntypedFormControl('', [
        Validators.required
      ]),
      nomineeName: new UntypedFormControl('', [
        Validators.required
      ]),
      dob: new UntypedFormControl('', [
        Validators.required
      ]),
      gender: new UntypedFormControl('', [
        Validators.required
      ]),
      nomineeShare: new UntypedFormControl('', [
        Validators.required
      ])
    });
  }

  onNomineeSubmit(): void {
    this.userProfileService.saveUserNomineeInfo(this.nominee).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}

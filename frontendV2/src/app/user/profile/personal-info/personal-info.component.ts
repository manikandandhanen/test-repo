import { Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, UntypedFormArray, UntypedFormControl, Validators } from '@angular/forms';
import { PersonalInfo } from 'app/user/shared/user.model';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { PersonalComponent } from './personal.component';

@Component({
  selector: 'personal-info',
  templateUrl: './personal-info.component.html',
})
export class PersonalInfoComponent implements OnInit {
  @ViewChild(PersonalComponent ) submit: PersonalComponent ;

  personalForm = new UntypedFormGroup({});

  personal: PersonalInfo = new PersonalInfo();

  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.personalForm = this.fb.group({
      userArray: new UntypedFormArray([this.getUserForm()])
    });
  }

  ngAfterViewInit() {
    // child is set
   // this.submit.onPersonalSubmit();
  }

  get userArray() {
    return (<UntypedFormArray>this.personalForm.get('userArray'));
  }

  addUser(): void {
    if (this.personalForm.valid) {
      this.userArray.push(this.getUserForm());
    }
  }

  removeUser(i: number): void {
    this.userArray.removeAt(i);
  }

  getUserForm() {
    return this.fb.group({
      name: new UntypedFormControl('', [
        Validators.required
      ]),
      relationshipType: new UntypedFormControl('', [
        Validators.required
      ]),
      dob: new UntypedFormControl('', [
        Validators.required
      ]),
    });
  }

  // onPersonalSubmit(): void {
  //   this.userProfileService.saveUserPersonalInfo(this.personal).subscribe({
  //     next: (data: any) => {
  //       //Alert service
  //     },
  //     error: (err: any) => {
  //       //alert service 
  //     }
  //   });
  // }
}



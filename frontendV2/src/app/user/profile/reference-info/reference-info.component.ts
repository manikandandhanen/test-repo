import { Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, UntypedFormArray, UntypedFormControl, Validators } from '@angular/forms';
import { ReferenceInfo } from 'app/user/shared/user.model';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { ReferenceComponent } from './referencecomponent';

@Component({
  selector: 'reference-info',
  templateUrl: './reference-info.component.html'
})
export class ReferenceInfoComponent implements OnInit {
  @ViewChild(ReferenceComponent) submit: ReferenceComponent;
  referenceForm = new UntypedFormGroup({});
  reference: ReferenceInfo = new ReferenceInfo();
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.referenceForm = this.fb.group({
      userArray: new UntypedFormArray([this.getUserForm()])
    });
  }

  ngAfterViewInit(): void {
    // child is set
    this.submit.onReferenceSubmit();
  }

  get userArray() {
    return (<UntypedFormArray>this.referenceForm.get('userArray'));
  }

  addUser(): void {
    if (this.referenceForm.valid) {
      this.userArray.push(this.getUserForm());
    }
  }

  removeUser(i: number): void {
    this.userArray.removeAt(i);
  }

  getUserForm() {
    return this.fb.group({
      referenceName: new UntypedFormControl('', [
        Validators.required
      ]),
      mobileNumber: new UntypedFormControl('', [
        Validators.required
      ]),
      relationshipType: new UntypedFormControl('', [
        Validators.required
      ])
    });
  }

  // onReferenceSubmit(): void {
  //   this.userProfileService.saveUserReferenceInfo(this.reference).subscribe({
  //     next: (data: any) => {
  //       //Alert service
  //     },
  //     error: (err: any) => {
  //       //alert service 
  //     }
  //   });
  // }
}

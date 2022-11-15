import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { DateOfJoiningInfo } from 'app/user/shared/user.model';


@Component({
  selector: 'date-of-joining-info',
  templateUrl: './date-of-joining-info.component.html'
})
export class DateOfJoiningInfoComponent implements OnInit {
  dateOfJoiningForm: UntypedFormGroup;
  
  dojInfo: DateOfJoiningInfo = new DateOfJoiningInfo();

  constructor(private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.dateOfJoiningForm = new UntypedFormGroup({
      doj: new UntypedFormControl('', [Validators.required
      ]),
      salary: new UntypedFormControl('', [
        Validators.required
      ]),
      location: new UntypedFormControl('', [
        Validators.required
      ]),
      project: new UntypedFormControl('', [
        Validators.required
      ])
    });
  }

  onDateOfJoiningSubmit(): void {
    this.userProfileService.saveUserDateOfJoiningInfo(this.dojInfo).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}

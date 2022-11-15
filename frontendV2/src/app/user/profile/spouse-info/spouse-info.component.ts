import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { SpouseInfo } from 'app/user/shared/user.model';


@Component({
  selector: 'spouse-info',
  templateUrl: './spouse-info.component.html'
})

export class SpouseInfoComponent implements OnInit {
  spouseForm: UntypedFormGroup;
  spouse: SpouseInfo = new SpouseInfo();

  constructor(private userProfileService: UserProfileService) { }

  ngOnInit(): void {
    this.spouseForm = new UntypedFormGroup({
      maritialStatus: new UntypedFormControl(''),
      spouseName: new UntypedFormControl(''),
      spouseEmployer: new UntypedFormControl(''),
      spousePhone: new UntypedFormControl('')
    });
  }

  onSpouseSubmit(): void {
    this.userProfileService.saveUserSpouseInfo(this.spouse).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}

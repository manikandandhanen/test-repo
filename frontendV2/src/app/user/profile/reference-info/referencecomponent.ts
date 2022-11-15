import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { ReferenceInfo } from 'app/user/shared/user.model';


@Component({
  selector: 'reference',
  templateUrl: './reference.component.html'
})

export class ReferenceComponent implements OnInit {

  @Input() inputFormGroup = this.fb.group({});
  reference: ReferenceInfo = new ReferenceInfo();
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void { }

  onReferenceSubmit(): void {
    // this.userProfileService.saveUserReferenceInfo(this.reference).subscribe({
    //   next: (data: any) => {
    //     //Alert service
    //   },
    //   error: (err: any) => {
    //     //alert service 
    //   }
    // });
  }
}
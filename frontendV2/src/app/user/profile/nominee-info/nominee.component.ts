import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { Nominee } from 'app/user/shared/user.model';

@Component({
  selector: 'nominee',
  templateUrl: './nominee.component.html'
})

export class NomineeComponent implements OnInit {

  @Input() inputFormGroup = this.fb.group({});
  nominee: Nominee = new Nominee();
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void { }

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
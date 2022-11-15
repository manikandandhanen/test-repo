import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { PersonalInfo } from 'app/user/shared/user.model';

@Component({
  selector: 'personal',
  templateUrl: './personal.component.html'
})

export class PersonalComponent implements OnInit {

  @Input() inputFormGroup = this.fb.group({});

  personal: PersonalInfo = new PersonalInfo();
  
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void { }

  onPersonalSubmit(): void {
    // this.userProfileService.saveUserPersonalInfo(this.personal).subscribe({
    //   next: (data: any) => {
    //     //Alert service
    //   },
    //   error: (err: any) => {
    //     //alert service 
    //   }
    // });
  }
}
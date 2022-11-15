import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { Dependent } from 'app/user/shared/user.model';


@Component({
  selector: 'dependent',
  templateUrl: './dependent.component.html'
})

export class DependentComponent implements OnInit {
  dependent: Dependent = new Dependent();
  @Input() inputFormGroup = this.fb.group({});

  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void { }

  onDependentSubmit(): void {
    this.userProfileService.saveUserDependentInfo(this.dependent).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}
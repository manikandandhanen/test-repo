import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { JobHistory } from 'app/user/shared/user.model';

@Component({
  selector: 'job-history',
  templateUrl: './job-history.component.html',
  styleUrls: ['./job-history.component.scss']
})

export class JobHistoryComponent implements OnInit {

  @Input() inputFormGroup = this.fb.group({});
  jobhistory: JobHistory = new JobHistory();
  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService) { }

  ngOnInit(): void { }

  onJobHistorySubmit(): void {
    this.userProfileService.saveUserJobHistoryInfo(this.jobhistory).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}
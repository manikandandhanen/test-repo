import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { BankInfo } from 'app/user/shared/user.model';



@Component({
  selector: 'bank-info',
  templateUrl: './bank-info.component.html'
})
export class BankInfoComponent implements OnInit {

  bankForm: UntypedFormGroup;
  bankInfo: BankInfo = new BankInfo();
  

  constructor(private userProfileService: UserProfileService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.bankForm = new UntypedFormGroup({
      bankName: new UntypedFormControl('', [Validators.required
      ]),
      accountNumber: new UntypedFormControl('', [
        Validators.required
      ]),
      ifscCode: new UntypedFormControl('', [
        Validators.required
      ]),
      nameInBank: new UntypedFormControl('', [
        Validators.required
      ])
    });
  }

  onBankSubmit(): void {
    this.userProfileService.saveUserBankInfo(this.bankInfo).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }
}
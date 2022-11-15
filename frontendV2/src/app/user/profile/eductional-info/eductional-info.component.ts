import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { UntypedFormArray, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { Educational } from 'app/user/shared/user.model';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { EductionalComponent } from './eductional.component';
import { MatDialog } from '@angular/material/dialog';
import * as moment from 'moment';

@Component({
  selector: 'eductional-info',
  templateUrl: './eductional-info.component.html'
})
export class EductionalInfoComponent implements OnInit {
  urls = [];
  @ViewChild(EductionalComponent ) submit: EductionalComponent ;
  @ViewChild(EductionalComponent ) fileUpload: EductionalComponent ; 

  @ViewChild('pdfDialog', { static: true }) aadhaarDialog: TemplateRef<any>;

  educationalForm = new UntypedFormGroup({});

  educational: Educational = new Educational();

  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.educationalForm = this.fb.group({
      userArray: new UntypedFormArray([this.getUserForm()])
    });
  }

  ngAfterViewInit(): void {
    // child is set
   // this.fileUpload.onSelectFile(event);
   
    this.submit.onEducationalSubmit();
  }

  get userArray() {
    return (<UntypedFormArray>this.educationalForm.get('userArray'));
  }

  addUser(): void  {
    if (this.educationalForm.valid) {
      this.userArray.push(this.getUserForm());
    }
  }

  removeUser(i: number): void  {
    this.userArray.removeAt(i);
  }

  getUserForm() {
    return this.fb.group({
      university: new UntypedFormControl('', [
        Validators.required
      ]),
      completionYear: new UntypedFormControl('', [
        Validators.required
      ]),
      //completionYear : new UntypedFormControl(moment()),
      program: new UntypedFormControl('', [
        Validators.required
      ]),
      aggregate: new UntypedFormControl('', [
        Validators.required
      ]),
      grade: new UntypedFormControl('', [
        Validators.required
      ])
    });
  }

  openDialog(): void {
    this.dialog.open(this.aadhaarDialog);
  }
}




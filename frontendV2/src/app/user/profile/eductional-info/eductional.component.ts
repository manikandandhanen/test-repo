import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UserProfileService } from 'app/user/shared/user-profile.service';
import { Educational } from 'app/user/shared/user.model';
import { EductionalInfoComponent } from './eductional-info.component';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';

export const MY_FORMATS = {
  parse: {
    dateInput: 'YYYY',
  },
  display: {
    dateInput: 'YYYY',
    monthYearLabel: 'YYYY',
    monthYearA11yLabel: 'YYYY',
  },
};



@Component({
  selector: 'eductional',
  templateUrl: './eductional.component.html',
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { 
     provide: MAT_DATE_FORMATS, useValue: MY_FORMATS
    },
   ]
})

export class EductionalComponent implements OnInit {
  urls = [];

  @ViewChild('pdfDialog', { static: true }) aadhaarDialog: TemplateRef<any>;
  @ViewChild(EductionalInfoComponent ) fileUpload: EductionalInfoComponent ; 
  @Input() inputFormGroup = this.fb.group({});

  educational: Educational = new Educational();

  constructor(private fb: UntypedFormBuilder, private userProfileService: UserProfileService,  private dialog: MatDialog) { }

  ngOnInit(): void {
   // this.fileUpload.urls;
   }

  onEducationalSubmit(): void {
    this.userProfileService.saveUserEducationalInfo(this.educational).subscribe({
      next: (data: any) => {
        //Alert service
      },
      error: (err: any) => {
        //alert service 
      }
    });
  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
        var filesAmount = event.target.files.length;
        for (let i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = (event:any) => {
                  console.log(event.target.result);
                   this.urls.push(event.target.result); 
                }

                reader.readAsDataURL(event.target.files[i]);
        }
    }
  }

  openDialog(): void {
    this.dialog.open(this.aadhaarDialog);
  }

  @ViewChild('picker', { static: false })
  private picker!: MatDatepicker<Date>;  

  chosenYearHandler(ev, input){
    let { _d } = ev;
    this.educational.completionYear = _d;
    this.picker.close()
  }
}
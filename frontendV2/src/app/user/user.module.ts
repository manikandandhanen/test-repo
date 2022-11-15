import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatRadioModule } from '@angular/material/radio';
import { MatStepperModule } from '@angular/material/stepper';
import { FlexLayoutModule } from '@angular/flex-layout';
import { QuillModule } from 'ngx-quill';
import { PdfViewerModule  } from 'ng2-pdf-viewer';
import { MatDialogModule } from '@angular/material/dialog';
import { FileUploadModule } from 'ng2-file-upload';

import { UserRoutes } from './user.routing';
import { ProfileComponent } from './profile/profile/profile.component';
import { BasicInfoComponent } from './profile/basic-info/basic-info.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { BankInfoComponent } from './profile/bank-info/bank-info.component';
import { SpouseInfoComponent } from './profile/spouse-info/spouse-info.component';
import { JobHistoryInfoComponent } from './profile/job-history-info/job-history-info.component';
import { PersonalInfoComponent } from './profile/personal-info/personal-info.component';
import { EductionalInfoComponent } from './profile/eductional-info/eductional-info.component';
import { CommunicationInfoComponent } from './profile/communication-info/communication-info.component';
import { NomineeInfoComponent } from './profile/nominee-info/nominee-info.component';
import { DependentInfoComponent } from './profile/dependent-info/dependent-info.component';
import { ReferenceInfoComponent } from './profile/reference-info/reference-info.component';
import { EmergencyContactInfoComponent } from './profile/emergency-contact-info/emergency-contact-info.component';
import { PersonalComponent } from './profile/personal-info/personal.component';
import { ReferenceComponent } from './profile/reference-info/referencecomponent';
import { DateOfJoiningInfoComponent } from './profile/date-of-joining-info/date-of-joining-info.component';
import { MatTableModule } from '@angular/material/table';
import { JobHistoryComponent } from './profile/job-history-info/job-history.component';
import { EductionalComponent } from './profile/eductional-info/eductional.component';
import { NomineeComponent } from './profile/nominee-info/nominee.component';
import { DependentComponent } from './profile/dependent-info/dependent.component';
import { FileUploadComponent } from './profile/file-upload/file-upload.component';



@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatListModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatRadioModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatStepperModule,
    FlexLayoutModule,
    QuillModule.forRoot(),
    RouterModule.forChild(UserRoutes),
    MatButtonToggleModule,
    MatTableModule,
    MatDialogModule,
    PdfViewerModule,
    FileUploadModule
  ],
  declarations: [
    ProfileComponent,
    BasicInfoComponent,
    BankInfoComponent,
    SpouseInfoComponent,
    JobHistoryInfoComponent,
    PersonalInfoComponent,
    EductionalInfoComponent,
    CommunicationInfoComponent,
    NomineeInfoComponent,
    DependentInfoComponent,
    ReferenceInfoComponent,
    EmergencyContactInfoComponent,
    PersonalComponent,
    ReferenceComponent,
    JobHistoryComponent,
    DateOfJoiningInfoComponent,
    EductionalComponent,
    NomineeComponent,
    DependentComponent,
    FileUploadComponent
  ],
  providers:[DatePipe]
})
export class UserModule { }

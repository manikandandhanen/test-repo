import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnInit,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import {
  GaurdianType,
  UserBasicInfo,
} from "app/user/shared/user-profile.model";
import { UserProfileService } from "app/user/shared/user-profile.service";
import { Validators, FormGroup, FormControl } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { MatStepper } from "@angular/material/stepper";
import { DatePipe } from "@angular/common";
import { BehaviorSubject } from "rxjs";

@Component({
  selector: "basic-info",
  templateUrl: "./basic-info.component.html",
  styleUrls: ["./basic-info.component.scss"],
})
export class BasicInfoComponent implements OnInit {
  //private _userBasicInfo: UserBasicInfo = new UserBasicInfo();

  private _userBasicInfo$ = new BehaviorSubject<UserBasicInfo>(
    new UserBasicInfo()
  );

  @Input()
  set userBasicInfo(value: UserBasicInfo) {
    this._userBasicInfo$.next(value);
  }

  basicInfo: UserBasicInfo = new UserBasicInfo();
  @ViewChild("stepper") stepper: MatStepper;
  //@ViewChild("aadhaarDialog", { static: true }) aadhaarDialog: TemplateRef<any>;

  defaultAvatorUrl: string = "https://www.w3schools.com/howto/img_avatar.png";
  FormGroupFileControlNameList: string[] = [
    "profilePictureFile",
    "panFile",
    "aadhaarFile",
  ];
  submitted: boolean;
  gaurdianTypes: (string | GaurdianType)[];
  selectedGuardianType: string = GaurdianType[GaurdianType.Father];
  basicForm: FormGroup;

  constructor(
    private _userProfileService: UserProfileService,
    private cd: ChangeDetectorRef,
    private dialog: MatDialog,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this._userBasicInfo$.subscribe((value) => {
      if (value) {
        this.basicInfo = value;
        this.basicForm.patchValue(value);

        if (value.profilePictureUrl) {
          this.basicForm.get("profilePictureFile").clearValidators();
        } else {
          this.basicForm
            .get("profilePictureFile")
            .addValidators(Validators.required);
        }
        this.basicForm.get("profilePictureFile").updateValueAndValidity();
      }
    });
    console.log("after ss=" + JSON.stringify(this.basicInfo));

    this.gaurdianTypes = this.getGaurdianTypes();
    this.basicForm = this.getBasicInfoFormGroup();

    this.basicInfo.guardianType = this.selectedGuardianType;
  }

  onFileChange(
    event: any,
    fileField: string,
    fileUrlField: string,
    preview: boolean
  ): void {
    if (event.target.files && event.target.files[0]) {
      let file = event.target.files[0];
      let reader = new FileReader();

      reader.readAsDataURL(file);

      reader.onload = () => {
        this.basicInfo[fileField] = file;
        this.basicInfo[fileUrlField] = preview ? reader.result : file.name;
        console.log(this.basicInfo[fileUrlField]);
      };

      this.cd.markForCheck();
    }
  }

  // getAttachmentUrl(resourceUrl: string) {
  //   let filedata = null;
  //   this._userProfileService
  //     .getResourceFileData(resourceUrl)
  //     .subscribe((value) => {
  //       filedata = filedata;
  //     });
  // }

  openDialog(templateRef: TemplateRef<any>): void {
    //templateRef.
    this.dialog.open(templateRef);
  }

  onBasicSubmit(): void {
    this.FormGroupFileControlNameList.forEach((formControlName) =>
      this.basicForm.controls[formControlName].markAsTouched({ onlySelf: true })
    );
    this.basicForm.markAsTouched({ onlySelf: true });

    if (this.basicForm.invalid) {
      return;
    }

    let basicInfoData = this.toFormData(this.basicForm.value);

    basicInfoData.set(
      "dob",
      this.datePipe.transform(basicInfoData.get("dob").toString(), "yyyy-MM-dd")
    );

    this._userProfileService.saveUserBasicInfo(basicInfoData).subscribe({
      next: (data: any) => {
        //Alert service
        this.stepper.next();
      },
      error: (err: any) => {
        //alert service
      },
    });
  }

  private toFormData(formGroupValue: any): FormData {
    const formData = new FormData();

    // all FormgroupFields except filefield
    let formControlNameList = Object.keys(formGroupValue).filter(
      (formControl) => !this.FormGroupFileControlNameList.includes(formControl)
    );

    // append value to formgroup
    for (const controlName of formControlNameList) {
      const value = formGroupValue[controlName];
      formData.append(controlName, value);
    }

    //read File Fields
    this.FormGroupFileControlNameList.forEach((fileField) => {
      if (this.basicInfo[fileField]) {
        formData.append(
          fileField,
          this.basicInfo[fileField],
          this.basicInfo[fileField].name
        );
      }
    });
    return formData;
  }

  private getBasicInfoFormGroup(): FormGroup {
    return new FormGroup({
      profilePictureFile: new FormControl(null, [Validators.required]),
      firstName: new FormControl("", [Validators.required]),
      userId: new FormControl(""),
      middleName: new FormControl(""),
      lastName: new FormControl("", [Validators.required]),
      dob: new FormControl("", [Validators.required]),
      aadhaarName: new FormControl("", [Validators.required]),
      aadhaarNumber: new FormControl("", [Validators.required]),
      aadhaarFile: new FormControl(null, [Validators.required]),
      panNumber: new FormControl("", [Validators.required]),
      panFile: new FormControl(null, [Validators.required]),
      nationality: new FormControl("", [Validators.required]),
      passportNumber: new FormControl(""),
      validVisaInformation: new FormControl(""),
      profilePictureUrl: new FormControl(""),
      panAttachmentUrl: new FormControl(""),
      aadhaarFileUrl: new FormControl(""),
      guardianType: new FormControl("", [Validators.required]),
      guardianName: new FormControl("", [Validators.required]),
    });
  }

  getGaurdianTypes(): (string | GaurdianType)[] {
    return Object.values(GaurdianType).filter(
      (value) => typeof value === "string"
    );
  }
}

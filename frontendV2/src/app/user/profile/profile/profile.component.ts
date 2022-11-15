import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { UserBasicInfo, UserProfile } from "app/admin/shared/user.model";
import { UserProfileService } from "app/user/shared/user-profile.service";
import { BehaviorSubject } from "rxjs";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
})
export class ProfileComponent implements OnInit {
  firstFormGroup: UntypedFormGroup;
  secondFormGroup: UntypedFormGroup;
  userProfile = new UserProfile();
  userBasicInfo = new UserBasicInfo();
  constructor(
    private fb: UntypedFormBuilder,
    private changeDetectorRef: ChangeDetectorRef,
    private _userProfileService: UserProfileService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {

   // let testEmitter$ = new BehaviorSubject<UserBasicInfo>(this.userBasicInfo);


    //read id as userid from routes
    this.userProfile.userId = this.route.snapshot.params["id"] || null;
    console.log("before");
    this._userProfileService.getById(this.userProfile.userId).subscribe({
      next: (userProfile: any) => {
        this.userProfile = userProfile;
      //  this.userBasicInfo = userProfile.userBasicInfo;
       // testEmitter$.next(this.userBasicInfo);
       // this.changeDetectorRef.markForCheck();
      //  console.log(this.userProfile);
        //console.log(this.userProfile.userBasicInfo);
        //console.log("inner  before");
        //this.userBasicInfo.firstName = "Nagesh Hunter";

        // if (!this.userProfile) {
        //   this.userProfile.UserBasicInfo = new UserBasicInfo();
        //   this.userProfile.UserBasicInfo.userId = this.userProfile.userId;
        // }
      },
      error: (err: any) => {
        // this.router.navigate(['/']);
      },
    });

    // console.log(this.userProfile);

    this.firstFormGroup = this.fb.group({
      firstCtrl: ["", Validators.required],
    });
    this.secondFormGroup = this.fb.group({
      secondCtrl: ["", Validators.required],
    });
  }

  submit() {
    console.log(this.firstFormGroup.value);
    console.log(this.secondFormGroup.value);
  }
}

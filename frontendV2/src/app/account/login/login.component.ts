import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  AfterViewInit,
  ElementRef,
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MatButton } from "@angular/material/button";
import { MatProgressBar } from "@angular/material/progress-bar";
import {
  Validators,
  UntypedFormGroup,
  UntypedFormControl,
  FormGroup,
  FormControl,
} from "@angular/forms";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { AppLoaderService } from "app/layouts/services/app-loader/app-loader.service";
import { JwtAuthService } from "app/layouts/services/auth/jwt-auth.service";
import { AuthenticationService } from "app/core/authentication/auth.service";
import {
  AuthResponseDto,
  UserForAuthenticationDto,
} from "../shared/account.model";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatProgressBar) progressBar: MatProgressBar;
  @ViewChild(MatButton) submitButton: MatButton;
  @ViewChild("input", { read: ElementRef })
  inputRef: ElementRef<HTMLInputElement>;
  signinForm: FormGroup;
  errorMessage: string = "";
  returnUrl: string;

  private _unsubscribeAll: Subject<any>;

  constructor(
    private authService: AuthenticationService,
    private matxLoader: AppLoaderService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this._unsubscribeAll = new Subject();
  }

  ngOnInit() {
    window.setTimeout(() => this.setValueFromBrowserAutofill());
    //incase if they entering to login
    this.authService.logout();
    this.signinForm = new FormGroup({
      username: new FormControl("", Validators.required),
      password: new FormControl("", Validators.required),
      rememberMe: new FormControl(true),
    });

    //this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
    this.route.queryParams
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((params) => (this.returnUrl = params["returnUrl"] || "/"));
  }

  ngAfterViewInit() {
    this.autoSignIn();
  }

  ngOnDestroy() {
    this._unsubscribeAll.next(1);
    this._unsubscribeAll.complete();
  }

  signin() {
    if (!this.signinForm.valid) {
      this.errorMessage = this.signinForm.errors[0];
      return;
    }
    const loginData = this.signinForm.value;

    this.submitButton.disabled = true;
    this.progressBar.mode = "indeterminate";

    const userForAuth: UserForAuthenticationDto = {
      userName: loginData.username,
      password: loginData.password,
    };

    this.authService.loginUser("api/v1/auth/login", userForAuth).subscribe({
      next: (res: AuthResponseDto) => {
        localStorage.setItem("token", res.token);
        this.authService.sendAuthStateChangeNotification(res.isAuthSuccessful);
        //  this.router.navigate([this.returnUrl]);
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err: HttpErrorResponse) => {
        this.submitButton.disabled = false;
        this.progressBar.mode = "determinate";

        this.errorMessage = err.error.errorMessage;

        //if autosignin
        if (this.matxLoader.isRunning) {
          this.matxLoader.close();
        }

        console.error("error caught in component");
      },
    });
    // this.jwtAuth.signin(signinData.username, signinData.password)
    // .subscribe(response => {
    //   this.router.navigateByUrl(this.jwtAuth.return);
    // }, err => {
    //   this.submitButton.disabled = false;
    //   this.progressBar.mode = 'determinate';
    //   this.errorMsg = err.message;
    //   // console.log(err);
    // })
  }

  autoSignIn() {
    if (this.returnUrl === "/") {
      return;
    }
    if (!this.signinForm.valid) {
      return;
    }
    this.matxLoader.open(
      `Automatically Signing you in! \n Return url: ${this.returnUrl.substring(
        0,
        20
      )}...`,
      { width: "320px" }
    );

    this.signin();
    console.log("autoSignIn");

    // setTimeout(() => {
    //   this.signin();
    //   console.log("autoSignIn");
    //   this.matxLoader.close();
    // }, 2000);
  }

  private setValueFromBrowserAutofill() {
    this.inputRef?.nativeElement.dispatchEvent(new Event("input"));
  }
}

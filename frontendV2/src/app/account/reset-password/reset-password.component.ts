import { Component, OnInit, ViewChild } from "@angular/core";
import { MatButton } from "@angular/material/button";
import { MatProgressBar } from "@angular/material/progress-bar";
import {
  Validators,
  UntypedFormGroup,
  UntypedFormControl,
} from "@angular/forms";
import { AuthenticationService } from "app/core/authentication/auth.service";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { User } from "app/admin/shared/user.model";
import Validation from "../shared/confirm-password.validators";

@Component({
  selector: "app-reset-password",
  templateUrl: "./reset-password.component.html",
  styleUrls: ["./reset-password.component.css"],
})
export class ResetPasswordComponent implements OnInit {
  @ViewChild(MatProgressBar) progressBar: MatProgressBar;
  @ViewChild(MatButton) submitButton: MatButton;

  user: User = null;
  isValidToken: boolean = false;
  resetPasswordForm: UntypedFormGroup;
  errorMsg: string;
  successMsg:string;

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.setupResetPasswordForm();

    this.route.queryParams.subscribe((params) => {
      let code = params["code"] || null;
      let userId = params["userId"] || null;

      if (code != null && userId != null) {
        this.authService
          .verifyResetPasswordToken(
            `api/v1/auth/verifyResetPasswordToken/?userid=${userId}&code=${encodeURIComponent(
              code
            )}`
          )
          .subscribe((res: any) => {
            if (res.isValid) {
              this.isValidToken = true;
              this.resetPasswordForm.patchValue({code:code,userId:userId});
            } else {
              this.errorMsg =
                "Invalid token, please contact your Administrator!";
              this.resetPasswordForm.disable({ onlySelf: true });
            }
          });
      } else {
        this.router.navigateByUrl("/404");
      }
    });
  }

  onResetPassword() {
    if (this.resetPasswordForm.invalid) {
      return;
    }
    this.errorMsg = "";
    const resetPasswordData = this.resetPasswordForm.value;

    this.submitButton.disabled = true;
    this.progressBar.mode = "indeterminate";

    this.authService
      .resetPassword("api/v1/auth/ResetPassword", resetPasswordData)
      .subscribe({
        next: (res) => {
          if(res.message)
          {
            this.progressBar.mode = "determinate";
            this.resetPasswordForm.reset();
            this.resetPasswordForm.disable({ onlySelf: true });
            this.successMsg = res.message;
          }
        },
        error: (err: any) => {
          this.submitButton.disabled = false;
          this.progressBar.mode = "determinate";
          if (err.status == 400) {
            if (err.error.errors) {
              let headers = Object.keys(err.error.errors);
              this.errorMsg = err.error.errors[headers[0]];
              return;
            }
            this.errorMsg = "Invalid request, Please contact administrator!";
          } else {
            this.errorMsg =
              "Something went wrong, Please contact administrator!";
          }
        },
      });
  }

  setupResetPasswordForm() {
    const password = new UntypedFormControl("",[
      Validators.required,
      Validators.pattern(
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/
      ),
      Validators.minLength(6),
    ]);
    const confirmPassword = new UntypedFormControl("", Validators.required);

    this.resetPasswordForm = new UntypedFormGroup({
      userId: new UntypedFormControl(""),
      code: new UntypedFormControl(""),
      email: new UntypedFormControl("", [
        Validators.required,
        Validators.email,
      ]),
      password: password,
      confirmPassword: confirmPassword,
      
    },{
      validators: [Validation.match('password', 'confirmPassword')]
    }
    );
  }
}

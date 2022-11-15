import { Component, OnInit, ViewChild } from "@angular/core";
import { MatButton } from "@angular/material/button";
import { MatProgressBar } from "@angular/material/progress-bar";
import { Validators, FormGroup, FormControl } from "@angular/forms";
import { AppLoaderService } from "app/layouts/services/app-loader/app-loader.service";
import { AuthenticationService } from "app/core/authentication/auth.service";
import { AuthResponseDto } from "../shared/account.model";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html",
  styleUrls: ["./forgot-password.component.css"],
})
export class ForgotPasswordComponent implements OnInit {
  @ViewChild(MatProgressBar) progressBar: MatProgressBar;
  @ViewChild(MatButton) submitButton: MatButton;
  forgotPasswordForm: FormGroup;
  errorMessage: string = "";
  successMessage: string = "";

  constructor(
    private authService: AuthenticationService,
    private matxLoader: AppLoaderService
  ) {}

  ngOnInit() {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
    });
  }

  signin() {
    if (!this.forgotPasswordForm.valid) {
      this.errorMessage = this.forgotPasswordForm.errors[0];
      return;
    }
    const forgotPasswordData = this.forgotPasswordForm.value;

    this.submitButton.disabled = true;
    this.progressBar.mode = "indeterminate";

    this.authService
      .forgotPassword("api/v1/auth/forgotpassword", forgotPasswordData)
      .subscribe({
        next: (res: any) => {
          this.successMessage = res.message;
          this.progressBar.mode = "determinate";

        },
        error: (err: HttpErrorResponse) => {
          this.submitButton.disabled = false;
          this.progressBar.mode = "determinate";

          if (err.status == 400) {
            this.errorMessage =
              "Invalid Request, Please contact Administrator!";
          } else {
            this.errorMessage = err.message;
          }

          console.error("error caught");
        },
      });
  }
}

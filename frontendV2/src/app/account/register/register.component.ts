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
import { Subject, takeUntil } from "rxjs";

import { el, tr } from "date-fns/locale";
import { User } from "app/admin/shared/user.model";
import Validation from "../shared/confirm-password.validators";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent implements OnInit {
  @ViewChild(MatProgressBar) progressBar: MatProgressBar;
  @ViewChild(MatButton) submitButton: MatButton;

  user: User = null;
  isValidToken: boolean = false;
  registerForm: UntypedFormGroup;
  errorMsg: string;
  registrationConfirmed: boolean = false;
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.setupRegisterForm();

    this.route.queryParams.subscribe((params) => {
      console.log(params); // { category: "fiction" }

      let code = params["code"] || null;

      let userId = this.route.snapshot.params["id"] || null;

      if (code != null && userId != null) {
        this.authService
          .verifyRegistrationToken(
            `api/v1/auth/verifyRegistrationToken/?userid=${userId}&code=${encodeURIComponent(
              code
            )}`
          )
          .subscribe({
            next: (user: User) => {
              if (user != null) {
                this.user = user;
                this.registerForm.patchValue({
                  userId: user.userId,
                  email: user.email,
                  code: code,
                });

                this.isValidToken = true;
                this.registerForm.controls['email'].disable();
              } else {
                this.errorMsg =
                  "Invalid token, please contact your Administrator!";
                this.registerForm.disable({ onlySelf: true });
              }
            },
            error: (err) => {
              this.registerForm.disable({ onlySelf: true });
            },
          });
      } else {
        this.router.navigateByUrl("/404");
      }
    });
  }

  onRegister() {
    if (this.registerForm.invalid) {
      return;
    }

    const registerData = this.registerForm.getRawValue();

    this.submitButton.disabled = true;
    this.progressBar.mode = "indeterminate";

    this.authService.loginUser("api/v1/auth/register", registerData).subscribe({
      next: (res) => {
        this.registrationConfirmed = true;
        //this.router.navigateByUrl("/login");
        this.progressBar.mode = "determinate";
      },
      error: (err: HttpErrorResponse) => {
        this.submitButton.disabled = false;
        this.progressBar.mode = "determinate";
        this.errorMsg = err.message;
      },
    });
  }

  setupRegisterForm() {
    const fullName = new UntypedFormControl("", Validators.required);
    const password = new UntypedFormControl("", [
      Validators.required,
      Validators.pattern(
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/
      ),
      Validators.minLength(6),
    ]);
    const confirmPassword = new UntypedFormControl("", Validators.required);

    this.registerForm = new UntypedFormGroup(
      {
        userId: new UntypedFormControl(""),
        code: new UntypedFormControl(""),
        fullName: fullName,
        email: new UntypedFormControl("", [
          Validators.required,
          Validators.email,
        ]),
        password: password,
        confirmPassword: confirmPassword,
        agreed: new UntypedFormControl("", (control: UntypedFormControl) => {
          const agreed = control.value;
          if (!agreed) {
            return { agreed: true };
          }
          return null;
        }),
      },
      {
        validators: [Validation.match("password", "confirmPassword")],
      }
    );
  }
}

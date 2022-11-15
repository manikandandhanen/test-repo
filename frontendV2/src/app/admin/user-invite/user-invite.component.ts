import { ENTER, COMMA } from "@angular/cdk/keycodes";
import { Component, Inject, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { UserService } from "../shared/user.service";
import { InviteUser, User } from "../shared/user.model";
import { forkJoin, Observable } from "rxjs";
import RegexValidator from "app/shared/utilities/regex-validator";
import { LoaderService } from "../../shared/loader.service";
import { AlertService } from "../../shared/alert.service";

@Component({
  selector: "app-user-invite",
  templateUrl: "./user-invite.component.html",
  styleUrls: ["./user-invite.component.scss"],
})
export class UserInviteComponent implements OnInit {
  userForm!: FormGroup;
  public seperatorKeysCodes: number[] = [ENTER, COMMA];
  public emailList = [];
  public emailListErrors = [];
  removable = true;
  actionBtn: string = "Invite";
  formTitle: string = "Invite users:";
  loading$ = this.loader.loading$;

  constructor(
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _dialogRef: MatDialogRef<UserInviteComponent>,
    public loader: LoaderService,
    public alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.userForm = this._formBuilder.group({
      email: [null],
      userRole: ["", [Validators.required]],
      requestProfile: [false],
    });
  }

  onEmailPaste(event: ClipboardEvent): void {
    event.preventDefault();
    this.emailListErrors.length = 0;
    var emails = event.clipboardData.getData("Text").split(/;|,|\n/);
    emails.forEach((value) => this.addEmailToList(value));
  }

  onEmailAdd(event: any): void {
    this.emailListErrors.length = 0;
    this.addEmailToList(event.value);
    if (event.input) {
      event.input.value = "";
    }
  }

  onEmailRemove(data: any): void {
    if (this.emailList.indexOf(data) >= 0) {
      this.emailList.splice(this.emailList.indexOf(data), 1);
    }
  }

  onInvite(): void {
    if (this.emailList.length == 0) {
      this.alertService.showError("email is required", "error");
      return;
    }

    if (this.userForm.valid) {
      let user: InviteUser = {
        email: null,
        userRole: this.userForm.get("userRole").value,
        requestProfile: this.userForm.get("requestProfile").value,
      };

      let Observables: Observable<User>[] = [];

      this.emailList
        .filter((email) => {
          return !email.invalid;
        })
        .forEach((email) => {
          user.email = email.value;

          var inviteUserObs = this._userService.inviteUser(user);
          Observables.push(inviteUserObs);
        });

      forkJoin(Observables).subscribe((users) => {
        this.userForm.reset();
        this._dialogRef.close({
          event: "inviteUser",
          data: { users: users },
        });
      });
    }
  }

  private addEmailToList(value: string): void {
    let email = value.trim();
    if (email) {
      if (this.emailList.some((e) => e.value === email)) {
        this.emailListErrors.push(`Duplicate:  ${email} removed!`);
        this.alertService.showWarning(`Duplicate:  ${email} removed!`);
        return;
      }

      let isValid = RegexValidator.validateEmail(email);
      if (!isValid) {
        this.alertService.showWarning(`Invalid:  ${email} removed!`);
        this.emailListErrors.push(`Invalid:  ${email} removed!`);
        return;
      }

      this._userService.emailExists(email).subscribe((exists) => {
        if (!exists) {
          this.emailList.push({
            value: email,
            invalid: !isValid,
          });
        } else {
          this.emailListErrors.push(`Already Exists:  ${email} removed!`);
          this.alertService.showWarning(`Already Exists:  ${email} removed!`);
        }
      });
    }
  }
}

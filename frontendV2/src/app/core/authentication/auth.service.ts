//https://github.com/CodeMazeBlog/angular-identity-aspnetcore-security/tree/angular-role-based-authorization/AngularClient/src/app
//npm i @auth0/angular-jwt
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {
  AuthResponseDto,
  UserForRegistrationDto,
  RegistrationResponseDto,
  UserForAuthenticationDto,
  UserForgotPassword,
} from "app/account/shared/account.model";
import { Subject } from "rxjs";
import { JwtHelperService } from "@auth0/angular-jwt";
import { EnvironmentUrlService } from "app/shared/environment-url.service";
import { User } from "app/admin/shared/user.model";

@Injectable({
  providedIn: "root",
})
export class AuthenticationService {
  
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(
    private http: HttpClient,
    private envUrl: EnvironmentUrlService,
    private jwtHelper: JwtHelperService
  ) {}

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    return this.http.post<RegistrationResponseDto>(
      this.createCompleteRoute(route, this.envUrl.authApiURI),
      body
    );
  };

  public verifyRegistrationToken = (route: string) => {
    return this.http.get<User>(
      this.createCompleteRoute(route, this.envUrl.authApiURI)
    );
  };

  public verifyResetPasswordToken(route: string) {
    return this.http.get<any>(
      this.createCompleteRoute(route, this.envUrl.authApiURI)
    );
  }

  public loginUser = (route: string, body: UserForAuthenticationDto) => {
    return this.http.post<AuthResponseDto>(
      this.createCompleteRoute(route, this.envUrl.authApiURI),
      body
    );
  };

  public resetPassword = (route: string, body: UserForgotPassword) => {
    return this.http.post<any>(
      this.createCompleteRoute(route, this.envUrl.authApiURI),
      body
    );
  };

  public forgotPassword = (route: string, body: UserForgotPassword) => {
    return this.http.post<any>(
      this.createCompleteRoute(route, this.envUrl.authApiURI),
      body
    );
  };

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  };

  public logout = () => {
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
  };

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");

    return token && !this.jwtHelper.isTokenExpired(token);
  };

  public isUserAdmin = (): boolean => {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token);
    const role =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];

    return role === "admin";
  };

  public loggedInUserFullName = (): string => {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token);

    //console.log(decodedToken);

    const userFullName =
      decodedToken[
        // "http://schemas.microsoft.com/ws/2008/06/identity/claims/name"
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
      ];

    return userFullName || "Ananymous";
  };

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  };
}

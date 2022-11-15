import { Routes } from "@angular/router";

import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { NotFoundComponent } from "./not-found/not-found.component";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { ResetPasswordComponent } from "./reset-password/reset-password.component";

export const AccountRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "register/:id",
        component: RegisterComponent,
        data: { title: "register" },
      },
      {
        path: "login",
        component: LoginComponent,
        data: { title: "login" },
      },
      {
        path: "forgot-password",
        component: ForgotPasswordComponent,
        data: { title: "forgot password" },
      },
      {
        path: "reset-password",
        component: ResetPasswordComponent,
        data: { title: "reset password" },
      },
      // {
      //   path: "404",
      //   component: NotFoundComponent,
      //   data: { title: "Not Found" },
      // },
      // {
      //   path: "**",
      //   component: LoginComponent,
      // },
    ],
  },
];

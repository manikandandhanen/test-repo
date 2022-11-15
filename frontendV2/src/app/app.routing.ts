import { Routes } from "@angular/router";
import { NotFoundComponent } from "./account/not-found/not-found.component";
import { AdminGuard } from "./core/authentication/admin.guard";
import { AuthGuard } from "./core/authentication/auth.guard";
import { AdminLayoutComponent } from "./layouts/components/layouts/admin-layout/admin-layout.component";
import { AuthLayoutComponent } from "./layouts/components/layouts/auth-layout/auth-layout.component";
//import { AuthGuard } from './layouts/guards/auth.guard';

export const rootRouterConfig: Routes = [
  {
    path: "",
    redirectTo: "dashboard",
    pathMatch: "full",
  },
  {
    path: "login",
    redirectTo: "account/login",
    pathMatch: "full",
  },
  {
    path: "users",
    redirectTo: "admin/users",
    pathMatch: "full",
  },

  // {
  //   path: 'profile',
  //   redirectTo: 'user/profile',
  //   pathMatch: 'full'
  // },
  {
    path: "account",
    component: AuthLayoutComponent,
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./account/account.module").then((m) => m.AccountModule),
        data: { title: "Account" },
      },
    ],
  },
  {
    path: "dashboard",
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
        data: { title: "Dashboard" },
      },
    ],
  },
  {
    path: "user",
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./user/user.module").then((m) => m.UserModule),
        data: { title: "User" },
      },
    ],
  },
  {
    path: "admin",
    component: AdminLayoutComponent,
    canActivate: [AuthGuard, AdminGuard],
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./admin/admin.module").then((m) => m.AdminModule),
        data: { title: "Admin" },
      },
    ],
  },
  // {
  //   path: 'register/:id',
  //   loadChildren: () =>
  //     import('src/app/password/password.module').then(
  //       m => m.PasswordModule
  //     ),
  //   data: { icon: 'assignment', text: 'Password' }
  // },
  {
    path: "**",
    component: NotFoundComponent,
  },
];

// export const rootRouterConfig1: Routes = [
//   // {
//   //   path: '',
//   //   redirectTo: 'account/login',
//   //   pathMatch: 'full'
//   // },
//   {
//     path: "",
//     component: AuthLayoutComponent,
//     children: [
//       {
//         path: "account",
//         loadChildren: () =>
//           import("./account/account.module").then((m) => m.AccountModule),
//         data: { title: "Account" },
//       },
//     ],
//   },
//   {
//     path: "",
//     component: AdminLayoutComponent,
//     canActivate: [AuthGuard],
//     children: [
//       {
//         path: "",
//         loadChildren: () =>
//           import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
//         data: { title: "Dashboard", breadcrumb: "DASHBOARD" },
//       },
//       {
//         path: "user",
//         loadChildren: () =>
//           import("./user/user.module").then((m) => m.UserModule),
//         data: { title: "User", breadcrumb: "User" },
//       },

//       {
//         path: "search",
//         loadChildren: () =>
//           import("./layouts/search-view/search-view.module").then(
//             (m) => m.SearchViewModule
//           ),
//       },
//     ],
//   },
//   {
//     path: "**",
//     redirectTo: "account/404",
//   },
// ];

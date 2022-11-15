import { Routes } from "@angular/router";

import { ProfileComponent } from "./profile/profile/profile.component";

export const UserRoutes: Routes = [
  {
    path: "",
    redirectTo: "profile",
    pathMatch: "full",
  },
  {
    path: "",
    children: [
      {
        path: "profile/:id",
        component: ProfileComponent,
        data: { title: "Profile", breadcrumb: "PROFILE" },
      },
    ],
  },
];

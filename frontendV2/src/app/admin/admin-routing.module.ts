import { Routes } from "@angular/router";
import { UserTableComponent } from "./user-table/user-table.component";

export const AdminRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "users",
        component: UserTableComponent,
        data: { title: "Users", breadcrumb: "Users" },
      },
    ],
  },
];

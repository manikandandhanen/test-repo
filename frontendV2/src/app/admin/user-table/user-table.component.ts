import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { UserInviteComponent } from "../user-invite/user-invite.component";
import { UserService } from "../shared/user.service";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { SelectionModel } from "@angular/cdk/collections";
import { User, UserStatus } from "../shared/user.model";

import { DialogService } from "app/shared/dialog.service";
import { UserEditRoleComponent } from "../user-edit-role/user-edit-role.component";
import { LoaderService } from "../../shared/loader.service";
import { AlertService } from "../../shared/alert.service";

@Component({
  selector: "app-user-table",
  templateUrl: "./user-table.component.html",
  styleUrls: ["./user-table.component.scss"],
})
export class UserTableComponent implements OnInit {
  displayedColumns: string[] = [
    "select",
    "email",
    "userRole",
    "status",
    "dateCreated",
    "Action",
  ];
  dataSource: MatTableDataSource<User>;
  statusList: string[] = ["Invited", "Active", "Deactivated", "Revoked"];
  user: User = new User();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  selection = new SelectionModel<User>(true, []);
  loading$ = this.loader.loading$;
  selectedStatus: string[] = [];

  constructor(
    private userService: UserService,
    public dialog: MatDialog,
    private dialogService: DialogService,
    public loader: LoaderService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadUserDataSource();
  }

  ngOnDestroy(): void {
    this.selection.changed.unsubscribe();
  }

  // formSubscribe() {
  //   this.status.valueChanges.subscribe((statusValue) => {
  //     this.filterValues['status'] = statusValue;
  //     this.dataSource.filter = JSON.stringify(this.filterValues);
  //   });
  // }

  onInviteUser() {
    this.dialog
      .open(UserInviteComponent, {
        disableClose: true,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result && result.event == "inviteUser") {
          result.data.users.forEach((user) => {
            if (user.isInviteSuccessful) {
              this.dataSource.data.push(user.invitedUser);
            }
            this.alertService.showSuccess(user.message);
          });
          //refresh

          this.dataSource.data = this.dataSource.data.map((o) => {
            return o;
          });
        }
      });
  }

  onEditUser(row: any) {
    this.dialog
      .open(UserEditRoleComponent, {
        data: row,
        disableClose: true,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result.event == "editUserRole") {
          row.userRoles = result.data.userRoles;
        } else if (result === "save") {
          this.loadUserDataSource();
        }
      });
  }

  loadUserDataSource() {
    this.userService.getAll().subscribe({
      next: (res: any) => {
        this.dataSource = new MatTableDataSource(res);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
    });
  }

  isActionSelected(action: string): boolean {
    return !this.selection.selected.some(
      (user: any) => user.userStatus === action
    );
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  applyUserStatusFilter() {
    //const filterValue = (event.target as HTMLInputElement).value;

    // this.filterValues['status'] = this.selectedStatus;
    // this.dataSource.filter = JSON.stringify(this.filterValues);

    // this.dataSource.filter = JSON.stringify(this.filterValues);

    // this.dataSource.filter = JSON.stringify({status: this.selectedStatus });
    console.log(this.selectedStatus);
    return;
    // if (this.selectedStatus.length == 0) {
    //   this.dataSource.filteredData = this.dataSource.data;
    //   return;
    // }

    // this.dataSource.filteredData = this.dataSource.data.filter((o) => {
    //   return this.selectedStatus.indexOf(o.userStatus) != -1;
    // });

    // if (this.dataSource.paginator) {
    //   this.dataSource.paginator.firstPage();
    // }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  updateSelectedUsersStatus(fromStatus: string, toStatus: string) {
    this.selection.selected
      .filter((user) => user.userStatus == fromStatus)
      .forEach((user) => {
        let userStatus: UserStatus = {
          userId: user.userId,
          status: toStatus,
        };

        this.userService.updateUserStatus(userStatus).subscribe((_res: any) => {
          if (_res) {
            user.userStatus = toStatus;
            this.alertService.showSuccess("user status updated successfully");
          }
        });
      });
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
    console.log(this.selection.selected);
  }

  onDeleteUser(row: User): void {
    this.dialogService
      .confirmDialog({
        title: "Delete Confirmation!",
        message: `Are you sure to delete  ` + row.email,
        confirmCaption: "Yes",
        cancelCaption: "No",
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.userService.deleteUser(row.userId).subscribe((data) => {
            this.dataSource.data = this.dataSource.data.filter(
              (u) => u.userId !== row.userId
            );
            this.alertService.showSuccess("User deleted successfully");
          });
        }
      });
  }

  // getFormsValue() {
  //   this.dataSource.filterPredicate = (data, filter: string): boolean => {
  //     let searchString = JSON.parse(filter);
  //     let isPositionAvailable = false;
  //     if (searchString.status.length) {
  //       for (const d of searchString.status) {
  //         if (data.userStatus.trim() === d) {
  //           isPositionAvailable = true;
  //         }
  //       }
  //     } else {
  //       isPositionAvailable = true;
  //     }
  //     return isPositionAvailable;

  //   }
  //   this.dataSource.filter = JSON.stringify(this.filterValues);
  // }
}

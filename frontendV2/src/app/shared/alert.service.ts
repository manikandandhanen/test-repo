import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { timeout } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class AlertService {
  constructor(private toastr: ToastrService) {}

  showSuccess(message: string): void {
    this.toastr.success(message);
  }

  showError(message: string, type: string): void {
    this.toastr.error(message, type, {
      timeOut: 100000,
      extendedTimeOut: 0,
    });
  }

  showInfo(message: string): void {
    this.toastr.info(message);
  }

  showWarning(message: string): void {
    this.toastr.warning(message);
  }
}

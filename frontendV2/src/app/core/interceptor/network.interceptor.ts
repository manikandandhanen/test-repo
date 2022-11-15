import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { finalize } from "rxjs/operators";
import { LoaderService } from "../../shared/loader.service";

@Injectable()
export class NetworkInterceptor implements HttpInterceptor {
  constructor(public loader: LoaderService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.loader.showLoader();
    return next.handle(request).pipe(
      finalize(() => {
        this.loader.hideLoader();
      })
    );
  }
}

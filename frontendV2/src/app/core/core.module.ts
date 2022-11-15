import { NgModule, Optional, SkipSelf } from '@angular/core'; 
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from 'environments/environment';
import {  AdminGuard } from './authentication/admin.guard';
import { AuthGuard } from './authentication/auth.guard';
import { AuthenticationService } from './authentication/auth.service';
 
export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  imports: [
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: environment.allowedDomains,
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    AdminGuard,
    AuthGuard  ,
    AuthenticationService
  ]
})
export class CoreModule {

  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    // Import guard
    if (parentModule) {
      throw new Error(`${parentModule} has already been loaded. Import Core module in the AppModule only.`);
    }
  }

}
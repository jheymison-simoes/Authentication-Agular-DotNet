import { AuthenticationService } from './../../client/login/authentication.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserAuthenticatedGuard implements CanActivate{
    constructor(
      private authenticationService: AuthenticationService,
      private router: Router) { }

    canActivate(){
      if (this.authenticationService.logado) {
        return true;
      }
      this.router.navigate(['login']);
      return false;
    }
}

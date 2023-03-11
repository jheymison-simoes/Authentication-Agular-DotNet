import { AuthenticationService } from './../../client/login/authentication.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserNotAuthenticatedGuard implements CanActivate{
    constructor(
      private authenticationService: AuthenticationService,
      private router: Router) { }
    canActivate(){
      if (this.authenticationService.logado) {
        this.router.navigate(['']);
        return false;
      }
      return true;
    }
}
